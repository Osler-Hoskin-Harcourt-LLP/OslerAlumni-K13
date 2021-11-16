using System;
using CMS;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Search;
using CMS.Search.Azure;
using ECA.Admin.Core.Modules;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Repositories;

[assembly: RegisterModule(typeof(PageCacheBustingModule))]

namespace OslerAlumni.Admin.Core.Modules
{
    public class PageCacheBustingModule
        : BaseModule
    {
        #region "Properties"

        public IDocumentRepository DocumentRepository { get; set; }

        public IKenticoSearchIndexRepository SearchIndexRepository { get; set; }

        public ICacheService CacheService { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PageCacheBustingModule"/> class.
        /// </summary>
        public PageCacheBustingModule()
            : base($"{GlobalConstants.ModulePrefix}.{nameof(PageCacheBustingModule)}")
        {
        }

        /// <summary>
        /// Called when [initialize].
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();

            DocumentEvents.ChangeOrder.After += DocumentChangeOrder_After;

            SearchTaskAzureInfo.TYPEINFO.Events.Delete.After += AzureSearchTaskDelete_After;
        }

        #region "Events"

        protected void DocumentChangeOrder_After(
            object sender,
            DocumentChangeOrderEventArgs e)
        {
            var page = e.Node;

            if (page == null)
            {
                return;
            }

            var aliasPathSections = (page.NodeAliasPath ?? string.Empty)
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var parentAliasPath = string.Empty;

            // Go through all ancestor levels for the page, whose node order was modified.
            // The reason for skipping the last 2 sections of the alias path is:
            // - the last section represents the page that has been modified,
            // so we don't need to touch its key (it will be handled by Kentico);
            // - the penultimate section represents the direct parent of the page that
            // has been modified, and Kentico already handles touching cache keys of direct 
            // parents correctly.
            for (var i = 0; i < aliasPathSections.Length - 2; i++)
            {
                parentAliasPath += "/" + aliasPathSections[i];

                // Touch the cache key of the format "node|<site name>|<alias path>|childnodes",
                // which should be used as a dependency when we need cache busted on a change to
                // any of the child pages in the content tree hierarchy
                CacheService.TouchCacheKeys(
                    $"node|{page.NodeSiteName}|{parentAliasPath}|childnodes");
            }
        }

        protected void AzureSearchTaskDelete_After(
            object sender,
            ObjectEventArgs e)
        {
            var searchTask = e.Object as SearchTaskAzureInfo;

            if (searchTask == null)
            {
                return;
            }

            // Making sure that the Azure search task that was just deleted
            // (presumably after being processed and reflected in the Azure index)
            // was actually related to a page
            if (!string.Equals(
                    searchTask.SearchTaskAzureObjectType,
                    TreeNode.OBJECT_TYPE,
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (string.Equals(
                    searchTask.SearchTaskAzureMetadata,
                    "_id",
                    StringComparison.OrdinalIgnoreCase))
            {
                // This field stores document and node IDs as a semicolon-separated list
                var ids = (searchTask.SearchTaskAzureAdditionalData ?? string.Empty)
                    .Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

                if (ids.Length == 2)
                {
                    var nodeId = ValidationHelper.GetInteger(ids[1], 0);

                    if (nodeId > 0)
                    {
                        string siteName;

                        var pageType =
                            DocumentRepository.GetDocumentClassName(nodeId, out siteName);

                        // If we are able to get page type and site information based on the 
                        // Azure search task, we should touch the associated cache key,
                        // which in turn should bust the cached listings of the pages of that type
                        if (!string.IsNullOrWhiteSpace(pageType)
                            && !string.IsNullOrWhiteSpace(siteName))
                        {
                            CacheService.TouchCacheKeys(
                                $"nodes|{siteName}|{pageType}|all");

                            return;
                        }
                    }
                }
            }

            // If we weren't able to get the document info,
            // based on which we could touch specific page type cache keys
            // (e.g. if the page had been deleted),
            // we should touch all Azure search index cache keys, 
            // so that related cached items are busted
            var azureIndexes =
                SearchIndexRepository.GetAzureSearchIndexes();

            if ((azureIndexes == null) || (azureIndexes.Count < 1))
            {
                return;
            }

            foreach (var azureIndex in azureIndexes)
            {
                CacheService.TouchCacheKeys(
                    $"{SearchIndexInfo.OBJECT_TYPE}|byname|{azureIndex.IndexName}");
            }
        }

        #endregion
    }
}

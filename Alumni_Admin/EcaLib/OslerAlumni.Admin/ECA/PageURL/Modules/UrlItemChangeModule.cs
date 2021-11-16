using System;
using CMS;
using CMS.CustomTables;
using ECA.Admin.Core.Modules;
using ECA.Admin.PageURL.Modules;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Repositories;
using ECA.PageURL.Kentico.Models;

[assembly: RegisterModule(typeof(UrlItemChangeModule))]
namespace ECA.Admin.PageURL.Modules
{
    public class UrlItemChangeModule
        : BaseModule
    {
        #region "Properties"

        public IDocumentRepository DocumentRepository { get; set; }

        public IEventLogRepository EventLogRepository { get; set; }

        public ICacheService CacheService { get; set; }

        #endregion

        public UrlItemChangeModule()
            : base($"{ECAGlobalConstants.Modules.PageURL}.{nameof(UrlItemChangeModule)}")
        { }

        protected override void OnInit()
        {
            base.OnInit();

            CustomTableItemEvents.Insert.After += Insert_After;
            CustomTableItemEvents.Update.After += Update_After;
            CustomTableItemEvents.Delete.After += Delete_After;
        }

        #region "Events"

        protected void Insert_After(object sender, CustomTableItemEventArgs e)
        {
            TouchPageCacheKeys(e.Item);
        }

        protected void Update_After(object sender, CustomTableItemEventArgs e)
        {
            TouchPageCacheKeys(e.Item);
        }

        protected void Delete_After(object sender, CustomTableItemEventArgs e)
        {
            TouchPageCacheKeys(e.Item);
        }

        #endregion

        #region "Methods"

        protected void TouchPageCacheKeys(
            CustomTableItem item)
        {
            if (item == null)
            {
                return;
            }

            try
            {
                // If this is not triggered by URL item insert/update/delete event, don't do anything
                if (!string.Equals(
                    item.CustomTableClassName,
                    CustomTable_PageURLItem.CLASS_NAME,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var urlItem = item as CustomTable_PageURLItem;

                if (urlItem == null)
                {
                    return;
                }

                // NOTE: We don't know what type of a page we are looking for, so this will use multi-doc query
                var page = DocumentRepository.GetDocument(
                    urlItem.NodeGUID,
                    ignorePublishedState: true,
                    cultureName: urlItem.Culture,
                    siteName: urlItem.Generalized.ObjectSiteName);

                if (page == null)
                {
                    return;
                }

                // Touch all of the cache keys related to the page, 
                // so that any cached items that depend on the changes made to the page
                // are refreshed and URL change is treated the same as any other change
                // made to the page data
                CacheService.TouchCacheKeys(page);
            }
            catch (Exception ex)
            {
                EventLogRepository.LogError(
                    GetType(),
                    nameof(TouchPageCacheKeys),
                    ex);
            }
        }

        #endregion
    }
}

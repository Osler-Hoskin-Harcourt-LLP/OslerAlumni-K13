using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.PageURL.Definitions;
using ECA.PageURL.Kentico.Models;

namespace ECA.PageURL.Services
{
    public class PageService
        : ServiceBase, IPageService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly IEventLogRepository _eventLogRepository;
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private readonly ICacheService _cacheService;
        private readonly IPageUrlService _pageUrlService;

        private readonly ContextConfig _context;

        #endregion

        public PageService(
            IDocumentRepository documenRepository,
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository,
            ICacheService cacheService,
            IPageUrlService pageUrlService,
            ContextConfig context)
        {
            _documentRepository = documenRepository;
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;
            _cacheService = cacheService;
            _pageUrlService = pageUrlService;

            _context = context;
        }

        #region "Methods"

        public bool TryGetPage(
            Guid nodeGuid,
            string cultureName,
            string siteName,
            out TreeNode page,
            string pageTypeName = null,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
        {
            page = null;

            if (nodeGuid == Guid.Empty)
            {
                return false;
            }

            cultureName = cultureName.ReplaceIfEmpty(_context.CultureName);
            siteName = siteName.ReplaceIfEmpty(_context.Site?.SiteName);

            var columnNameList = columnNames?.ToArray();

            var cacheParameters = new CacheParameters
            {
                // We are explicitly allowing for null values to be cached,
                // since this is a method that is going to be called on each request,
                // and the empty cached value will be busted on the change of any 
                // culture version of the page anyways
                AllowNullValue = true,
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Pages.PageByNodeGuidAndColumns,
                    nodeGuid,
                    columnNameList.JoinSorted(
                        ECAGlobalConstants.Caching.Separator,
                        ECAGlobalConstants.Caching.All),
                    includeAllCoupledColumns),
                IsCultureSpecific = true,
                CultureCode = cultureName,
                IsSiteSpecific = true,
                SiteName = siteName,
                // Bust the cache whenever the page is modified
                CacheDependencies = new List<string>
                {
                    $"nodeguid|{siteName}|{nodeGuid}"
                }
            };

            page = _cacheService.Get(
                () =>
                    _documentRepository.GetDocument(
                        nodeGuid,
                        pageTypeName,
                        false,
                        columnNameList,
                        includeAllCoupledColumns,
                        cultureName),
                cacheParameters);

            return true;
        }

        public bool TryGetPageCultureUrls(
            TreeNode page,
            bool excludeCurrent,
            bool ignorePublishedState,
            out Dictionary<string, string> cultureUrls,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
        {
            cultureUrls = null;

            if (page == null)
            {
                return false;
            }

            try
            {
                var columnNameList = columnNames?.ToArray();

                // Get all of the allowed cultures, except for the excluded one
                var cultures = !excludeCurrent
                    ? _context.AllowedCultureCodes
                    : _context.AllowedCultureCodes
                        .Where(c =>
                            !string.Equals(c.Value, page.DocumentCulture, StringComparison.OrdinalIgnoreCase));

                cultureUrls = new Dictionary<string, string>();

                foreach (var culture in cultures)
                {
                    // First check that the culture version of the page exists and is published
                    if (!ignorePublishedState)
                    {
                        TreeNode culturePage;
                        
                        if (!TryGetPage(
                                page.NodeGUID,
                                culture.Value,
                                _context.Site?.SiteName,
                                out culturePage,
                                page.NodeClassName,
                                columnNameList,
                                includeAllCoupledColumns)
                            || (culturePage == null))
                        {
                            continue;
                        }
                    }

                    string url;

                    if (!_pageUrlService.TryGetPageMainUrl(
                            page.NodeGUID,
                            culture.Value,
                            out url,
                            page.NodeSiteName))
                    {
                        continue;
                    }

                    cultureUrls[culture.Key] = url;
                }

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(TryGetPageCultureUrls),
                    ex);

                return false;
            }
        }

        public bool TryGetStandalonePage(
            StandalonePageType standalonePageType,
            string cultureName,
            string siteName,
            out TreeNode page,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
        {
            page = null;

            var nodeGuid = _settingsKeyRepository.GetValue<Guid>(
                standalonePageType.ToStringRepresentation());
            
            // NOTE: We don't know what type of a page we are looking for, so this will use multi-doc query
            return TryGetPage(
                nodeGuid,
                cultureName,
                siteName,
                out page,
                null,
                columnNames,
                includeAllCoupledColumns);
        }

        public bool TryGetUrlItemPage(
            CustomTable_PageURLItem urlItem,
            out TreeNode page,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
        {
            // NOTE: We don't know what type of a page we are looking for, so this will use multi-doc query
            return TryGetPage(
                urlItem.NodeGUID,
                urlItem.Culture,
                urlItem.Generalized.ObjectSiteName,
                out page,
                null,
                columnNames,
                includeAllCoupledColumns);
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Admin.PageURL.Repositories;
using ECA.Content.Repositories;
using ECA.Core.Services;
using ECA.PageURL.Kentico.Models;
using ECA.PageURL.Repositories;
using ECA.PageURL.Services;

namespace ECA.Admin.PageURL.Services
{
    public class PathChangeService
        : ServiceBase, IPathChangeService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly IPageUrlItemRepository _pageUrlItemRepository;
        private readonly IPathChangeItemRepository _pathChangeItemRepository;
        private readonly IPageUrlService _pageUrlService;

        #endregion

        public PathChangeService(
            IDocumentRepository documentRepository,
            IPageUrlItemRepository pageUrlItemRepository,
            IPathChangeItemRepository pathChangeItemRepository,
            IPageUrlService pageUrlService)
        {
            _documentRepository = documentRepository;
            _pageUrlItemRepository = pageUrlItemRepository;
            _pathChangeItemRepository = pathChangeItemRepository;
            _pageUrlService = pageUrlService;
        }

        #region "Methods"

        public bool IsPathChangeReverted(
            CustomTable_PathChangeItem pathChange,
            out TreeNode pathChangePage)
        {
            pathChangePage = null;

            if (pathChange == null)
            {
                return false;
            }

            // NOTE: We don't know what type of a page we are looking for, so this will use multi-doc query
            pathChangePage =
                _documentRepository.GetDocument(
                    pathChange.NodeGUID);

            if (pathChangePage == null)
            {
                return false;
            }

            return pathChangePage.DocumentNamePath
                .EqualsCSafe(
                    pathChange.OriginalDocumentNamePath,
                    true);
        }

        public void LogPathChange(
            TreeNode page)
        {
            var pathChangeItem = new CustomTable_PathChangeItem
            {
                NodeGUID = page.NodeGUID,
                Culture = page.DocumentCulture,
                OriginalDocumentNamePath = page.DocumentNamePath,
                SiteID = page.NodeSiteID
            };

            _pathChangeItemRepository.Save(pathChangeItem);
        }

        public bool TryGetPagesInPath(
            CustomTable_PathChangeItem pathChange,
            TreeNode pathChangePage,
            int count,
            out IList<TreeNode> pages)
        {
            pages = null;

            if ((pathChange == null) || (pathChangePage == null))
            {
                return false;
            }

            var urlPath = _pageUrlService.GetUrlPath(
                pathChange.OriginalDocumentNamePath,
                pathChange.SiteName);

            var urlItems = _pageUrlItemRepository
                .GetPageUrlItems(
                    // NOTE: the trailing slash is important in this case,
                    // as otherwise we might get a partial match (e.g. "/test" that matches URLs with prefix "/test-a", etc.,
                    // which would result in the scheduled task getting stuck on the same set of changes
                    $"{urlPath}/",
                    pathChange.Culture,
                    // We are only interested in updating the main URLs of the sub-pages
                    isMainOnly: true,
                    // We can only update non-custom main URLs of the sub-pages
                    isNonCustomOnly: true,
                    // NOTE: We need to pass in the node alias path, so that only URL items of the pages
                    // that are located UNDER the page that caused the path change are picked up
                    // (this wouldn't always be the case if we had another page in the content tree 
                    // that is named the same and is located at the same level,
                    // which in turn could result in the early termination of
                    // the scheduled task that is processing the path changes as it would keep pulling the same list of pages,
                    // but their URLs wouldn't change, since their ancestor's name didn't change)
                    nodeAliasPath: pathChangePage.NodeAliasPath,
                    count: count);

            if (DataHelper.DataSourceIsEmpty(urlItems))
            {
                return true;
            }

            // NOTE: We don't know what type of a page we are looking for, so this will use multi-doc query
            pages = _documentRepository
                .GetDocuments(
                    urlItems.Select(u => u.NodeGUID).ToList(),
                    pageTypeName: null,
                    ignorePublishedState: true,
                    cultureName: pathChange.Culture)
                .ToList();

            return true;
        }

        #endregion
    }
}

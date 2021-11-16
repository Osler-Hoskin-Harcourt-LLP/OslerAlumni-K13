using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class GenericPageService
        : ServiceBase, IGenericPageService
    {
        #region "Constants"

        private static string[] DefaultPageColumnNames
            => new[]
            {
                nameof(PageType_Page.Title),
                nameof(PageType_Page.Description),
                nameof(PageType_Page.DefaultAction),
                nameof(PageType_Page.DefaultController),
                nameof(PageType_Page.HasSuccessState),
                nameof(PageType_Page.SuccessHeader),
                nameof(PageType_Page.SuccessDescription)
            };

        #endregion

        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;

        #endregion

        public GenericPageService(
            IDocumentRepository documentRepository,
            ICacheService cacheService)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
        }

        #region "Methods"

        public PageType_Page GetSubPageByAlias(
            TreeNode page,
            string nodeAlias)
        {
            if ((page == null)
                || string.IsNullOrWhiteSpace(nodeAlias))
            {
                return null;
            }

            return GetSubPages(page)
                .FirstOrDefault(sp => 
                    string.Equals(sp.NodeAlias, nodeAlias, StringComparison.OrdinalIgnoreCase));
        }

        public IList<PageType_Page> GetSubPages(
            TreeNode page)
        {
            if (page == null)
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.GenericPages.SubPagesByPage,
                    page.NodeGUID),
                IsCultureSpecific = true,
                CultureCode = page.DocumentCulture,
                IsSiteSpecific = true,
                SiteName = page.NodeSiteName,
                // Bust the cache whenever any of the sub-pages is modified
                CacheDependencies = new List<string>
                {
                    $"node|{page.NodeSiteName}|{page.NodeAliasPath}|childnodes"
                }
            };

            var result = _cacheService.Get(
                () =>
                    _documentRepository
                        .GetChildDocuments<PageType_Page>(
                            page,
                            PageType_Page.CLASS_NAME,
                            false,
                            DefaultPageColumnNames)
                        ?.ToList(),
                cacheParameters);

            return result;
        }

        #endregion
    }
}

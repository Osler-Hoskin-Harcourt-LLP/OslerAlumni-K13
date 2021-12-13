using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Extensions;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.Mvc.Navigation.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class BreadCrumbService
        : ServiceBase, IBreadCrumbService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly IPageService _pageService;
        private readonly IPageUrlService _pageUrlService;
        private readonly ICacheService _cacheService;
        private readonly IEventLogRepository _eventLogRepository;
        private readonly ContextConfig _context;

        #endregion

        public BreadCrumbService(IDocumentRepository documentRepository,
            IPageService pageService,
            IPageUrlService pageUrlService,
            ICacheService cacheService,
            IEventLogRepository eventLogRepository,
            ContextConfig context)
        {
            _documentRepository = documentRepository;
            _pageService = pageService;
            _pageUrlService = pageUrlService;
            _cacheService = cacheService;
            _eventLogRepository = eventLogRepository;
            _context = context;
        }

        #region "Methods"

        public IEnumerable<NavigationItem> GetBreadCrumbs(Guid pageGuid)
        {
            var document =  _documentRepository.GetDocument(pageGuid);

            return GetBreadCrumbs(document.NodeAliasPath);
        }

        public IEnumerable<NavigationItem> GetBreadCrumbs(string nodeAliasPath)
        {
            if (string.IsNullOrWhiteSpace(nodeAliasPath))
            {
                return null;
            }

            List<NavigationItem> result = new List<NavigationItem>();

            //Add Home Page Breadcrumb
            var homePage = GetHomePageBreadCrumb();

            if (homePage != null)
            {
                result.Add(homePage);
            }
            
            //Add Remaining BreadCrumbs
            result.AddRange(GetBreadCrumbsRescursive(nodeAliasPath));

            return result;
        }

        private NavigationItem GetHomePageBreadCrumb()
        {
            TreeNode page;

            if (!_pageService.TryGetStandalonePage(
                StandalonePageType.Home,
                LocalizationContext.CurrentCulture.CultureCode,
                SiteContext.CurrentSiteName,
                out page,
                //columnNames: new []{nameof(PageType_Home.Title) },
                includeAllCoupledColumns: true))
            {
                _eventLogRepository.LogError(GetType(), nameof(GetBreadCrumbs),
                    $"Could not find a page corresponding to '{StandalonePageType.Home}' in the content tree.");

                return null;
            }
            
            string urlOutput = string.Empty;

            if (!_pageUrlService.TryGetPageMainUrl(
                StandalonePageType.Home,
                LocalizationContext.CurrentCulture.CultureCode,
                out urlOutput))
            {
                _eventLogRepository.LogError(GetType(), nameof(GetBreadCrumbs),
                    $"Could not find a page url corresponding to '{StandalonePageType.Home}' in the content tree.");

                return null;
            }

            var homePage = page.ToPageType<PageType_Home>();

            return new NavigationItem()
            {
                Title = homePage.PageName,
                Url = urlOutput
            };
        }

        private List<NavigationItem> GetBreadCrumbsRescursive(string nodeAliasPath)
        {
            var outputBreadCrumbList = new List<NavigationItem>();

            if (string.IsNullOrWhiteSpace(nodeAliasPath))
            {
                return outputBreadCrumbList;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Pages.PageByNodeAliasPath,
                    nodeAliasPath),
                IsCultureSpecific = true,
                IsSiteSpecific = true,
                // Bust the cache whenever the page is modified
                CacheDependencies = new List<string>
                {
                    $"node|{SiteContext.CurrentSiteName}|{nodeAliasPath}",
                }
            };

            var result = _cacheService.Get(
                (cp) =>
                {

                    //Add parent documents to the list recursively

                    var aliasList = nodeAliasPath
                        .Split(
                            new[] { '/' },
                            StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    if (aliasList.Count > 1)
                    {
                        aliasList.RemoveAt(aliasList.Count - 1);

                        var parentNodeAliasPath = $"/{string.Join("/", aliasList)}";
                        
                        outputBreadCrumbList.AddRange(GetBreadCrumbsRescursive(parentNodeAliasPath));
                    }


                    var ancestorPageAlias = string.Empty;

                    foreach (var alias in aliasList)
                    {
                        ancestorPageAlias += $"/{alias}";

                        // Bust the cache whenever the ancestor pages are modified.
                        cp.CacheDependencies.Add($"node|{SiteContext.CurrentSiteName}|{ancestorPageAlias}");
                    }


                    //Add current document to the list.

                    var document = _documentRepository
                        .GetDocuments(columnNames: new List<string>()
                            {
                                nameof(IBasePageType.Title)
                            },
                            whereCondition: new WhereCondition()
                                .WhereEquals(nameof(TreeNode.NodeAliasPath), nodeAliasPath)
                                //.WhereFalse(nameof(TreeNode.DocumentMenuItemHideInNavigation)) TODO##
                                .WhereIn(nameof(TreeNode.ClassName), Constants.OslerPageTypes.ToArray())
                        )
                        .FirstOrDefault();

                    if (document != null)
                    {
                        var basePage = document as IBasePageType;

                        string urlOutput = string.Empty;

                        if (_pageUrlService.TryGetPageMainUrl(document, out urlOutput))
                        {
                            outputBreadCrumbList.Add(new NavigationItem()
                            {
                                Title = basePage.Title,
                                Url = urlOutput
                            });
                        }

                    }


                    return outputBreadCrumbList;

                },
               cacheParameters);



            return result;
        }

        #endregion
    }
}

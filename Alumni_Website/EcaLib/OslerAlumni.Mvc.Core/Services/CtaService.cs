using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using DocumentFormat.OpenXml.Spreadsheet;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Extensions;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.PageURL.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class CtaService : ServiceBase, ICtaService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;
        private readonly IPageUrlService _pageUrlService;
        private readonly ISettingsKeyRepository _settingsKeyRepository;

        public CtaService(
            IAuthorizationService authorizationService,
            IDocumentRepository documentRepository,
            ICacheService cacheService,
            IPageUrlService pageUrlService,
            ISettingsKeyRepository settingsKeyRepository)
        {
            _authorizationService = authorizationService;
            _documentRepository = documentRepository;
            _cacheService = cacheService;
            _pageUrlService = pageUrlService;
            _settingsKeyRepository = settingsKeyRepository;
        }

        #region "Methods"

        public IEnumerable<CtaViewModel> GetRelatedContentCtas(TreeNode page)
        {
            return GetCtas(
                page,
                nameof(PageType_BasePageType.PageType_BasePageTypeFields.RelatedContentWidgetZone),
                replaceWithDefaultCtas: true);
        }

        public IEnumerable<CtaViewModel> GetTopWidgetZoneCtas(TreeNode page)
        {
            return GetCtas(
                page,
                nameof(PageType_BasePageType.PageType_BasePageTypeFields.TopWidgetZone));
        }

        public List<PageType_CTA> GetDefaultCtas(List<Guid> linkedPagesToExclude =null)
        {
            var path = _settingsKeyRepository.GetValue<string>(GlobalConstants.Settings.Ctas.DefaultCtasPath)
                .ReplaceIfEmpty("/");
            
            path =
                $"{path}{(path.EndsWith("/") ? string.Empty : "/")}%";

            var defaultCtas =
                _documentRepository.GetDocuments(PageType_CTA.CLASS_NAME, includeAllCoupledColumns: true, path: path);

            List<PageType_CTA> defaultPageTypeCtas = defaultCtas?.Select(cta => cta.ToPageType<PageType_CTA>()).ToList();

            if (linkedPagesToExclude != null && linkedPagesToExclude.Count > 0)
            {
                defaultPageTypeCtas = defaultPageTypeCtas?.Where(ctaPage => !linkedPagesToExclude.Contains(ctaPage.PageGUID)).ToList();
            }

            return defaultPageTypeCtas;
        }

        #endregion

        #region "Helper methods"

        protected List<CtaViewModel> GetCtas(
            TreeNode page,
            string fieldName, bool replaceWithDefaultCtas = false)
        {
            if (page == null)
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.CTA.CTAByPageAndFieldName,
                    page.NodeGUID,
                    fieldName,
                    _authorizationService.CurrentUserHasCompetitorRole()),
                IsCultureSpecific = true,
                CultureCode = page.DocumentCulture,
                IsSiteSpecific = true,
                SiteName = page.NodeSiteName,
                // Bust the cache whenever the related page is modified
                CacheDependencies = new List<string>
                {
                    $"nodeid|{page.NodeID}|relationships"
                }
            };

            if (!string.IsNullOrWhiteSpace(page.ClassName))
            {
                // Bust the cache whenever the page type relationship itself is changed for the page's page type
                cacheParameters.CacheDependencies.Add(
                    string.Format(
                        ECAGlobalConstants.Caching.Classes.RelationshipByClassAndField,
                        page.ClassName,
                        fieldName));
            }

            var result = _cacheService.Get(
                cp =>
                {
                    IList<RelationshipInfo> relationships;

                    var ctaPages = _documentRepository
                        .GetRelatedDocuments<PageType_CTA>(
                            page,
                            fieldName,
                            out relationships,
                            PageType_CTA.CLASS_NAME,
                            false,
                            new[]
                            {
                                nameof(PageType_CTA.Title),
                                nameof(PageType_CTA.Content),
                                nameof(PageType_CTA.LinkText),
                                nameof(PageType_CTA.ExternalURL),
                                nameof(PageType_CTA.PageGUID),
                                nameof(PageType_CTA.Image),
                                nameof(PageType_CTA.ImageAltText)
                            })
                        ?.ToList();

                    if (ctaPages == null || ctaPages.Count == 0)
                    {
                        return null;
                    }

                    int originalAddedCtasCount = ctaPages.Count;

                    //Ensure the CTAs don't have self reference.
                    ctaPages = ctaPages.Where(ctaPage => ctaPage.PageGUID != page.NodeGUID).ToList();

                    if (relationships != null)
                    {
                        // NOTE: We are using relationship information directly,
                        // so that cache dependencies are added even if
                        // the pages in the relationship are not currently in the published state,
                        // and are therefore not going to be part of the cached data
                        foreach (var relationship in relationships)
                        {
                            // Bust the cache whenever the CTA page itself is modified
                            cp.CacheDependencies.Add(
                                $"nodeid|{relationship.RightNodeId}");

                            // Bust the cache whenever the relationship itself
                            // (e.g. order of related pages) is modified
                            cp.CacheDependencies.Add(
                                $"{RelationshipInfo.OBJECT_TYPE_ADHOC}|byid|{relationship.RelationshipID}");
                        }
                    }

                    var ctaViewModels = ToCtaViewModels(ctaPages);

                    if (replaceWithDefaultCtas)
                    {
                        //Replace the number of CTAs taken out with the number of Default CTAs
                        var numberOfDefaultCtasToAdd = originalAddedCtasCount - ctaViewModels.Count;

                        if (numberOfDefaultCtasToAdd > 0)
                        {
                            //Ensure the default CTAs don't reference same pages already referenced by other CTAs.
                            var pagesToExclude = ctaViewModels.Where(vm => vm.LinkedPageGuid.HasValue)
                                .Select(vm => vm.LinkedPageGuid.Value).ToList();

                            //Ensure the default CTAs don't have self reference.
                            pagesToExclude.Add(page.NodeGUID);

                            var defaultCtaViewModels = ToCtaViewModels(GetDefaultCtas(pagesToExclude))?.Take(numberOfDefaultCtasToAdd).ToList();

                            if (defaultCtaViewModels != null)
                            {
                                ctaViewModels.AddRange(defaultCtaViewModels);
                            }
                        }
                    }

                    foreach (var ctaViewModel in ctaViewModels)
                    {
                        // Bust the cache whenever CTA is modified
                        cp.CacheDependencies.Add($"nodeguid|{page.NodeSiteName}|{ctaViewModel.CtaGuid}");

                        if (ctaViewModel.LinkedPageGuid.HasValue)
                        {
                            // Bust the cache whenever current culture version of
                            // the page referenced from the CTA is modified
                            cp.CacheDependencies.Add($"nodeguid|{page.NodeSiteName}|{ctaViewModel.CtaGuid}");
                        }
                    }

                    return ctaViewModels;
                },
                cacheParameters);

            return result;
        }

        protected List<CtaViewModel> ToCtaViewModels(
            List<PageType_CTA> ctaPages)
        {
            if (ctaPages == null || ctaPages.Count < 1)
            {
                return new List<CtaViewModel>();
            }

            // NOTE: We don't know what type of a page the CTA is linked to, so this will use multi-doc query
            // Pulling in the pages to ensure that we are only linking to published documents
            var linkedPages = _documentRepository
                .GetDocuments(
                    ctaPages
                        .Where(ctaPage => ctaPage.PageGUID != Guid.Empty)
                        .Select(ctaPage => ctaPage.PageGUID), includeAllCoupledColumns:true)
                ?.ToList();

            //Filter out proteced resources from competitors
            linkedPages = linkedPages?.Where(linkedPage =>
            {
                if (linkedPage is ICompetitorProtected)
                {
                    var competitorProtectedPage = (ICompetitorProtected) linkedPage;

                    if (competitorProtectedPage.HideFromCompetitors &&
                        _authorizationService.CurrentUserHasCompetitorRole())
                    {
                        return false;
                    }
                }

                return true;

            }).ToList();

            var linkedPageUrls = linkedPages
                ?.ToDictionary(
                    linkedPage => linkedPage.NodeGUID,
                    linkedPage =>
                    {
                        string url;

                        _pageUrlService.TryGetPageMainUrl(
                            linkedPage,
                            out url);

                        return url;
                    });

            return ctaPages
                .Select(ctaPage =>
                {
                    string url = null;

                    if ((linkedPageUrls != null) && (ctaPage.PageGUID != Guid.Empty))
                    {
                        if (!linkedPageUrls.TryGetValue(ctaPage.PageGUID, out url))
                        {
                            // If the CTA is linked to a page that no longer exists,
                            // or is not published yet,
                            // or is protected from competitor
                            // we should exclude that CTA item
                            return null;
                        }
                    }

                    bool? isExternal = null;

                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        isExternal = false;
                    }
                    else if (!string.IsNullOrWhiteSpace(ctaPage.ExternalURL))
                    {
                        isExternal = true;
                        url = ctaPage.ExternalURL;
                    }

                    return new CtaViewModel
                    {
                        Title = ctaPage.Title,
                        Url = URLHelper.ResolveUrl(url),
                        Content = ctaPage.Content,
                        Image = URLHelper.ResolveUrl(ctaPage.Image),
                        ImageAltText = ctaPage.ImageAltText,
                        LinkText = ctaPage.LinkText,
                        IsExternal = isExternal,
                        CtaGuid = ctaPage.NodeGUID,
                        LinkedPageGuid = ctaPage.PageGUID == Guid.Empty ? (Guid?) null: ctaPage.PageGUID
                    };
                })
                .Where(ctaViewModel => ctaViewModel != null)
                .ToList();
        }

        #endregion
    }
}

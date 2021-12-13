using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Extensions;
using ECA.Content.Repositories;
using ECA.Core.Models;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class BoardOpportunityService
        : ServiceBase, IBoardOpportunityService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;
        private readonly ContextConfig _context;

        #endregion

        public BoardOpportunityService(
            IDocumentRepository documentRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
            _context = context;
        }

        #region "Methods"

        public List<PageType_BoardOpportunity> GetLatestBoardOpportunities(int top)

        {
            if (top <= 0)
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Prefix +
                           $"{nameof(BoardOpportunityService)}|{nameof(GetLatestBoardOpportunities)}|top|{top}",
                AllowNullValue = false,
                CultureCode = LocalizationContext.CurrentCulture.CultureCode,
                CacheDependencies = new List<string>()
                {
                    string.Format(GlobalConstants.Caching.Pages.PagesByType, SiteContext.CurrentSiteName,
                        PageType_BoardOpportunity.CLASS_NAME)
                }
            };

            var result = _cacheService.Get(
                cp =>
                {

                    var featuredItems = _documentRepository
                        .GetDocuments(
                            pageTypeName: PageType_BoardOpportunity.CLASS_NAME,
                            columnNames:
                            new[]
                            {
                                nameof(PageType_BoardOpportunity.Title),
                                nameof(PageType_BoardOpportunity.PostedDate),
                                nameof(PageType_BoardOpportunity.BoardOpportunityLocation),
                                nameof(PageType_BoardOpportunity.ExternalUrl)
                            },
                            top: top,
                            orderDirection: OrderDirection.Descending,
                            orderByColumns: new[] { nameof(PageType_BoardOpportunity.PostedDate) });

                    return featuredItems.Select(fi => fi.ToPageType<PageType_BoardOpportunity>()).ToList();
                },
                cacheParameters);

            return result;
        }

        #endregion

    }
}

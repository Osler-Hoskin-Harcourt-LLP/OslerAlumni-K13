using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Extensions;
using ECA.Content.Repositories;
using ECA.Core.Models;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class EventsService
        : ServiceBase, IEventsService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;
        private readonly ContextConfig _context;

        #endregion

        public EventsService(
            IDocumentRepository documentRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
            _context = context;
        }

        #region "Methods"
        

        public List<PageType_Event> GetLatestEvents(int top, bool filterForCompetitor)
        {
            if (top <= 0)
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Prefix +
                           $"{nameof(EventsService)}|{nameof(GetLatestEvents)}|top|{top}|filterForCompetitor|{filterForCompetitor}",
                AllowNullValue = false,
                CultureCode = _context.CultureName,
                CacheDependencies = new List<string>()
                {
                    string.Format(GlobalConstants.Caching.Pages.PagesByType, _context.Site.SiteName,
                        PageType_Event.CLASS_NAME)
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var whereCondition = new WhereCondition()
                        .WhereGreaterOrEquals(nameof(PageType_Event.EndDate), DateTime.Now);

                    if (filterForCompetitor)
                    {
                        whereCondition = whereCondition.WhereFalse(nameof(PageType_Event.HideFromCompetitors));
                    }

                    var featuredItems = _documentRepository
                        .GetDocuments(
                            pageTypeName: PageType_Event.CLASS_NAME,
                            cultureName: _context.CultureName,
                            columnNames:
                            new[]
                            {
                                nameof(PageType_Event.Title),
                                nameof(PageType_Event.EndDate),
                                nameof(PageType_Event.DisplayDate),
                                nameof(PageType_Event.HostedByOsler)
                            },
                            top: top,
                            orderDirection: OrderDirection.Ascending,
                            orderByColumns: new[] {nameof(PageType_Event.EndDate) },
                            whereCondition: whereCondition
                        );

                    return featuredItems.Select(featuredItem => featuredItem.ToPageType<PageType_Event>()).ToList();
                },
                cacheParameters);

            return result;
        }

        #endregion

    }
}

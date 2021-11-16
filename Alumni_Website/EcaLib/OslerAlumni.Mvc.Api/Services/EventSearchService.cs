using System;
using System.Collections.Generic;
using System.Linq;
using ECA.Caching.Models;
using ECA.Caching.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Api.Services
{
    public class EventSearchService
        : BaseSearchService<EventSearchRequest, Event>
    {
        #region "Private fields"

        private readonly ILocationItemRepository _locationItemRepository;

        #endregion

        public EventSearchService(
            ILocationItemRepository locationItemRepository,
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        {
            _locationItemRepository = locationItemRepository;
        }

        protected override SearchResponse<Event> GetSearchResults(
            EventSearchRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            var searchResponse = base.GetSearchResults(
                searchRequest,
                cacheParameters);

            if (searchResponse == null)
            {
                return null;
            }

            UpdateLocationDisplay(
                searchResponse.Items, 
                searchRequest.Culture,
                cacheParameters);

            return searchResponse;
        }

        #region "Helper methods"

        protected void UpdateLocationDisplay(
            List<Event> eventList,
            string cultureName,
            CacheParameters cacheParameters = null)
        {
            if ((eventList == null) || (eventList.Count < 1))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }

            var locations =
                _locationItemRepository
                    .GetAllLocationItems(cultureName)?
                    .ToDictionary(
                        loc => loc.ItemGUID,
                        loc => loc.Location)
                ??
                new Dictionary<Guid, string>();

            eventList.ForEach(e =>
            {
                string locationStr;

                locations
                    .TryGetValue(e.Location, out locationStr);

                var list = new List<string>() {e.City, locationStr}.Where(l => !string.IsNullOrWhiteSpace(l));

                e.LocationDisplay = string.Join(", ", list);
            });

            // Bust the cache if any of the locations or associated resource strings is modified
            cacheParameters?.CacheDependencies
                .Add(_cacheService.GetCacheKey(
                    GlobalConstants.Caching.Events.LocationItemsAll,
                    cultureName));
        }

        #endregion
    }
}

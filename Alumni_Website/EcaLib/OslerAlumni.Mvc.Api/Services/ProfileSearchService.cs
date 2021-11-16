using System.Linq;
using System.Collections.Generic;
using CMS.Helpers;
using CMS.Localization;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Extensions;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Api.Services
{
    public class ProfileSearchService
        : BaseSearchService<ProfileSearchRequest, Profile>
    {
        #region "Private fields"

        private readonly IKenticoResourceStringRepository _resourceStringRepository;

        #endregion

        public ProfileSearchService(
            IKenticoResourceStringRepository resourceStringRepository,
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        {
            _resourceStringRepository = resourceStringRepository;
        }

        #region "Helper methods"

        protected override SearchResponse<Profile> GetSearchResults(
            ProfileSearchRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            ParseFilters(searchRequest);

            var searchResponse = base.GetSearchResults(
                searchRequest,
                cacheParameters);

            if (searchResponse == null)
            {
                return null;
            }

            if (!searchRequest.IncludeFilters)
            {
                return searchResponse;
            }

            SetFilters(
                searchResponse,
                searchRequest.Culture,
                cacheParameters);

            return searchResponse;
        }

        protected override string GetSearchResultsCacheKey(
            ProfileSearchRequest searchRequest)
        {
            return string.Format(
                GlobalConstants.Caching.Search.SearchResultsBySearchRequestWithFitlers,
                searchRequest.PageTypes.JoinSorted(
                    GlobalConstants.Caching.Separator,
                    GlobalConstants.Caching.All),
                searchRequest.IndexName,
                searchRequest.PageSize,
                searchRequest.Skip,
                searchRequest.OrderBy,
                searchRequest.OrderByDirection,
                searchRequest.ExcludedNodeGuids?
                    .Select(guid => guid.ToString())
                    .ToArray()
                    .JoinSorted(
                        GlobalConstants.Caching.Separator,
                        string.Empty),
                searchRequest.FilterForCompetitor,
                searchRequest.IncludeFilters);
        }

        protected void ParseFilters(
            ProfileSearchRequest searchRequest)
        {
            // We need to map the incoming resource string ids to the resource string code names
            searchRequest.JurisdictionsCodeNames = MapResourceStringIdsToKeys(searchRequest.Jurisdictions);

            searchRequest.IndustriesCodeNames = MapResourceStringIdsToKeys(searchRequest.Industries);

            searchRequest.PracticeAreasCodeNames = MapResourceStringIdsToKeys(searchRequest.PracticeAreas);

            searchRequest.OfficeLocationsCodeNames = MapResourceStringIdsToKeys(searchRequest.OfficeLocations);
        }

        protected void SetFilters(
            SearchResponse<Profile> response,
            string cultureName,
            CacheParameters cacheParameters = null)
        {
            if (response == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }

            response.Filters = new List<SearchFilter>
            {
                GetLocationFilter(cultureName),
                GetYearOfCallFilter(cultureName),
                GetJurisdictionsFilter(cultureName),
                GetIndustryFilter(cultureName),
                GetOfficeLocationFilter(cultureName),
                GetPracticeAreaFilter(cultureName)                
            };

            // Bust the cache if any of the resource strings associated with the filters are modified
            // Note since we can't bust cache based on resource string prefix, we will just bust on any resource string changes

            cacheParameters?.CacheDependencies
                .Add($"{ResourceStringInfo.OBJECT_TYPE}|all");
        }

        private SearchFilter GetLocationFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(ProfileSearchRequest.Location),
                Title = ResHelper.GetString(
                    Constants.ResourceStrings.Profile.LocationFilter,
                    cultureName),
                Type =
                    FilterType.FreeText,

                PlaceHolderText = ResHelper.GetString(Constants.ResourceStrings.Profile.LocationFilterPlaceholder, cultureName)
            };
        }


        private SearchFilter GetJurisdictionsFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(ProfileSearchRequest.Jurisdictions),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Profile.JurisdictionFilter,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,
                Options =
                    _resourceStringRepository.GetByPrefix(GlobalConstants.ResourceStrings.OnePlaceLocalizations
                            .JurisdictionPrefix)?
                        .Select(resourceString => new SearchFilterOption
                        {
                            CodeName = resourceString.StringID.ToString(),
                            DisplayName = ResHelper.GetString(resourceString.StringKey, cultureName)
                        })
                        .OrderBy(sf=>sf?.DisplayName)
                        .ToList()
            };
        }

        private SearchFilter GetIndustryFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(ProfileSearchRequest.Industries),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Profile.IndustryFilter,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,
                Options =
                    _resourceStringRepository
                        .GetByPrefix(GlobalConstants.ResourceStrings.OnePlaceLocalizations.IndustryPrefix)?
                        .Select(resourceString => new SearchFilterOption
                        {
                            CodeName = resourceString.StringID.ToString(),
                            DisplayName = ResHelper.GetString(resourceString.StringKey, cultureName)
                        })
                        .OrderBy(sf => sf?.DisplayName)
                        .ToList()
            };
        }

        private SearchFilter GetPracticeAreaFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(ProfileSearchRequest.PracticeAreas),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Profile.PracticeAreaFilter,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,
                Options =
                    _resourceStringRepository
                        .GetByPrefix(GlobalConstants.ResourceStrings.OnePlaceLocalizations.PracticeAreaPrefix)
                        ?.Select(resourceString =>
                            new SearchFilterOption
                            {
                                CodeName = resourceString.StringID.ToString(),
                                DisplayName = ResHelper.GetString(resourceString.StringKey, cultureName)
                            })
                        .OrderBy(sf => sf?.DisplayName)
                        .ToList()
            };
        }

        private SearchFilter GetOfficeLocationFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(ProfileSearchRequest.OfficeLocations),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Profile.OfficeLocationFilter,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,
                Options =
                    _resourceStringRepository
                        .GetByPrefix(GlobalConstants.ResourceStrings.OnePlaceLocalizations.OfficeLocationPrefix)
                        ?.Select(resourceString =>
                            new SearchFilterOption
                            {
                                CodeName = resourceString.StringID.ToString(),
                                DisplayName = ResHelper.GetString(resourceString.StringKey, cultureName)
                            })
                        .OrderBy(sf => sf?.DisplayName)
                        .ToList()
            };
        }


        private SearchFilter GetYearOfCallFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(ProfileSearchRequest.YearOfCall),
                Title = ResHelper.GetString(
                    Constants.ResourceStrings.Profile.YearOfCallFilter,
                    cultureName),
                Type =
                    FilterType.FreeText,

                PlaceHolderText =
                    ResHelper.GetString(Constants.ResourceStrings.Profile.YearOfCallFilterPlaceholder, cultureName)
            };
        }

        private List<string> MapResourceStringIdsToKeys(List<int> ids)
        {
            return ids != null
                ? _resourceStringRepository
                    .GetByIds(ids)
                    .Select(r => r.StringKey)
                    .ToList()
                : new List<string>();
        }

        #endregion
    }
}

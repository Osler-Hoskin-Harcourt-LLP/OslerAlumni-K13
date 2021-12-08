using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Extensions;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Api.Services
{
    public class BoardOpportunitySearchService
        : BaseSearchService<BoardOpportunitySearchRequest, BoardOpportunity>
    {
        #region "Private fields"

        private readonly IBoardOpportunityTypeItemRepository _boardOpportunityTypeItemRepository;
        private readonly IJobCategoryItemRepository _jobCategoryItemRepository;

        #endregion

        public BoardOpportunitySearchService(
            IBoardOpportunityTypeItemRepository boardOpportunityTypeItemRepository,
            IJobCategoryItemRepository jobCategoryItemRepository,
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        {
            _boardOpportunityTypeItemRepository = boardOpportunityTypeItemRepository;
            _jobCategoryItemRepository = jobCategoryItemRepository;
        }

        #region "Helper methods"

        protected override string GetSearchResultsCacheKey(
            BoardOpportunitySearchRequest searchRequest)
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

        protected override SearchResponse<BoardOpportunity> GetSearchResults(
            BoardOpportunitySearchRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            var searchResponse = base.GetSearchResults(
                searchRequest,
                cacheParameters);

            if (searchResponse == null)
            {
                return null;
            }

            UpdateBoardCategoryAndTypeDisplay(
                searchResponse.Items,
                searchRequest.Culture,
                cacheParameters);

            if (!searchRequest.IncludeFilters)
            {
                return searchResponse;
            }

            SetFilters(searchResponse, searchRequest.Culture);

            return searchResponse;
        }


        protected void UpdateBoardCategoryAndTypeDisplay(
            List<BoardOpportunity> boardOpportunities,
            string cultureName,
            CacheParameters cacheParameters = null)
        {
            if ((boardOpportunities == null) || (boardOpportunities.Count < 1))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }

            var jobCategories =
                _jobCategoryItemRepository.GetAllJobCategoryItems(cultureName)?
                    .ToDictionary(
                        jc => jc.CodeName,
                        jc => jc.DisplayName)
                ??
                new Dictionary<string, string>();


            var boardOpportunityTypes =
                _boardOpportunityTypeItemRepository.GetAllBoardOpportunityTypeItems(cultureName)?
                    .ToDictionary(
                        jc => jc.CodeName,
                        jc => jc.DisplayName)
                ??
                new Dictionary<string, string>();

            boardOpportunities.ForEach(boardOpportunity =>
            {
                if (!string.IsNullOrWhiteSpace(boardOpportunity.JobCategoryCodeName))
                {
                    string jobCategoryStr;

                    jobCategories
                        .TryGetValue(boardOpportunity.JobCategoryCodeName, out jobCategoryStr);

                    boardOpportunity.JobCategoryDisplayName = jobCategoryStr;
                }

                if (!string.IsNullOrWhiteSpace(boardOpportunity.BoardOpportunityTypeCodeName))
                {
                    string boardOpportunityTypeStr;

                    boardOpportunityTypes
                        .TryGetValue(boardOpportunity.BoardOpportunityTypeCodeName, out boardOpportunityTypeStr);

                    boardOpportunity.BoardOpportunityTypeDisplayName = boardOpportunityTypeStr;
                }
            });

            // Bust the cache if any of the job categories or associated resource strings is modified
            var cacheDependencies = cacheParameters?.CacheDependencies;

            cacheDependencies?.Add(_cacheService.GetCacheKey(
                GlobalConstants.Caching.Jobs.JobCategoryItemsAll,
                cultureName));

            cacheDependencies?.Add(_cacheService.GetCacheKey(
                GlobalConstants.Caching.BoardOpportunities.BoardOpportunityItemsAll,
                cultureName));
        }

        private void SetFilters(
            SearchResponse<BoardOpportunity> response,
            string cultureName)
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
                GetBoardOpportunityLocationFilter(cultureName),
                GetBoardOpportunityTypeFilter(cultureName),
                GetJobCategoryFilter(cultureName),
            };
        }

        private SearchFilter GetBoardOpportunityLocationFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(BoardOpportunitySearchRequest.Location),

                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.BoardOpportunity.SearchFilters.LocationTitle,
                        cultureName),
                Type =
                    FilterType.FreeText,

                PlaceHolderText =
                    ResHelper.GetString(Constants.ResourceStrings.BoardOpportunity.SearchFilters.LocationPlaceHolder,
                        cultureName)
            };
        }

        private SearchFilter GetBoardOpportunityTypeFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(BoardOpportunitySearchRequest.BoardOpportunityTypes),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.BoardOpportunity.SearchFilters.BoardOpportunityTypeTitle,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,

                Options =

                    _boardOpportunityTypeItemRepository
                        .GetAllBoardOpportunityTypeItems(cultureName)?
                        .Select(filter =>
                            new SearchFilterOption
                            {
                                CodeName = filter.CodeName,
                                DisplayName = filter.DisplayName
                            })
                        .OrderBy(sf => sf?.DisplayName)
                        .ToList()
            };
        }

        private SearchFilter GetJobCategoryFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(BoardOpportunitySearchRequest.JobCategories),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Jobs.SearchFilters.JobCategoryTitle,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,

                Options =

                    _jobCategoryItemRepository
                        .GetAllJobCategoryItems(cultureName)?
                        .Select(filter =>
                            new SearchFilterOption
                            {
                                CodeName = filter.CodeName,
                                DisplayName = filter.DisplayName
                            })
                        .OrderBy(sf => sf?.DisplayName)
                        .ToList()
            };

        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Extensions;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Api.Helpers;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Api.Services
{
    public class JobSearchService
        : BaseSearchService<JobSearchRequest, Job>
    {
        #region "Private fields"

        private readonly IJobCategoryItemRepository _jobCategoryItemRepository;

        #endregion

        public JobSearchService(
            IJobCategoryItemRepository jobCategoryItemRepository,
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        {
            _jobCategoryItemRepository = jobCategoryItemRepository;
        }

        #region "Helper methods"

        protected override string GetSearchResultsCacheKey(
            JobSearchRequest searchRequest)
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

        protected override SearchResponse<Job> GetSearchResults(
            JobSearchRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            var searchResponse = base.GetSearchResults(
                searchRequest,
                cacheParameters);

            if (searchResponse == null)
            {
                return null;
            }

            UpdateJobCategoryDisplay(
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


        protected void UpdateJobCategoryDisplay(
            List<Job> jobList,
            string cultureName,
            CacheParameters cacheParameters = null)
        {
            if ((jobList == null) || (jobList.Count < 1))
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

            jobList.ForEach(e =>
            {
                string jobCategoryStr;

                jobCategories
                    .TryGetValue(e.JobCategoryCodeName, out jobCategoryStr);

                e.JobCategoryDisplayName = jobCategoryStr;
            });

            // Bust the cache if any of the job categories or associated resource strings is modified
            cacheParameters?.CacheDependencies
                .Add(_cacheService.GetCacheKey(
                    GlobalConstants.Caching.Jobs.JobCategoryItemsAll,
                    cultureName));
        }

        private void SetFilters(
            SearchResponse<Job> response,
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
                GetJobLocationFilter(cultureName),
                GetJobClassificationFilter(cultureName),
                GetJobCategoryFilter(cultureName)
            };
        }

        private SearchFilter GetJobCategoryFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(JobSearchRequest.JobCategories),
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

        private SearchFilter GetJobLocationFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(Job.JobLocation),

                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Jobs.SearchFilters.JobLocationTitle,
                        cultureName),
                Type =
                    FilterType.FreeText,

                PlaceHolderText =
                    ResHelper.GetString(Constants.ResourceStrings.Jobs.JobLocationPlaceholder, cultureName)
            };
        }


        private SearchFilter GetJobClassificationFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(JobSearchRequest.JobClassifications),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Jobs.SearchFilters.JobClassificationTitle,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,

                Options =

                    EnumHelpers.ToSearchFilters<JobClassification>(cultureName)
            };
        }

        #endregion
    }
}

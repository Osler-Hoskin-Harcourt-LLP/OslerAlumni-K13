using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CMS.Helpers;
using CMS.Search;
using ECA.Core.Extensions;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.Mvc.Navigation.Extensions;
using ECA.PageURL.Services;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Api.Helpers;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Definitions;

using SearchParameters = Microsoft.Azure.Search.Models.SearchParameters;

namespace OslerAlumni.Mvc.Api.Services
{
    public class AzureSearchService
        : ServiceBase, ISearchService
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly IKenticoSearchIndexRepository _searchIndexRepository;
        private readonly IPageUrlService _pageUrlService;

        #endregion

        #region "Constants"

        private const int MaxAzureSkipCount = 100000;

        #endregion

        public AzureSearchService(
            IEventLogRepository eventLogRepository,
            IKenticoSearchIndexRepository searchIndexRepository,
            IPageUrlService pageUrlService)
        {
            _eventLogRepository = eventLogRepository;
            _searchIndexRepository = searchIndexRepository;
            _pageUrlService = pageUrlService;
        }

        #region "Methods"

        public SearchResponse<T> Search<T>(
            SearchRequest<T> searchRequest,
            out SearchIndexInfo searchIndex)
            where T : class, ISearchable, new()
        {
            searchIndex = null;

            if (searchRequest == null)
            {
                return null;
            }

            try
            {
                var searchIndexClient =
                    GetSearchIndexClient(
                        searchRequest.IndexName,
                        out searchIndex);

                if (searchIndexClient == null)
                {
                    return null;
                }

                var azureSearchFieldProperties = typeof(T)
                    .GetPropertiesWithAttribute<AzureSearchFieldAttribute>();

                var searchText = Helpers.AzureHelper.SanitizeKeyword(searchRequest.Keywords);

                var searchParams = GetSearchParameters(
                    searchRequest,
                    azureSearchFieldProperties);

                if (searchParams != null)
                {
                    searchParams.IncludeTotalResultCount = true;
                }

                var searchResult = searchIndexClient.Documents
                    .Search(
                        searchText,
                        searchParams);

                var items = searchResult?.Results?
                                .Select(item =>
                                    GetTypedDocument<T>(item.Document, azureSearchFieldProperties))
                                .ToList()
                            ?? new List<T>();

                foreach (var item in items)
                {
                    string url;

                    if (!_pageUrlService.TryGetPageMainUrl(
                            item.NodeGuid,
                            item.Culture,
                            out url))
                    {
                        continue;
                    }

                    item.PageUrl = url;
                }

                var response = new SearchResponse<T>
                {
                    Items = items,
                    PageSize = searchRequest.PageSize,
                    PageNumber = searchRequest.PageNumber,
                    //Azure still returns total count even if we skipped past the total number of results.
                    //This is why we need to check the item count as well.
                    TotalItemCount = items.Count() == 0 ? 0 : searchResult?.Count,
                    IsKeywordOrFilteredSearch = searchRequest.IsKeywordOrFilteredSearch()
                };

                response.TotalResultsCountDisplay = string.Format(
                    ResHelper.GetString(
                        Constants.ResourceStrings.ListTotalResultsCountDisplay,
                        searchRequest.Culture),
                    response.StartIndex,
                    response.EndIndex,
                    response.TotalItemCount);

                return response;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(Search),
                    ex);

                return null;
            }
        }

        #endregion

        #region "Helper methods"

        
        
        protected ISearchIndexClient GetSearchIndexClient(
            string indexName,
            out SearchIndexInfo searchIndex)
        {
            searchIndex = null;

            if (string.IsNullOrWhiteSpace(indexName))
            {
                return null;
            }

            try
            {
                searchIndex =
                    _searchIndexRepository.GetByName(indexName);

                if (searchIndex == null)
                {
                    return null;
                }

                var client = new SearchServiceClient(
                    searchIndex.IndexSearchServiceName,
                    new SearchCredentials(searchIndex.IndexQueryKey));

                return client.Indexes?.GetClient(indexName);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetSearchIndexClient),
                    ex);

                return null;
            }
        }

        protected SearchParameters GetSearchParameters<T>(
            SearchRequest<T> searchRequest,
            Dictionary<PropertyInfo, AzureSearchFieldAttribute> azureSearchFieldProperties = null)
            where T : class, ISearchable, new()
        {

            //Note: The skip parameter is calculated as a double since there may be too large ints that
            //cause Int32 overflow and return as negative numbers. This avoids that.

            double skip = (double) searchRequest.Skip +
                       (double) (searchRequest.PageNumber - 1) * (double) searchRequest.PageSize;

            var searchParams = new SearchParameters
            {
                Top = searchRequest.PageSize,
                SearchMode = SearchMode.All,
                QueryType = QueryType.Full,
                ScoringProfile = GlobalConstants.AzureSearch.ScoringProfile.BoostTitle,
                //Skip parameter cannot be more than 100,000
                Skip =  skip > MaxAzureSkipCount? MaxAzureSkipCount : Convert.ToInt32(skip)
            };

            var filterExpression = searchRequest.GetFilterExpression();

            searchParams.Filter =
                filterExpression?.ExpressionText;

            if (azureSearchFieldProperties == null)
            {
                azureSearchFieldProperties = typeof(T)
                    .GetPropertiesWithAttribute<AzureSearchFieldAttribute>();
            }

            searchParams.Select = azureSearchFieldProperties.Values
                .Select(azureSearchField => azureSearchField.FieldName)
                .ToList();

            searchParams.OrderBy = searchRequest.GetOrderByExpressions();

            return searchParams;
        }
        
        protected T GetTypedDocument<T>(
            Document searchDocument,
            Dictionary<PropertyInfo, AzureSearchFieldAttribute> azureSearchFieldProperties = null)
            where T : class, ISearchable, new()
        {
            var result = new T();

            try
            {
                if (azureSearchFieldProperties == null)
                {
                    azureSearchFieldProperties = typeof(T)
                        .GetPropertiesWithAttribute<AzureSearchFieldAttribute>();
                }

                foreach (var fieldProperty in azureSearchFieldProperties)
                {
                    var property = fieldProperty.Key;
                    var azureSearchField = fieldProperty.Value;

                    object docValue;

                    if (!searchDocument.TryGetValue(
                            azureSearchField.FieldName,
                            out docValue))
                    {
                        continue;
                    }

                    if (docValue != null)
                    {
                        var propValue = docValue
                            .ChangeType(
                                property.PropertyType);

                        property.SetValue(result, propValue, null);
                    }
                    
                }

                return result;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetTypedDocument),
                    ex);

                return default(T);
            }
        }

        

        #endregion
    }
}

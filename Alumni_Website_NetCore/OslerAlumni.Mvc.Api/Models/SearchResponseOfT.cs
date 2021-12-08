using System;
using System.Collections.Generic;
using CMS.Helpers;
using Newtonsoft.Json;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Wrapper class for the search results response from search API.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class SearchResponse<TResult>
    {
        /// <summary>
        /// Strongly-typed list of search results.
        /// </summary>
        [JsonProperty("items")]
        public virtual List<TResult> Items { get; set; }

        /// <summary>
        /// Number of search results per page (as specified in the search request).
        /// </summary>
        [JsonProperty("pageSize")]
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Page number, for which the search results are being returned
        /// (as specified in the search request).
        /// </summary>
        [JsonProperty("pageNumber")]
        public virtual int PageNumber { get; set; }

        /// <summary>
        /// Total number of results matching the search criteria.
        /// </summary>
        [JsonProperty("totalItemCount")]
        public virtual long? TotalItemCount { get; set; }

        /// <summary>
        /// Total number of pages, based on the total number of results matching the search criteria
        /// and the requested number of search results per page.
        /// </summary>
        [JsonProperty("totalPageCount")]
        public virtual long TotalPageCount
            => ((PageSize > 0) && TotalItemCount.HasValue)
                ? (int) Math.Ceiling(TotalItemCount.Value / (double) PageSize)
                : 0;

        /// <summary>
        /// Total results count that can be displayed to the user.
        /// </summary>
        [JsonProperty("totalResultsCountDisplay")]
        public string TotalResultsCountDisplay { get; set; }

        [JsonIgnore]
        public int StartIndex => (TotalItemCount != null && PageNumber > 0 && PageSize > 0)
            ? Math.Min((PageNumber - 1) * PageSize + 1, (int)TotalItemCount) //Start Index should never be more than total results (in case of 0 results)
            : 0;

        [JsonIgnore]
        public int EndIndex => (TotalItemCount != null && PageNumber > 0 && PageSize > 0)? Math.Min(StartIndex + PageSize - 1, (int)TotalItemCount) : 0;

        /// <summary>
        /// The filters that should be displayed in the page to allow the user to filter results
        /// </summary>
        [JsonProperty("filters")]
        public virtual IList<SearchFilter> Filters { get; set; }

        /// <summary>
        /// This field indicates whether filters were provided to the search.
        /// Useful to distinguish between no items or no items due to
        /// search parameters specified.
        /// </summary>
        public bool IsKeywordOrFilteredSearch { get; set; }

    }
}

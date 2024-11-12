using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Core.Extensions;
using Newtonsoft.Json;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Api.Attributes.Validation;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Base class for requests to search API.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class SearchRequest<TResult>
        where TResult : class, ISearchable, new()
    {
        #region "Constants"

        private const string AzureScoreField = "search.score()";
        private readonly string OrderByScore =
            $"{AzureScoreField} {Definitions.OrderByDirection.Descending.ToStringRepresentation()}";

        #endregion

        #region "Private fields"

        protected string[] _pageTypes;

        #endregion

        #region "Properties"

        /// <summary>
        /// Name of the search index, e.g. "alumni-dev-global".
        /// </summary>
        [JsonIgnore]
        public virtual string? IndexName { get; set; }

        /// <summary>
        /// List of Kentico Page Types that should be included in the search,
        /// e.g. [ "osleralumni.pagetype_page", "osleralumni.pagetype_news" ].
        /// </summary>
        [Required]
        [MinLength(1)]
        [OslerAlumni.Mvc.Api.Attributes.Validation.AllowedValues(
            new[]
            {
                PageType_Page.CLASS_NAME,
                PageType_Event.CLASS_NAME,
                PageType_LandingPage.CLASS_NAME,
                PageType_News.CLASS_NAME,
                PageType_Job.CLASS_NAME,
                PageType_Resource.CLASS_NAME,
                PageType_Profile.CLASS_NAME,
                PageType_DevelopmentResource.CLASS_NAME,
                PageType_BoardOpportunity.CLASS_NAME
            },
            false,
            true,
            ErrorMessage = "One or more values in the provided list is an incorrect page type")]
        [JsonIgnore]
        public virtual string[] PageTypes
        {
            get
            {
                if ((_pageTypes == null) || (_pageTypes.Length < 1))
                {
                    var pageTypeAttr = typeof(TResult)
                        .GetCustomAttribute<SearchPageTypeAttribute>(true);

                    if (pageTypeAttr != null)
                    {
                        _pageTypes = new[] { pageTypeAttr.PageTypeName };
                    }
                }

                return _pageTypes;
            }
        }

        /// <summary>
        /// Standard culture code, in which to search for the pages, e.g. "fr-CA".
        /// NOTE: Search is case-sensitive and the culture code is
        /// expected to follow correct casing.
        /// </summary>
        [Required]
        [AllowedCulture(ErrorMessage = "Incorrect culture")]
        [JsonProperty("culture")]
        public virtual string Culture { get; set; }

        /// <summary>
        /// A flag indicating if the search is being conducted in Kentico's "Preview" mode,
        /// i.e. if unpublished pages should be included in the search when possible.
        /// </summary>
        [JsonProperty("isPreviewMode")]
        public virtual bool IsPreviewMode { get; set; }

        /// <summary>
        /// Depending on whether we are in preview mode or not,
        /// this will be used to either filter out the content that's not published yet
        /// or to include it for preview.
        /// </summary>
        [JsonIgnore]
        public virtual DateTime DatePublishedFrom
            => IsPreviewMode ? DateTime.MaxValue : DateTime.Now;

        /// <summary>
        /// Regardless of the preview mode, this will be used to filter out the content
        /// that is no longer published.
        /// </summary>
        [JsonIgnore]
        public virtual DateTime DatePublishedTo
            => DateTime.Now;

        /// <summary>
        /// Number of search results per page.
        /// </summary>
        [Required]
        [MinValue(1, ErrorMessage = "Page size cannot be less than 1")]
        [JsonProperty("pageSize")]
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Page number, for which to return the search results, if available.
        /// </summary>
        [Required]
        [MinValue(1, ErrorMessage = "Page number cannot be less than 1")]
        [JsonProperty("pageNumber")]
        public virtual int PageNumber { get; set; }

        /// <summary>
        /// Number of search results to skip from the top of the list, an offset.
        /// </summary>
        [MinValue(0, ErrorMessage = "Number of items to skip cannot be less than 0")]
        [JsonProperty("skip")]
        public virtual int Skip { get; set; }

        /// <summary>
        /// Name of a Kentico Page Type field, by which the search results should be ordered.
        /// </summary>
        [JsonProperty("orderBy")]
        public virtual string? OrderBy { get; set; }

        /// <summary>
        /// Direction, in which the search results should be ordered.
        /// </summary>
        [JsonProperty("orderByDirection")]
        public virtual OrderByDirection? OrderByDirection { get; set; }

        /// <summary>
        /// Keywords for full-text search.
        /// </summary>
        [JsonProperty("keywords")]
        public virtual string Keywords { get; set; }

        /// <summary>
        /// List of Kentico Node GUIDs to be excluded from the search results.
        /// For instance, if the listing page has featured items, those could be
        /// exluded from the main list of search results to avoid content
        /// duplication on the page.
        /// </summary>
        [JsonProperty("excludedNodeGuids")]
        public virtual Guid[] ExcludedNodeGuids { get; set; }

        /// <summary>
        /// Filters out resources that are protected from competitors.
        /// </summary>
        [JsonIgnore]
        public bool FilterForCompetitor { get; set; } = false;


        #endregion

        #region "Methods"

        /// <summary>
        /// Gets the AzureSearchFilterExpression that will be sent to Azure Search
        /// Override this method to modify the default filter logic.
        /// </summary>
        /// <returns>AzureSearchFilterExpression</returns>
        public virtual AzureSearchFilterExpression GetFilterExpression()
        {
            var filterExpression = new AzureSearchFilterExpression()
                .And()
                .In(nameof(TreeNode.ClassName), PageTypes)
                .Equals(nameof(TreeNode.DocumentCulture), Culture)
                .Compare(nameof(TreeNode.DocumentPublishFrom), DatePublishedFrom,
                    FilterOperation.LessOrEqualThanOrEmpty)
                .Compare(nameof(TreeNode.DocumentPublishTo), DatePublishedTo, FilterOperation.GreaterOrEqualThanOrEmpty)
                .NotIn(nameof(TreeNode.NodeGUID), ExcludedNodeGuids);

            return filterExpression;
        }

        /// <summary>
        /// Gets the AzureOrderByExpressions that will be sent to Azure Search
        /// Override this method to modify the default order.
        /// </summary>
        /// <returns>AzureSearchFilterExpression</returns>
        public virtual List<string> GetOrderByExpressions()
        {
            var orderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                orderBy = typeof(TResult)
                    .GetProperty(OrderBy.ToPascalCase())?
                    .GetCustomAttribute<AzureSearchFieldAttribute>()?
                    .FieldName;

                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = OrderByDirection.HasValue
                        ? $"{orderBy} {OrderByDirection.Value.ToStringRepresentation()}"
                        : $"{orderBy} {Definitions.OrderByDirection.Ascending.ToStringRepresentation()}";
                }
            }

            var orderByClauses = new List<string>();

            // If keyword search is used, order by scoring
            if (!string.IsNullOrWhiteSpace(Keywords))
            {
                orderByClauses.Add(OrderByScore);
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderByClauses.Add(orderBy);
            }

            return orderByClauses;
        }

        /// <summary>
        /// Determines if the search request invokes keyword-based search
        /// and/or filters search results.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsKeywordOrFilteredSearch()
        {
            return !string.IsNullOrWhiteSpace(Keywords);
        }

        #endregion
    }
}

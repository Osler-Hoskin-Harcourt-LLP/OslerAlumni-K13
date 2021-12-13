using System.Collections.Generic;
using CMS.Helpers;
using Newtonsoft.Json;
using OslerAlumni.Mvc.Api.Attributes.Validation;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for resource search requests to the search API.
    /// </summary>
    public class DevelopmentResourceSearchRequest
        : SearchRequest<DevelopmentResource>
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets a value indicating whether the filters should be returned in the response or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [get filters]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("includeFilters")]
        public bool IncludeFilters { get; set; }

        /// <summary>
        /// If provided, the resources will be filtered by provided types.
        /// </summary>
        [AllowedDevelopmentResourceTypes(ErrorMessage = "Invalid Development Resource Types provided to filter by.")]
        [JsonProperty("developmentResourceTypes")]
        public List<string> DevelopmentResourceTypes { get; set; } = new List<string>();

        #endregion

        #region "Methods"

        public override AzureSearchFilterExpression GetFilterExpression()
        {
            var filterExpression = base.GetFilterExpression();

            if (DataHelper.DataSourceIsEmpty(DevelopmentResourceTypes))
            {
                return filterExpression;
            }

            // Filter by resource types if provided.
            var additionalFilter = new AzureSearchFilterExpression();

            foreach (var resourceType in DevelopmentResourceTypes)
            {
                var itemfilter = new AzureSearchFilterExpression()
                    .Matches(nameof(PageType_DevelopmentResource.DevelopmentResourceTypes), resourceType);

                additionalFilter.Or(itemfilter);
            }

            return filterExpression
                .And(additionalFilter);
        }

        public override bool IsKeywordOrFilteredSearch()
        {
            return base.IsKeywordOrFilteredSearch()
                || !DataHelper.DataSourceIsEmpty(DevelopmentResourceTypes);
        }

        #endregion
    }
}

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
    public class ResourceSearchRequest
        : SearchRequest<Resource>
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
        [AllowedResourceTypes(ErrorMessage = "Invalid Resource Types provided to filter by.")]
        [JsonProperty("resourceTypes")]
        public List<string> ResourceTypes { get; set; }

        #endregion

        #region "Methods"

        public override AzureSearchFilterExpression GetFilterExpression()
        {
            var filterExpression = base.GetFilterExpression();

            if (FilterForCompetitor)
            {
                filterExpression.Equals(nameof(PageType_Resource.HideFromCompetitors), false);
            }

            if (DataHelper.DataSourceIsEmpty(ResourceTypes))
            {
                return filterExpression;
            }

            // Filter by resource types if provided.
            var additionalFilter = new AzureSearchFilterExpression();


            foreach (var resourceType in ResourceTypes)
            {
                var itemfilter = new AzureSearchFilterExpression()
                    .Matches(nameof(PageType_Resource.Types), resourceType);

                additionalFilter.Or(itemfilter);
            }

            return filterExpression
                .And(additionalFilter);
        }

        public override bool IsKeywordOrFilteredSearch()
        {
            return base.IsKeywordOrFilteredSearch()
                || !DataHelper.DataSourceIsEmpty(ResourceTypes);
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OslerAlumni.Mvc.Api.Attributes.Validation;
using OslerAlumni.Mvc.Api.Helpers;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for event search requests to the search API.
    /// </summary>
    public class BoardOpportunitySearchRequest
        : SearchRequest<BoardOpportunity>
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
        /// Location to filter by. If not provided will return all locations.
        /// NOTE: Search is case-insensitive but must be exact match
        /// </summary>
        //[AllowedJobClassification(ErrorMessage = "Incorrect Location")]
        [JsonProperty("location")]
        public string Location { get; set; } = "";


        /// <summary>
        /// Job Categories to filter by. If not provided will return all categories.
        /// NOTE: Search is case-sensitive and the jobCategories is
        /// expected to follow correct casing.
        /// </summary>
        [AllowedJobCategories(ErrorMessage = "Incorrect Job Categories")]
        [JsonProperty("jobCategories")]
        public List<string> JobCategories { get; set; } = new List<string>();


        /// <summary>
        /// Board Oppurtunity Types to filter by. If not provided will return all.
        /// </summary>
        [AllowedBoardOpportunityTypes(ErrorMessage = "Incorrect Board Oppurtunity Type")]
        [JsonProperty("boardOpportunityTypes")]
        public List<string> BoardOpportunityTypes { get; set; } = new List<string>();

        #endregion

        #region "Methods"

        public override AzureSearchFilterExpression GetFilterExpression()
        {
            AzureSearchFilterExpression filter = base.GetFilterExpression();

            if (!string.IsNullOrWhiteSpace(Location))
            {
                //Filter by location if provided.
                var additionalFilter = new AzureSearchFilterExpression()
                    .Matches(nameof(PageType_BoardOpportunity.BoardOpportunityLocation), AzureHelper.SanitizeKeyword(Location));

                filter.And(additionalFilter);
            }

            if (BoardOpportunityTypes?.Any() ?? false)
            {
                //Filter by BoardOpportunityTypes if provided.
                var additionalFilter = new AzureSearchFilterExpression()
                    .In(nameof(PageType_BoardOpportunity.BoardOpportunityTypeCodeName), BoardOpportunityTypes.ToArray());

                filter.And(additionalFilter);
            }

            if (JobCategories?.Any() ?? false)
            {
                //Filter by category if provided.
                var additionalFilter = new AzureSearchFilterExpression()
                    .In(nameof(PageType_BoardOpportunity.JobCategoryCodeName), JobCategories.ToArray());

                filter.And(additionalFilter);
            }

            return filter;
        }

        public override bool IsKeywordOrFilteredSearch()
        {
            return base.IsKeywordOrFilteredSearch()
                   || !string.IsNullOrWhiteSpace(Location)
                   || (BoardOpportunityTypes != null && BoardOpportunityTypes.Count > 0)
                   || (JobCategories != null && JobCategories.Count > 0);
        }

        #endregion
    }
}

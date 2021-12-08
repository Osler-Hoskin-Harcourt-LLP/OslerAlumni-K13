using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Api.Attributes.Validation;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Core.Extensions;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for global site search requests to the search API.
    /// </summary>
    public class GlobalSearchRequest
        : SearchRequest<GlobalResult>
    {

        public GlobalSearchRequest()
        {
            if ((_pageTypes == null) || (_pageTypes.Length < 1))
            {
                var allowedValuesAttr = GetType()
                    .GetProperty(nameof(PageTypes))?
                    .GetCustomAttribute<AllowedValuesAttribute>(true);

                if (allowedValuesAttr != null)
                {
                    _pageTypes = allowedValuesAttr.GetAllowableValues()?
                        .Select(v => new SearchPageTypeAttribute(v).PageTypeName)
                        .ToArray();
                }
            }
        }

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
        /// If provided, will filter the result to only certain page types.
        /// </summary>
        [JsonProperty("pageTypesFilter")]
        public PageTypeFilter[] PageTypesFilter
        {
            set
            {
                if ((value != null) && (value.Length > 0))
                {
                    // Directly set the inner field value, since PageTypes field is read-only
                    _pageTypes = value.Select(pageTypeFilter =>
                        pageTypeFilter.GetAttribute<SearchPageTypeAttribute>().PageTypeName).ToArray();
                }
            }
        }

        public override AzureSearchFilterExpression GetFilterExpression()
        {
            var filter = base.GetFilterExpression();

            if (FilterForCompetitor)
            {
                filter.Compare(
                    nameof(PageType_Event.HideFromCompetitors),
                    false,
                    FilterOperation.EqualOrEmpty);
            }

            return filter;
        }

        #endregion
    }
}

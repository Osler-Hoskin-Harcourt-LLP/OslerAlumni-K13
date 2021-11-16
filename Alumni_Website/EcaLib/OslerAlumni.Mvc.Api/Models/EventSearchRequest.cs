using System;
using System.Collections.Generic;
using System.Reflection;
using CMS.Helpers;
using Newtonsoft.Json;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for event search requests to the search API.
    /// </summary>
    public class EventSearchRequest
        : SearchRequest<Event>
    {
        
        [JsonIgnore]
        public override string OrderBy { get; set; }

        [JsonIgnore]
        public override OrderByDirection? OrderByDirection { get; set; }

        #region "Methods"

        public override AzureSearchFilterExpression GetFilterExpression()
        {
            var filter = base.GetFilterExpression();

            //Show only future events or past webinar-on-demand events.
            var additionalFilter = new AzureSearchFilterExpression()
                .GreaterOrEqualThan(nameof(PageType_Event.EndDate), DateTime.Now)
                .Or()
                .Equals(nameof(PageType_Event.DeliveryMethods), DeliveryMethods.WebinarOnDemand);

            if (FilterForCompetitor)
            {
                filter
                    .And(new AzureSearchFilterExpression()
                        .Equals(nameof(PageType_Event.HideFromCompetitors), false)
                    );
            }

            return filter
                .And(additionalFilter);
        }

        public override List<string> GetOrderByExpressions()
        {
            //Events need to be sorted specially so ignore the sort parameters passed in.

            var orderByClauses = new List<string>();
            
            var orderBy = typeof(Event)
                .GetProperty(nameof(Event.SortOrder))?
                .GetCustomAttribute<AzureSearchFieldAttribute>()?
                .FieldName;

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var orderByClause = $"{orderBy} {Definitions.OrderByDirection.Ascending.ToStringRepresentation()}";

                orderByClauses.Add(orderByClause);
            }

            orderBy = typeof(Event)
                .GetProperty(nameof(Event.SortDummyDateTimeTicks))?
                .GetCustomAttribute<AzureSearchFieldAttribute>()?
                .FieldName;

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var orderByClause = $"{orderBy} {Definitions.OrderByDirection.Ascending.ToStringRepresentation()}";

                orderByClauses.Add(orderByClause);
            }

            return orderByClauses;
        }

        #endregion
    }
}

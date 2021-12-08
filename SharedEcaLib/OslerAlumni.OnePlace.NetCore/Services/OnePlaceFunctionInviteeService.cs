using System.Collections.Generic;
using System.Linq;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Nito.AsyncEx;
using OslerAlumni.Core.Definitions;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceFunctionInviteeService
        : ServiceBase, IOnePlaceFunctionInviteeService
    {
        #region "Constants"

        private const string AcceptedRsvpStatus = "accepted";
      
        #endregion

        #region "Private fields"

        private readonly IOnePlaceDataService _onePlaceDataService;
        private readonly ICacheService _cacheService;
        private readonly IEventLogRepository _eventLogRepository;

        #endregion

        public OnePlaceFunctionInviteeService(
            IOnePlaceDataService onePlaceDataService, ICacheService cacheService, IEventLogRepository eventLogRepository)
        {
            _onePlaceDataService = onePlaceDataService;
            _cacheService = cacheService;
            _eventLogRepository = eventLogRepository;
        }

        #region "Methods"

        public List<OnePlaceFunctionInvitee> GetAttendeesForOneplaceFunction(string oneplaceFunctionId)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(GlobalConstants.Caching.Search.SearchResultsByOnePlaceTypeAndId,
                    nameof(OnePlaceFunctionInvitee), oneplaceFunctionId),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                AllowNullValue = false
            };

            var response = _cacheService.Get(
                cp =>
                {
                    IList<OnePlaceFunctionInvitee> responseList;
                    string message;

                    if (!TryGetOnePlaceFunctionAttendees(oneplaceFunctionId, out responseList, out message))
                    {
                        _eventLogRepository.LogError(GetType(), nameof(GetAttendeesForOneplaceFunction), message);

                        return null;
                    }

                    return responseList;

                }, cacheParameters);

            return response?.ToList() ?? new List<OnePlaceFunctionInvitee>(); 
        }

        public bool TryGetOnePlaceFunctionAttendees(string oneplaceFunctionId,
            out IList<OnePlaceFunctionInvitee> attendees,
            out string errorMessage)
        {
            errorMessage = null;

            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetInvitees,
                PageType_OnePlaceQueries.CLASS_NAME);

            var oneplaceFunctionIdPropertyName =
                typeof(OnePlaceFunctionInvitee).GetPropertyName(
                    nameof(OnePlaceFunctionInvitee.OnePlaceFunctionId),
                    NameSource.Json);


            var rsvpStatusPropertyName =
                typeof(OnePlaceFunctionInvitee).GetPropertyName(
                    nameof(OnePlaceFunctionInvitee.RsvpStatus),
                    NameSource.Json);


            var contactPropertyName = typeof(OnePlaceFunctionInvitee).GetPropertyName(
                nameof(OnePlaceFunctionInvitee.Contact),
                NameSource.Json);

            var contactLastNamePropertyName = typeof(Contact).GetPropertyName(
                nameof(Contact.LastName),
                NameSource.Json);

            var lastNamePropertyName = $"{contactPropertyName}.{contactLastNamePropertyName}";
            

            var where = new OnePlaceWhereCondition()
                .WhereEquals(
                    oneplaceFunctionIdPropertyName,
                    oneplaceFunctionId)
                .WhereEquals(
                    rsvpStatusPropertyName,
                    AcceptedRsvpStatus);

            IList<OnePlaceFunctionInvitee> attendeeList = null;
            string message = null;

            var isSuccess = AsyncContext.Run(() =>
                _onePlaceDataService.TryGetList(
                    query,
                    out attendeeList,
                    out message,
                    where: where,
                    orderBy: lastNamePropertyName));

            attendees = attendeeList;
            errorMessage = message;

            return isSuccess;
        }


        #endregion
    }
}

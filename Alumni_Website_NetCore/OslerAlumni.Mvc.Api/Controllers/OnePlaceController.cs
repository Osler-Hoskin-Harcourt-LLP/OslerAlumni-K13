using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Services;
using System.Globalization;


namespace OslerAlumni.Mvc.Api.Controllers
{
    /// <summary>
    /// OnePlace API to be used by front-end functionality.
    /// It expects to run in the context of a session
    /// of an authenticated user on the main site.
    /// </summary>
    [Authorize(Policy = "PublicPage")]
    public class OnePlaceController
        : BaseApiController
    {
        private readonly IOnePlaceFunctionService _onePlaceFunctionService;
        private readonly ICacheService _cacheService;
        private readonly IEventLogRepository _eventLogRepository;
        private readonly IOnePlaceFunctionInviteeService _onePlaceFunctionInviteeService;
        private readonly IProfileService _profileService;

        public OnePlaceController(
            IOnePlaceFunctionService onePlaceFunctionService,
            ICacheService cacheService,
            IEventLogRepository eventLogRepository,
            IOnePlaceFunctionInviteeService onePlaceFunctionInviteeService,
            IProfileService profileService)
        {
            _onePlaceFunctionService = onePlaceFunctionService;
            _cacheService = cacheService;
            _eventLogRepository = eventLogRepository;
            _onePlaceFunctionInviteeService = onePlaceFunctionInviteeService;
            _profileService = profileService;
        }

        /// <summary>
        /// Endpoint for OnePlace Functions.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This returns all active functions in OnePlace.
        /// </remarks>
        [HttpPost]
        public ActionResult Functions()
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(GlobalConstants.Caching.Search.SearchResultsByOnePlaceType,
                    nameof(OnePlaceFunction)),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                AllowNullValue = false
            };

            var response = _cacheService.Get(
                cp =>
                {
                    IList<OnePlaceFunction> responseList;
                    string message;

                    if (!_onePlaceFunctionService.TryGetOnePlaceFunctions(out responseList, out message))
                    {
                        _eventLogRepository.LogError(GetType(), nameof(Functions), message);

                        return null;
                    }

                    return responseList.ToList();
                },
                cacheParameters);

            if (response == null)
            {
                return StatusCode(500);
            }

            return ToSearchResponse(GlobalConstants.Cultures.English, response);
        }


        //TODO: [MS] Remove this mock value.
        /// <summary>
        /// Mock Endpoint for OnePlace Function Attendees.
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        /// <remarks>
        /// This search returns all attendees for a oneplace function who have RSVP = yes.
        /// </remarks>
        [HttpPost]
        public ActionResult FunctionAttendeesMock(
            [FromBody] FunctionAttendeeSearchRequest searchRequest)
        {
            if (searchRequest == null)
            {
                return BadRequest($"Missing '{nameof(searchRequest)}' parameter.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = new List<Attendee>();

            for (int i = 0; i < 250; i++)
            {
                items.Add(new Attendee()
                {
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    CompanyName = $"Company{i}",
                    ProfileUrl = "www.google.com"
                });
            }

            return ToSearchResponse(searchRequest.Culture, items);
        }

        /// <summary>
        /// Endpoint for OnePlace Function Attendees.
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        /// <remarks>
        /// This search returns all attendees for a oneplace function who have RSVP = yes.
        /// </remarks>
        [HttpPost]
        public ActionResult FunctionAttendees(
            [FromBody] FunctionAttendeeSearchRequest searchRequest)
        {
            if (searchRequest == null)
            {
                return BadRequest($"Missing '{nameof(searchRequest)}' parameter.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(GlobalConstants.Caching.Search.SearchResultsByOnePlaceTypeAndId,
                    nameof(OnePlaceFunctionInvitee), searchRequest.OnePlaceFunctionId),
                CultureCode = searchRequest.Culture,
                AllowNullValue = false
            };

            var response = _cacheService.Get(
                cp =>
                {
                    var attendees =
                        _onePlaceFunctionInviteeService
                            .GetAttendeesForOneplaceFunction(searchRequest.OnePlaceFunctionId);

                    return ToAttendees(attendees, searchRequest.Culture);
                },
                cacheParameters);


            return ToSearchResponse(searchRequest.Culture, response);
        }

        private List<Attendee> ToAttendees(List<OnePlaceFunctionInvitee> onePlaceFunctionsAttendeeList, string culture)
        {

            var profileUrls = _profileService.GetProfileUrls(culture);


            var result = new List<Attendee>();

            foreach (var onePlaceFunctionInvitee in onePlaceFunctionsAttendeeList)
            {
                var attendee = new Attendee()
                {
                    FirstName = onePlaceFunctionInvitee.Contact?.FirstName,
                    LastName = onePlaceFunctionInvitee.Contact?.LastName,
                    CompanyName = onePlaceFunctionInvitee.Contact?.Account?.Name
                };

                if (string.Equals(attendee.CompanyName,
                    DataSubmissionConstants.SpecialCompanies.NoCompany,
                    StringComparison.OrdinalIgnoreCase))
                {
                    attendee.CompanyName = String.Empty;
                }

                string profileUrl;

                if (profileUrls.TryGetValue(onePlaceFunctionInvitee.ContactId, out profileUrl))
                {
                    attendee.ProfileUrl = profileUrl;
                }

                result.Add(attendee);
            }

            return result;
        }


        private ActionResult ToSearchResponse<T>(string culture, List<T> returnList)
        {
            var defaultSettings = JsonConvert.DefaultSettings();

            defaultSettings.Culture =
                new CultureInfo(culture);

            var result = new SearchResponse<T>();

            result.Items = returnList;

            result.TotalItemCount = result.Items.Count;

            return Json(result, defaultSettings);
        }
    }
}

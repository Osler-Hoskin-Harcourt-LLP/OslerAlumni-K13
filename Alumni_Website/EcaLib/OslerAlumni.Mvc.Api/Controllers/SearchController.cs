using System.Globalization;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Api.Services;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Api.Controllers
{
    /// <summary>
    /// Search API to be used by front-end functionality.
    /// It expects to run in the context of a session
    /// of an authenticated user on the main site.
    /// </summary>
    [RoutePrefix("search")]
    [Authorize]
    public class SearchController
        : BaseApiController
    {
        #region "Private fields"

        private readonly IAuthorizationService _authorizationService;
        private readonly ISearchService<BoardOpportunitySearchRequest, BoardOpportunity> _boardOpportunityService;
        private readonly ISearchService<DevelopmentResourceSearchRequest, DevelopmentResource> _developmentResourceService;
        private readonly ISearchService<EventSearchRequest, Event> _eventSearchService;
        private readonly ISearchService<GlobalSearchRequest, GlobalResult> _globalSearchService;
        private readonly ISearchService<JobSearchRequest, Job> _jobSearchService;
        private readonly ISearchService<NewsSearchRequest, News> _newsSearchService;
        private readonly ISearchService<ProfileSearchRequest, Profile> _profileSearchService;
        private readonly ISearchService<ResourceSearchRequest, Resource> _resourceSearchService;

        private readonly SearchConfig _searchConfig;

        #endregion

        public SearchController(
            IAuthorizationService authorizationService,
            ISearchService<BoardOpportunitySearchRequest, BoardOpportunity> boardOpportunityService,
            ISearchService<DevelopmentResourceSearchRequest, DevelopmentResource> developmentResourceService,
            ISearchService<EventSearchRequest, Event> eventSearchService,
            ISearchService<GlobalSearchRequest, GlobalResult> globalSearchService,
            ISearchService<JobSearchRequest, Job> jobSearchService,
            ISearchService<NewsSearchRequest, News> newsSearchService,
            ISearchService<ProfileSearchRequest, Profile> profileSearchService,
            ISearchService<ResourceSearchRequest, Resource> resourceSearchService,
            SearchConfig searchConfig)
        {
            _authorizationService = authorizationService;
            _boardOpportunityService = boardOpportunityService;
            _developmentResourceService = developmentResourceService;
            _eventSearchService = eventSearchService;
            _globalSearchService = globalSearchService;
            _jobSearchService = jobSearchService;
            _newsSearchService = newsSearchService;
            _profileSearchService = profileSearchService;
            _resourceSearchService = resourceSearchService;

            _searchConfig = searchConfig;
        }

        #region "Actions"


        /// <summary>
        /// Endpoint for searching Board Opportunities.
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        /// <remarks>
        /// This search returns all currently published Board Opportunities.
        /// </remarks>
        [Route("boardOpportunities")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<BoardOpportunity>))]
        public IHttpActionResult BoardOpportunities(
            [FromBody] BoardOpportunitySearchRequest searchRequest)
        {
            return Search(
                searchRequest,
                _boardOpportunityService);
        }


        /// <summary>
        /// Endpoint for development resources search
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [Route("developmentResources")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<DevelopmentResource>))]
        public IHttpActionResult DevelopmentResources(
            [FromBody] DevelopmentResourceSearchRequest searchRequest)
        {
            return Search(searchRequest, _developmentResourceService);
        }

        /// <summary>
        /// Endpoint for directory search of alumni profiles
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [Route("directory")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<Profile>))]
        public IHttpActionResult Directory(
            [FromBody] ProfileSearchRequest searchRequest)
        {
            return Search(
                searchRequest,
                _profileSearchService);
        }

        /// <summary>
        /// Endpoint for searching events.
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        /// <remarks>
        /// This search returns all types of upcoming events as well
        /// as past events that are of type On-Demand Webinar.
        /// </remarks>
        [Route("events")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<Event>))]
        public IHttpActionResult Events(
            [FromBody] EventSearchRequest searchRequest)
        {
            return Search(
                searchRequest,
                _eventSearchService);
        }

        /// <summary>
        /// Endpoint for global site search:
        /// provides ability to search through all valid Page Types on the site.
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [Route("global")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<Page>))]
        public IHttpActionResult GlobalSearch(
            [FromBody] GlobalSearchRequest searchRequest)
        {
            return Search(
                searchRequest,
                _globalSearchService);
        }

        /// <summary>
        /// Endpoint for searching job postings.
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        /// <remarks>
        /// This search returns all currently published jobs postings.
        /// </remarks>
        [Route("jobs")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<Job>))]
        public IHttpActionResult Jobs(
            [FromBody] JobSearchRequest searchRequest)
        {
            return Search(
                searchRequest,
                _jobSearchService);
        }

        /// <summary>
        /// Endpoint for news article search (including spotlights).
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [Route("news")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<News>))]
        public IHttpActionResult News(
            [FromBody] NewsSearchRequest searchRequest)
        {
            return Search(
                searchRequest,
                _newsSearchService);
        }

        /// <summary>
        /// Endpoint for resources search
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [Route("resources")]
        [HttpPost]
        [ResponseType(typeof(SearchResponse<Resource>))]
        public IHttpActionResult Resources(
            [FromBody] ResourceSearchRequest searchRequest)
        {
            return Search(searchRequest, _resourceSearchService);
        }

        #endregion

        #region "Methods"

        public IHttpActionResult Search<TResult, TRequest>(
            TRequest searchRequest,
            ISearchService<TRequest, TResult> searchService)
            where TRequest : SearchRequest<TResult>, new()
            where TResult : class, ISearchable, new()
        {
            if (searchRequest == null)
            {
                return BadRequest($"Missing '{nameof(searchRequest)}' parameter.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            searchRequest.FilterForCompetitor = _authorizationService.CurrentUserHasCompetitorRole();

            searchRequest.IndexName = _searchConfig.IndexName;

            var response = searchService.Search(searchRequest);

            if (response == null)
            {
                return InternalServerError();
            }

            var defaultSettings = JsonConvert.DefaultSettings();

            defaultSettings.DateFormatString = StringHelper.GetDateTimeFormat(searchRequest.Culture);

            defaultSettings.Culture =
                new CultureInfo(searchRequest.Culture);

            return Json(response, defaultSettings);
        }
        
        #endregion
    }
}

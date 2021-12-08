using System.Collections.Generic;
using System.Linq;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Nito.AsyncEx;
using OslerAlumni.Core.Definitions;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceFunctionService
        : ServiceBase, IOnePlaceFunctionService
    {
        #region "Constants"

      
        #endregion

        #region "Private fields"

        private readonly IOnePlaceDataService _onePlaceDataService;
        private readonly ICacheService _cacheService;
        private readonly IEventLogRepository _eventLogRepository;

        #endregion

        public OnePlaceFunctionService(
            IOnePlaceDataService onePlaceDataService, ICacheService cacheService, IEventLogRepository eventLogRepository)
        {
            _onePlaceDataService = onePlaceDataService;
            _cacheService = cacheService;
            _eventLogRepository = eventLogRepository;
        }

        #region "Methods"

        public List<OnePlaceFunction> GetFunctions()
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

                    if (!TryGetOnePlaceFunctions(out responseList, out message))
                    {
                        _eventLogRepository.LogError(GetType(), nameof(GetFunctions), message);

                        return null;
                    }

                    return responseList;

                }, cacheParameters);

            return response?.ToList() ?? new List<OnePlaceFunction>(); 
        }

        public bool TryGetOnePlaceFunctions(
            out IList<OnePlaceFunction> functions,
            out string errorMessage)
        {
            errorMessage = null;

            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetFunctions,
                PageType_OnePlaceQueries.CLASS_NAME);


            IList<OnePlaceFunction> functionList = null;
            string message = null;

            var isSuccess = AsyncContext.Run(() =>
                _onePlaceDataService.TryGetList(
                    query,
                    out functionList,
                    out message));

            functions = functionList;
            errorMessage = message;

            return isSuccess;
        }


        #endregion
    }
}

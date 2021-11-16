using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Nito.AsyncEx;
using NuGet;
using OslerAlumni.OnePlace.Delegates;
using OslerAlumni.OnePlace.Models;
using Salesforce.Common;
using Salesforce.Common.Models.Json;
using Salesforce.Force;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceDataService
        : ServiceBase, IOnePlaceDataService
    {
        #region "Constants"

        protected const string OnePlaceClientError = "Failed to initialize OnePlace (SalesForce) client.";
        protected const string OnePlaceEmptyResponse = "Empty response received from OnePlace.";

        #endregion

        #region "Private fields"

        protected readonly IEventLogRepository _eventLogRepository;
        protected readonly IOnePlaceConnectionService _onePlaceConnectionService;
        protected readonly OnePlaceConfig _onePlaceConfig;

        // Semaphore that will grant access to only 1 thread at a time
        private readonly SemaphoreSlim _clientSemaphore = new SemaphoreSlim(1, 1);

        private IForceClient _client;

        #endregion
        
        public OnePlaceDataService(
            IEventLogRepository eventLogRepository,
            IOnePlaceConnectionService onePlaceConnectionService,
            OnePlaceConfig onePlaceConfig)
        {
            _eventLogRepository = eventLogRepository;
            _onePlaceConnectionService = onePlaceConnectionService;
            _onePlaceConfig = onePlaceConfig;
        }

        #region "Methods"

        public bool TryCreate(
            string objectName,
            object obj,
            out string objectId,
            out string errorMessage)
        {
            var client =
                AsyncContext.Run(() => GetOrSetClient());

            if (client == null)
            {
                objectId = null;
                errorMessage = OnePlaceClientError;

                return false;
            }

            var response = client
                .CreateAsync(
                    objectName,
                    obj)
                .GetAwaiter()
                .GetResult();

            if (response == null)
            {
                objectId = null;
                errorMessage = OnePlaceEmptyResponse;

                return false;
            }

            objectId = response.Id;
            errorMessage = response.Errors?.ToString();

            return response.Success;
        }

        public bool TryDelete(
            string objectName,
            string objectId,
            out string errorMessage)
        {
            var client =
                AsyncContext.Run(() => GetOrSetClient());

            if (client == null)
            {
                errorMessage = OnePlaceClientError;

                return false;
            }

            errorMessage = null;

            var result = client
                .DeleteAsync(
                    objectName,
                    objectId)
                .GetAwaiter()
                .GetResult();

            return result;
        }

        /// <summary>
        /// Adds a where condition that filters the list of returned items by a main reference, e.g. ID or name.
        /// </summary>
        /// <typeparam name="T">Type of the object to be returned.</typeparam>
        /// <param name="mainReference">Id of the item to look up.</param>
        /// <param name="listFunc">
        /// Method that returns the list of items. The method has to accept 4 parameters:
        /// - list of main references (e.g. IDs or names);
        /// - list of column names;
        /// - (output) list of results;
        /// - (output) an error message, if available;
        /// - number of results to return.
        /// Return value of the method should indicate if the list retrieval was successful or
        /// if it encountered any errors.
        /// </param>
        /// <param name="columnNames"></param>
        /// <param name="result"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool TryGetByMainReference<T>(
            string mainReference,
            ListFunctionDelegate<T> listFunc,
            IList<string> columnNames,
            out T result,
            out string errorMessage)
            where T : class, new()
        {
            result = null;

            if (string.IsNullOrWhiteSpace(mainReference))
            {
                errorMessage = "Cannot look up a record by empty ID";

                return false;
            }

            if (listFunc == null)
            {
                errorMessage = "Method for retrieving a list of records from OnePlace was not provided";

                return false;
            }

            IList<T> records;

            var isSuccess = listFunc(
                new List<string> { mainReference },
                columnNames,
                out records,
                out errorMessage,
                1);

            result = records?.FirstOrDefault();

            return isSuccess;
        }

        public bool TryGetList<T>(
            IOnePlaceQuery query,
            out IList<T> resultList,
            out string errorMessage,
            IList<string> columnNames = null,
            IOnePlaceWhereCondition where = null,
            string orderBy = null,
            int? topN = null)
        {
            resultList = null;
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(query?.QueryText))
            {
                errorMessage = "OnePlace query is empty";

                return false;
            }

            query = query
                .Columns(columnNames)
                .Where(where)
                .OrderBy(orderBy)
                .TopN(topN ?? 0);

            resultList = new List<T>();

            // When this is set to null, then no successive block of records exists to retrieve in the query result 
            string trackNextRecordsUrl = null;

            // Loop works with multiple blocks of records from the same query result
            // via the SF result.NextRecordsUrl property 
            do
            {
                // The query argument will be executed unless nextRecordsUrl contains a value,
                // in which case the next block of records will be retrieved
                var nextRecordsUrl = trackNextRecordsUrl;

                // Get the client
                var client = AsyncContext.Run(() =>
                    GetOrSetClient());

                if (client == null)
                {
                    errorMessage = OnePlaceClientError;

                    return false;
                }

                var result = AsyncContext.Run(() =>
                    GetListContinuationAsync<T>(
                        client,
                        query.QueryText,
                        nextRecordsUrl));

                if (result == null)
                {
                    errorMessage = "Failed to obtain results of the OnePlace query";

                    return false;
                }

                var response = result.Response;

                if (response == null)
                {
                    errorMessage = OnePlaceEmptyResponse;

                    return false;
                }

                // Update the client object, in case the authorization token
                // was updated because of the session expiration
                AsyncContext.Run(() =>
                    GetOrSetClient(result.Client));

                resultList.AddRange(response.Records);

                // Track the next record block in the result
                trackNextRecordsUrl = response.NextRecordsUrl;
            }
            // Until all records are retrieved
            while (!string.IsNullOrWhiteSpace(trackNextRecordsUrl));

            return true;
        }

        public bool TryUpdate(
            string objectName,
            string objectId,
            object obj,
            out string errorMessage)
        {
            var client =
                AsyncContext.Run(() => GetOrSetClient());

            if (client == null)
            {
                errorMessage = OnePlaceClientError;

                return false;
            }

            var response = client
                .UpdateAsync(
                    objectName,
                    objectId,
                    obj)
                .GetAwaiter()
                .GetResult();

            if (response == null)
            {
                errorMessage = OnePlaceEmptyResponse;

                return false;
            }

            errorMessage = response.Errors?.ToString();

            return response.Success;
        }

        #endregion

        #region "Helper methods"

        /// <summary>
        /// Run the provided SOQL query and return a parsed list of results (asynchronous), or return the next portion
        /// of a query's unretrieved result via param <paramref name="nextRecordsURL"/>.
        /// </summary>
        /// <typeparam name="T">Type of the results in the list.</typeparam>
        /// <param name="client">
        /// Salesforce connection client.
        /// </param>
        /// <param name="query">
        /// SOQL query. This value is ignored if a <paramref name="nextRecordsURL"/> value is specified.
        /// </param>
        /// <param name="nextRecordsURL">
        /// Supports retrieving the next unretrieved portion of a query's results (defaults to null). 
        /// If a value is specified then the <paramref name="query"/> value is ignored.
        /// </param>
        /// <returns>Wrapper object with the list of results.</returns>
        protected async Task<OnePlaceResult<T>> GetListContinuationAsync<T>(
            IForceClient client,
            string query,
            string nextRecordsURL = null)
        {
            Func<IForceClient, Task<OnePlaceResult<T>>> executeFunc =
                async conClient =>
                {
                    try
                    {
                        return new OnePlaceResult<T>
                        {
                            Client = conClient,
                            Response = !string.IsNullOrWhiteSpace(nextRecordsURL)
                                ? await conClient.QueryContinuationAsync<T>(nextRecordsURL)
                                : await conClient.QueryAsync<T>(query)
                        };
                    }
                    catch (Exception ex)
                    {
                        var forceEx = ex as ForceException;

                        _eventLogRepository.LogEvent(
                            GetType().FullName,
                            nameof(GetListContinuationAsync),
                            eventType:
                            // If it's session expiration, log it as a warning, not error
                            ((forceEx != null) && (forceEx.Error == Error.InvalidSessionId))
                                ? "W"
                                : "E",
                            eventDescription: "Query: " + query,
                            exception: ex);

                        throw ex;
                    }
                };

            var result = await _onePlaceConnectionService
                .RetryOnTimeoutAsync(
                    client,
                    _onePlaceConfig,
                    executeFunc);

            return result;
        }

        /// <summary>
        /// Returns the underlying Salesforce client.
        /// </summary>
        /// <returns></returns>
        protected async Task<IForceClient> GetOrSetClient(
            IForceClient client = null)
        {
            if ((client != null) && (_client != client)
                || (client == null) && (_client == null))
            {
                // Locking to 1 thread
                await _clientSemaphore.WaitAsync();

                try
                {
                    if (client != null)
                    {
                        _client = client;
                    }
                    else if (_client == null)
                    {
                        _client = await _onePlaceConnectionService.ConnectAsync(_onePlaceConfig);
                    }
                }
                finally
                {
                    // Always release the semaphore
                    _clientSemaphore.Release();
                }
            }

            return _client;
        }

        #endregion
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Nito.AsyncEx;
using OslerAlumni.Core.Definitions;
using OslerAlumni.OnePlace.Models;
using Salesforce.Common;
using Salesforce.Common.Models.Json;
using Salesforce.Force;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceConnectionService
        : ServiceBase, IOnePlaceConnectionService
    {
        #region "Private fields"

        protected readonly IEventLogRepository _eventLogRepository;
        protected readonly ISettingsKeyRepository _settingsKeyRepository;

        #endregion

        public OnePlaceConnectionService(
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository)
        {
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;
        }

        #region "Methods"

        /// <summary>
        /// Create a connection to Salesforce.
        /// </summary>
        /// <returns>
        /// Salesforce connection client with populated authentication details,
        /// such as HTTP header, Bearer value (access token).
        /// </returns>
        /// <remarks>
        /// Only one connection is needed. However, the connection times out after N minutes,
        /// so this method is called multiple times.
        /// </remarks>
        public virtual IForceClient Connect(
            OnePlaceConfig onePlaceConfig)
        {
            return AsyncContext.Run(
                () => ConnectAsync(onePlaceConfig));
        }

        /// <summary>
        /// Create a connection to Salesforce (asynchronous).
        /// </summary>
        /// <returns>
        /// Salesforce connection client with populated authentication details,
        /// such as HTTP header, Bearer value (access token).
        /// </returns>
        /// <remarks>
        /// Only one connection is needed. However, the connection times out after N minutes,
        /// so this method is called multiple times.
        /// </remarks>
        public virtual async Task<IForceClient> ConnectAsync(
            OnePlaceConfig onePlaceConfig)
        {
            if ((onePlaceConfig == null)
                || string.IsNullOrWhiteSpace(onePlaceConfig.Url))
            {
                return null;
            }

            var isOnePlaceEnabled = _settingsKeyRepository.GetValue<bool>(
                GlobalConstants.Settings.OnePlace.Enabled);

            if (!isOnePlaceEnabled)
            {
                return null;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls;

            var password = onePlaceConfig.Password + onePlaceConfig.SecurityToken;

            var authClient = new AuthenticationClient
            {
                ApiVersion = onePlaceConfig.ApiVersion
            };

            try
            {
                await authClient
                    .UsernamePasswordAsync(
                        onePlaceConfig.ConsumerKey,
                        onePlaceConfig.ConsumerSecret,
                        onePlaceConfig.Username,
                        password,
                        onePlaceConfig.Url);

                return new ForceClient(
                    authClient.InstanceUrl,
                    authClient.AccessToken,
                    authClient.ApiVersion);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(ConnectAsync),
                    ex);

                return null;
            }
        }

        public virtual async Task<T> RetryOnTimeoutAsync<T>(
            IForceClient client,
            OnePlaceConfig onePlaceConfig,
            Func<IForceClient, Task<T>> executeFunc,
            bool propagateExceptions = false)
        {
            // First try
            var result = await TryExecuteAsync(
                client,
                onePlaceConfig,
                executeFunc,
                propagateExceptions);

            // If failed due to timeout, try again
            if (result.IsSessionTimeout)
            {
                // Pass null as the client, so that it's reinitialized
                result = await TryExecuteAsync(
                    null,
                    onePlaceConfig,
                    executeFunc,
                    propagateExceptions);
            }

            return result.Result;
        }

        #endregion

        #region "Helper methods"

        protected virtual async Task<ExecutionResult<T>> TryExecuteAsync<T>(
            IForceClient client,
            OnePlaceConfig onePlaceConfig,
            Func<IForceClient, Task<T>> executeFunc,
            bool propagateExceptions = false)
        {
            var executionResult = new ExecutionResult<T>
            {
                IsSessionTimeout = false,
                Result = default(T)
            };

            try
            {
                if (client == null)
                {
                    client = await ConnectAsync(onePlaceConfig);
                }

                if (client != null)
                {
                    executionResult.Result = await executeFunc(client);
                }
            }
            catch (Exception ex)
            {
                var forceEx = ex as ForceException;

                // Assuming that exception logging is handled within the internal function
                executionResult.IsSessionTimeout =
                    (forceEx != null) && (forceEx.Error == Error.InvalidSessionId);

                if (!executionResult.IsSessionTimeout && propagateExceptions)
                {
                    throw ex;
                }
            }

            return executionResult;
        }

        #endregion

        #region "Helper classes"

        protected class ExecutionResult<T>
        {
            public bool IsSessionTimeout { get; set; }

            public T Result { get; set; }
        }

        #endregion
    }
}

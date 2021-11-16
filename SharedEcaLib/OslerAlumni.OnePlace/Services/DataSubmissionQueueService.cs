using System;
using System.Linq;
using CMS.DataEngine;
using ECA.Core.Services;
using Newtonsoft.Json;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Factories;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Repositories;
using OslerAlumni.OnePlace.Serialization;

namespace OslerAlumni.OnePlace.Services
{
    public class DataSubmissionQueueService
        : ServiceBase, IDataSubmissionQueueService
    {
        #region "Private fields"

        private readonly IDataSubmissionQueueItemRepository _dataSubmissionQueueItemRepository;
        private readonly IDataSubmissionServiceFactory _dataSubmissionServiceFactory;

        #endregion

        public DataSubmissionQueueService(
            IDataSubmissionQueueItemRepository dataSubmissionQueueItemRepository,
            IDataSubmissionServiceFactory dataSubmissionServiceFactory)
        {
            _dataSubmissionQueueItemRepository = dataSubmissionQueueItemRepository;
            _dataSubmissionServiceFactory = dataSubmissionServiceFactory;
        }

        #region "Methods"

        /// <inheritdoc />
        public DataSubmissionResult Create<T>(
            T contextObject,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo
        {
            return TryExecuteAndQueue(
                contextObject,
                DataSubmissionMethod.Post,
                payload,
                dependencyIds: dependencyIds,
                executeImmediately: executeImmediately);
        }

        /// <inheritdoc />
        public DataSubmissionResult Delete<T>(
            T contextObject,
            string externalId,
            Type type,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo
        {
            var instance = Activator.CreateInstance(type);

            return TryExecuteAndQueue(
                contextObject,
                DataSubmissionMethod.Delete,
                instance,
                externalId,
                dependencyIds,
                executeImmediately);
        }

        /// <inheritdoc />
        public DataSubmissionResult Delete<T>(
            T contextObject,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo
        {
            return TryExecuteAndQueue(
                contextObject,
                DataSubmissionMethod.Delete,
                payload,
                dependencyIds: dependencyIds,
                executeImmediately: executeImmediately);
        }

        /// <inheritdoc />
        public DataSubmissionResult Update<T>(
            T contextObject,
            string externalId,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo
        {
            return TryExecuteAndQueue(
                contextObject,
                DataSubmissionMethod.Patch,
                payload,
                externalId,
                dependencyIds,
                executeImmediately);
        }

        /// <inheritdoc />
        public DataSubmissionResult Upsert<T>(
            T contextObject,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo
        {
            return TryExecuteAndQueue(
                contextObject,
                DataSubmissionMethod.Upsert,
                payload,
                dependencyIds: dependencyIds,
                executeImmediately: executeImmediately);
        }

        /// <inheritdoc />
        public DataSubmissionResult Retry(
            CustomTable_DataSubmissionQueueItem item)
        {
            var result = TryExecute(item);

            item = ParseResult(result, item);

            // Update item to reflect current state
            _dataSubmissionQueueItemRepository.Save(item);

            return result;
        }

        #endregion

        #region "Helper methods"

        /// <summary>
        /// Try to persist data in Salesforce. Stores a local record in case of an error.
        /// </summary>
        /// <param name="contextObject"></param>
        /// <param name="method"></param>
        /// <param name="externalId"></param>
        /// <param name="payload"></param>
        /// <param name="dependencyIds"></param>
        /// <param name="executeImmediately"></param>
        /// <returns></returns>
        protected DataSubmissionResult TryExecuteAndQueue<T>(
            T contextObject,
            DataSubmissionMethod method,
            object payload,
            string externalId = null,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo
        {
            if (contextObject == null)
            {
                throw new ArgumentNullException(
                    nameof(contextObject));
            }

            if (payload == null)
            {
                throw new ArgumentNullException(
                    nameof(payload));
            }

            var result = new DataSubmissionResult
            {
                Success = false
            };

            var contextType = typeof(T);
            var payloadType = payload.GetType();

            var item = new CustomTable_DataSubmissionQueueItem
            {
                Method = (int)method,
                ExternalId = externalId,
                TotalAttempts = 0,
                IsProcessed = false,
                ContextObjectType = contextType.AssemblyQualifiedName,
                ContextObjectId = contextObject.Generalized.ObjectID,
                Payload =
                    JsonConvert.SerializeObject(
                        payload,
                        Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CustomContractResolver()
                        }),
                PayloadType = payloadType.AssemblyQualifiedName,
                DependsOnItemIdsArray = dependencyIds
            };

            if (executeImmediately)
            {
                // Try to execute 
                result = TryExecute(item);

                item = ParseResult(result, item);
            }
           
            // Queue the task up
            _dataSubmissionQueueItemRepository.Save(item);

            result.TaskId = item.ItemID;

            return result;
        }

        protected CustomTable_DataSubmissionQueueItem ParseResult(
            DataSubmissionResult result,
            CustomTable_DataSubmissionQueueItem item)
        {
            if ((item == null) || (result == null))
            {
                return item;
            }

            item.IsProcessed = result.Success;

            if (result.Success)
            {
                // Save the object ID after a successful retry
                item.ExternalId = result.ExternalId;
            }

            // Increment # of attempts and log reason
            item.TotalAttempts++;
            item.LastResponse = result.Message;

            return item;
        }

        /// <summary>
        /// Try to persist data in Salesforce
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected DataSubmissionResult TryExecute(
            CustomTable_DataSubmissionQueueItem item)
        {
            var result = new DataSubmissionResult
            {
                Success = false
            };

            try
            {
                object payload;
                Type payloadType;
                string message;

                if (!TryGetPayload(
                        item,
                        out payloadType,
                        out payload,
                        out message))
                {
                    result.Message = message;

                    return result;
                }

                var contextType = Type.GetType(item.ContextObjectType);

                var service = _dataSubmissionServiceFactory
                    .GetDataSubmissionService(contextType, payloadType);

                if (service == null)
                {
                    result.Message =
                        $"Could not find data submission service for object type '{payloadType.Name}'";

                    return result;
                }

                string externalId;

                result.Success =
                    service.TrySubmitPayload(
                        item,
                        payloadType,
                        payload,
                        out externalId,
                        out message);

                // This will either set object ID that we already know
                // or the one we get in response from Salesforce
                result.ExternalId = externalId;

                result.Message = message;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.ToString();
            }

            return result;
        }

        protected bool TryGetPayload(
            CustomTable_DataSubmissionQueueItem item,
            out Type payloadType,
            out object payload,
            out string message)
        {
            payload = null;
            payloadType = null;
            message = null;

            var payloadJson = item.Payload;

            if (string.IsNullOrWhiteSpace(payloadJson))
            {
                message = "Missing object data";

                return false;
            }

            var dependsOnItemIds = item.DependsOnItemIdsArray;

            if ((dependsOnItemIds != null) && (dependsOnItemIds.Length > 0))
            {
                // Obtain the dependencies, unless they have already been obtained and pre-populated
                var dependencies = item.DependsOnItemsArray;

                if ((dependencies == null) || (dependencies.Length != dependsOnItemIds.Length))
                {
                    dependencies = dependsOnItemIds.Select(
                            dependsOnItemId =>
                                _dataSubmissionQueueItemRepository.GetDataSubmissionQueueItem(
                                    dependsOnItemId))
                            .ToArray();

                    item.DependsOnItemsArray = dependencies;
                }

                foreach (var dependency in dependencies)
                {
                    // TODO: [VI] Missing parent logic for when it no longer exists and needs to be re-created?
                    if ((dependency == null) || !dependency.IsProcessed)
                    {
                        message = "Parent task has not been processed yet";

                        return false;
                    }

                    // Generate the placeholder macro in the format of "#ID_XXX#",
                    // where XXX is the ID of the dependency task.
                    var placeholderMacro = string.Format(
                        DataSubmissionConstants.Placeholders.ID,
                        dependency.ItemID);
                    
                    // Replace references
                    payloadJson = payloadJson.Replace(
                        placeholderMacro,
                        dependency.ExternalId);
                }
            }

            payloadType = Type.GetType(item.PayloadType);

            payload = JsonConvert.DeserializeObject(
                payloadJson,
                payloadType);

            if (payload == null)
            {
                message = "Missing object data";

                return false;
            }

            return true;
        }

        #endregion
    }
}

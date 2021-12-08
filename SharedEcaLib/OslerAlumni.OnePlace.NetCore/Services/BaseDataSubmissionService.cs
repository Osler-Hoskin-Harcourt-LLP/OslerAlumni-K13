using System;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Attributes;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Kentico.Models;

namespace OslerAlumni.OnePlace.Services
{
    public abstract class BaseDataSubmissionService
        : ServiceBase, IDataSubmissionService
    {
        #region "Private fields"

        private readonly IOnePlaceDataService _onePlaceDataService;

        #endregion

        protected BaseDataSubmissionService(
            IOnePlaceDataService onePlaceDataService)
        {
            _onePlaceDataService = onePlaceDataService;
        }

        #region "Methods"

        /// <inheritdoc />
        public abstract bool AppliesTo(
            Type contextType,
            Type type);

        /// <inheritdoc />
        public virtual bool TrySubmitPayload(
            CustomTable_DataSubmissionQueueItem item,
            Type payloadType,
            object payload,
            out string externalId,
            out string message)
        {
            var result = false;

            externalId = item.ExternalId;

            var method = (DataSubmissionMethod)item.Method;

            var processResult =
                ProcessPayload(
                    payloadType,
                    payload,
                    method,
                    externalId);

            if (processResult != null)
            {
                externalId = processResult.Id;
                message = processResult.Message;

                if (!processResult.Success
                    || (processResult.Method == DataSubmissionMethod.None))
                {
                    return processResult.Success;
                }

                method = processResult.Method;
                payload = processResult.Payload;
            }

            // Get Salesforce object name from attribute associated with payload type
            var payloadTypeName =
                OnePlaceObjectAttribute.GetObjectName(payloadType);

            switch (method)
            {
                case DataSubmissionMethod.Post:
                    {
                        result = _onePlaceDataService.TryCreate(
                            payloadTypeName,
                            payload,
                            out externalId,
                            out message);

                        break;
                    }
                case DataSubmissionMethod.Patch:
                    {
                        result = _onePlaceDataService.TryUpdate(
                            payloadTypeName,
                            externalId,
                            payload,
                            out message);

                        break;
                    }
                case DataSubmissionMethod.Delete:
                    {
                        result = _onePlaceDataService.TryDelete(
                            payloadTypeName,
                            externalId,
                            out message);

                        break;
                    }
                default:
                    {
                        message = "Unsupported method";

                        break;
                    }
            }

            if (result)
            {
                // Update Salesforce reference (or its macro form, indicating we are awaiting response) in the user/contact record
                result = TryUpdateContextObjectExternalId(
                    item.ContextObjectId,
                    externalId,
                    out message);
            }

            return result;
        }

        /// <inheritdoc />
        public virtual bool TryUpdateContextObjectExternalId(
            int contextObjectId,
            string externalId,
            out string message)
        {
            message = null;

            return true;
        }

        #endregion

        #region "Helper methods"

        protected virtual ProcessingResult TryProcessCreate(
            Type payloadType,
            object payload)
        {
            return new ProcessingResult
            {
                Success = false,
                Message = $"Creating objects of type '{payloadType.Name}' in OnePlace is not supported."
            };
        }

        protected virtual ProcessingResult TryProcessDelete(
            Type payloadType,
            string externalId,
            object payload)
        {
            return new ProcessingResult
            {
                Success = false,
                Message = $"Delete objects of type '{payloadType.Name}' from OnePlace is not supported."
            };
        }

        protected virtual ProcessingResult TryProcessUpdate(
            Type payloadType,
            string externalId,
            object payload)
        {
            return new ProcessingResult
            {
                Success = false,
                Message = $"Updating objects of type '{payloadType.Name}' in OnePlace is not supported."
            };
        }

        protected virtual ProcessingResult TryProcessUpsert(
            Type payloadType,
            string externalId,
            object payload)
        {
            return new ProcessingResult
            {
                Success = false,
                Message = $"Creating or updating objects of type '{payloadType.Name}' in OnePlace is not supported."
            };
        }

        protected ProcessingResult ProcessPayload(
            Type payloadType,
            object payload,
            DataSubmissionMethod method,
            string externalId = null)
        {
            ProcessingResult processResult = null;

            switch (method)
            {
                case DataSubmissionMethod.Patch:
                    {
                        processResult = TryProcessUpdate(
                            payloadType, 
                            externalId,
                            payload);
                    }
                    break;
                case DataSubmissionMethod.Post:
                    {
                        processResult = TryProcessCreate(
                            payloadType,
                            payload);
                    }
                    break;
                case DataSubmissionMethod.Upsert:
                    {
                        processResult = TryProcessUpsert(
                            payloadType,
                            externalId,
                            payload);
                    }
                    break;
                case DataSubmissionMethod.Delete:
                    {
                        processResult = TryProcessDelete(
                            payloadType,
                            externalId, 
                            payload);
                    }
                    break;
            }

            return processResult;
        }

        #endregion

        #region "Helper classes"

        protected class ProcessingResult
        {
            public string Id { get; set; }

            public string Message { get; set; }

            public DataSubmissionMethod Method { get; set; }

            public object Payload { get; set; }

            public bool Success { get; set; }
        }

        #endregion
    }
}

using System;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Kentico.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IDataSubmissionService
        : IService
    {
        /// <summary>
        /// Checks if the particular implementation of the interface applies to
        /// the specific type of a context object and a specific type of data submission object.
        /// </summary>
        /// <param name="contextType">
        /// Type of the Kentico object (e.g. User or Contact), in whose "context"
        /// a OnePlace object should be created/updated/deleted.
        /// </param>
        /// <param name="type">
        /// Type of the OnePlace object that should be created/updated/deleted.
        /// </param>
        /// <returns></returns>
        bool AppliesTo(
            Type contextType,
            Type type);

        /// <summary>
        /// Attempts to submit the payload to OnePlace.
        /// </summary>
        /// <param name="item">
        /// Queued data submission task.
        /// </param>
        /// <param name="payloadType">
        /// Type of the payload object.
        /// </param>
        /// <param name="payload">
        /// Payload object to be submitted to OnePlace.
        /// </param>
        /// <param name="externalId">
        /// Outputs OnePlace ID of the object, if it is determined
        /// that it already exists in OnePlace.
        /// </param>
        /// <param name="message">
        /// Outputs an error message if the data submission fails or cannot proceed for some reason.
        /// </param>
        /// <returns>
        /// True, if the data submission was successful; false, otherwise.
        /// </returns>
        bool TrySubmitPayload(
            CustomTable_DataSubmissionQueueItem item,
            Type payloadType,
            object payload,
            out string externalId,
            out string message);

        /// <summary>
        /// Attempts to set the OnePlace reference of the Kentico object
        /// (e.g. User or Contact), in whose "context" a OnePlace object
        /// was created/updated/deleted.
        /// </summary>
        /// <param name="contextObjectId">
        /// ID of the Kentico object (e.g. User or Contact), in whose "context"
        /// a OnePlace object should be created/updated/deleted.
        /// </param>
        /// <param name="externalId">
        /// OnePlace ID of the object that the Kentico object should reference.
        /// </param>
        /// <param name="message">
        /// Outputs an error message if the action fails or cannot proceed for some reason.
        /// </param>
        /// <returns>
        /// True, if the action succeeded; false, otherwise.
        /// </returns>
        bool TryUpdateContextObjectExternalId(
            int contextObjectId,
            string externalId,
            out string message);
    }
}

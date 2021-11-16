using System;
using CMS.DataEngine;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IDataSubmissionQueueService
        : IService
    {
        /// <summary>
        /// Generates a data submission task for creating a new object in OnePlace.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be created.
        /// </typeparam>
        /// <param name="contextObject">
        /// Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be created.
        /// E.g. creating or updating a User or Contact in Kentico would also require
        /// creating or updating the following objects in OnePlace:
        /// - Account;
        /// - Contact;
        /// - Board Membership Accounts;
        /// - Board Membership Relationships;
        /// - etc.
        /// </param>
        /// <param name="payload">
        /// Payload of the new OnePlace object that needs to be created.
        /// </param>
        /// <param name="dependencyIds">
        /// IDs of other queued data submission tasks, on whose successful execution
        /// the current task should depend. The ID of the OnePlace object that was created
        /// or updated as part of the dependency task should be referenced in the current
        /// object payload as "#ID_XXX#" macro placeholder, where XXX is the ID of
        /// the dependency task.
        /// </param>
        /// <param name="executeImmediately">
        /// Indicates if the current data submission task should be tried immediately,
        /// or be left to be handled by the data submission queue task at a later time. 
        /// </param>
        /// <returns>
        /// An instance of <see cref="DataSubmissionResult"/>, indicating the ID of the
        /// data submission task that was generated, so that it can be passed in as the
        /// dependencyId for the next data submission task in the chain.
        /// </returns>
        DataSubmissionResult Create<T>(
            T contextObject,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo;

        /// <summary>
        /// Generates a data submission task for deleting an existing object in OnePlace.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be deleted.
        /// </typeparam>
        /// <param name="contextObject">
        /// Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be deleted.
        /// </param>
        /// <param name="externalId">
        /// OnePlace ID of the object that needs to be deleted. This ID does NOT need
        /// to be part of the actual payload.
        /// </param>
        /// <param name="type">
        /// Type of the OnePlace object that needs to be deleted.
        /// </param>
        /// <param name="dependencyIds">
        /// IDs of other queued data submission tasks, on whose successful execution
        /// the current task should depend.
        /// </param>
        /// <param name="executeImmediately">
        /// Indicates if the current data submission task should be tried immediately,
        /// or be left to be handled by the data submission queue task at a later time. 
        /// </param>
        /// <returns>
        /// An instance of <see cref="DataSubmissionResult"/>, indicating the ID of the
        /// data submission task that was generated, so that it can be passed in as the
        /// dependencyId for the next data submission task in the chain.
        /// </returns>
        DataSubmissionResult Delete<T>(
            T contextObject,
            string externalId,
            Type type,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo;

        /// <summary>
        /// Generates a data submission task for deleting an existing object in OnePlace.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be deleted.
        /// </typeparam>
        /// <param name="contextObject">
        /// Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be deleted.
        /// </param>
        /// <param name="payload">
        /// Payload of the OnePlace object that needs to be deleted.
        /// </param>
        /// <param name="dependencyIds">
        /// IDs of other queued data submission tasks, on whose successful execution
        /// the current task should depend. The ID of the OnePlace object that was created
        /// or updated as part of the dependency task should be referenced in the current
        /// object payload as "#ID_XXX#" macro placeholder, where XXX is the ID of
        /// the dependency task.
        /// </param>
        /// <param name="executeImmediately">
        /// Indicates if the current data submission task should be tried immediately,
        /// or be left to be handled by the data submission queue task at a later time. 
        /// </param>
        /// <returns>
        /// An instance of <see cref="DataSubmissionResult"/>, indicating the ID of the
        /// data submission task that was generated, so that it can be passed in as the
        /// dependencyId for the next data submission task in the chain.
        /// </returns>
        DataSubmissionResult Delete<T>(
            T contextObject,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo;

        /// <summary>
        /// Generates a data submission task for updating an existing object in OnePlace.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be updated.
        /// </typeparam>
        /// <param name="contextObject">
        /// Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be updated.
        /// E.g. creating or updating a User or Contact in Kentico would also require
        /// creating or updating the following objects in OnePlace:
        /// - Account;
        /// - Contact;
        /// - Board Membership Accounts;
        /// - Board Membership Relationships;
        /// - etc.
        /// </param>
        /// <param name="externalId">
        /// OnePlace ID of the object that needs to be updated. This ID does NOT need
        /// to be part of the actual payload.
        /// </param>
        /// <param name="payload">
        /// Payload of the OnePlace object that needs to be updated.
        /// </param>
        /// <param name="dependencyIds">
        /// IDs of other queued data submission tasks, on whose successful execution
        /// the current task should depend. The ID of the OnePlace object that was created
        /// or updated as part of the dependency task should be referenced in the current
        /// object payload as "#ID_XXX#" macro placeholder, where XXX is the ID of
        /// the dependency task.
        /// </param>
        /// <param name="executeImmediately">
        /// Indicates if the current data submission task should be tried immediately,
        /// or be left to be handled by the data submission queue task at a later time. 
        /// </param>
        /// <returns>
        /// An instance of <see cref="DataSubmissionResult"/>, indicating the ID of the
        /// data submission task that was generated, so that it can be passed in as the
        /// dependencyId for the next data submission task in the chain.
        /// </returns>
        DataSubmissionResult Update<T>(
            T contextObject,
            string externalId,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo;

        /// <summary>
        /// Generates a data submission task for creating (if the object doesn't exists)
        /// or updating (if the object already exists) an object in OnePlace.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be created or updated.
        /// </typeparam>
        /// <param name="contextObject">
        /// Kentico object (e.g. User or Contact), in whose "context"
        /// this OnePlace object needed to be created or updated.
        /// E.g. creating or updating a User or Contact in Kentico would also require
        /// creating or updating the following objects in OnePlace:
        /// - Account;
        /// - Contact;
        /// - Board Membership Accounts;
        /// - Board Membership Relationships;
        /// - etc.
        /// </param>
        /// <param name="payload">
        /// Payload of the OnePlace object that needs to be created or updated.
        /// </param>
        /// <param name="dependencyIds">
        /// IDs of other queued data submission tasks, on whose successful execution
        /// the current task should depend. The ID of the OnePlace object that was created
        /// or updated as part of the dependency task should be referenced in the current
        /// object payload as "#ID_XXX#" macro placeholder, where XXX is the ID of
        /// the dependency task.
        /// </param>
        /// <param name="executeImmediately">
        /// Indicates if the current data submission task should be tried immediately,
        /// or be left to be handled by the data submission queue task at a later time. 
        /// </param>
        /// <returns>
        /// An instance of <see cref="DataSubmissionResult"/>, indicating the ID of the
        /// data submission task that was generated, so that it can be passed in as the
        /// dependencyId for the next data submission task in the chain.
        /// </returns>
        DataSubmissionResult Upsert<T>(
            T contextObject,
            object payload,
            int[] dependencyIds = null,
            bool executeImmediately = false)
            where T : IInfo;

        /// <summary>
        /// Tries to execute a queued data submission task.
        /// </summary>
        /// <param name="item">
        /// Queued data submission task to try to execute.
        /// </param>
        /// <returns>
        /// An instance of <see cref="DataSubmissionResult"/>, indicating if the
        /// data submission was successful and providing an error message, if it was not.
        /// </returns>
        DataSubmissionResult Retry(
            CustomTable_DataSubmissionQueueItem item);
    }
}

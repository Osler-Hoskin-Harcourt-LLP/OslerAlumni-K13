using System;
using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceContactService
        : IService
    {
        /// <inheritdoc cref="TryGetContacts(IList{string}, IList{string}, bool, out IList{Contact}, out string, int?)" />
        bool TryGetAlumniContacts(
            IList<string> ids,
            out IList<Contact> contacts,
            out string errorMessage,
            IList<string> columnNames = null,
            bool includeMergeParents = false);

        /// <summary>
        /// Returns the list of contacts from OnePlace,
        /// filtered by the criteria that qualify them as Osler Alumni. 
        /// </summary>
        /// <param name="topN">
        /// Max number of contacts to returned from the top of the list,
        /// which is ordered by LastModidfiedDate, ascending.
        /// </param>
        /// <param name="contacts"></param>
        /// <param name="errorMessage"></param>
        /// <param name="modifiedStartDate">
        /// Date/time after which the contacts must have been modified
        /// in order to be included it the returned list.
        /// </param>
        /// <returns></returns>
        bool TryGetAlumniContacts(
            int topN,
            out IList<Contact> contacts,
            out string errorMessage,
            DateTime? modifiedStartDate = null);

        /// <summary>
        /// Returns the contact by its OnePlace ID.
        /// </summary>
        /// <param name="id">OnePlace ID of the contact.</param>
        /// <param name="columnNames"></param>
        /// <param name="contact"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool TryGetContact(
            string id,
            IList<string> columnNames,
            out Contact contact,
            out string errorMessage);

        bool TryGetContactMergeParent(
            string id,
            IList<string> columnNames,
            out Contact contact,
            out string errorMessage);

        /// <inheritdoc cref="TryGetContacts(IList{string}, IList{string}, bool, out IList{Contact}, out string, int?)" />
        bool TryGetContacts(
            IList<string> ids,
            IList<string> columnNames,
            out IList<Contact> contacts,
            out string errorMessage,
            int? topN = null);

        /// <summary>
        /// Returns the list of contacts from OnePlace matching the given list of IDs. 
        /// </summary>
        /// <param name="ids">
        /// The list of contact IDs, whose information should be returned.
        /// </param>
        /// <param name="columnNames">
        /// The list of contact fields to include in the returned results.
        /// </param>
        /// <param name="includeMergeParents"></param>
        /// <param name="contacts"></param>
        /// <param name="errorMessage"></param>
        /// <param name="topN"></param>
        /// <returns></returns>
        bool TryGetContacts(
            IList<string> ids,
            IList<string> columnNames,
            bool includeMergeParents,
            out IList<Contact> contacts,
            out string errorMessage,
            int? topN = null);
    }
}

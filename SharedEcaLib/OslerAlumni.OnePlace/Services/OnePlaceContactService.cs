using System;
using System.Collections.Generic;
using System.Linq;
using DotNetOpenAuth.Messaging;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Services;
using Nito.AsyncEx;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceContactService
        : ServiceBase, IOnePlaceContactService
    {
        #region "Constants"

        protected static readonly Type ContactType = typeof(Contact);

        protected static readonly string[] DefaultColumnNames =
        {
            ContactType
                .GetPropertyName(nameof(Contact.Id), NameSource.Json),
            ContactType
                .GetPropertyName(nameof(Contact.IsAlumni), NameSource.Json),
            ContactType
                .GetPropertyName(nameof(Contact.HasLeftOnBadTerms), NameSource.Json),
            ContactType
                .GetPropertyName(nameof(Contact.Status), NameSource.Json)
        };

        #endregion

        #region "Private fields"

        private readonly IOnePlaceDataService _onePlaceDataService;

        #endregion

        public OnePlaceContactService(
            IOnePlaceDataService onePlaceDataService)
        {
            _onePlaceDataService = onePlaceDataService;
        }

        #region "Methods"

        /// <inheritdoc />
        public bool TryGetAlumniContacts(
            IList<string> ids,
            out IList<Contact> contacts,
            out string errorMessage,
            IList<string> columnNames = null,
            bool includeMergeParents = false)
        {
            var columnNamesList = columnNames ?? new List<string>();

            columnNamesList.AddRange(DefaultColumnNames);

            return TryGetContacts(
                ids,
                columnNamesList,
                includeMergeParents,
                out contacts,
                out errorMessage);
        }

        /// <inheritdoc />
        public bool TryGetAlumniContacts(
            int topN,
            out IList<Contact> contacts,
            out string errorMessage,
            DateTime? modifiedStartDate = null)
        {
            errorMessage = null;

            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetAlumniContactsWithRelatedData,
                PageType_OnePlaceQueries.CLASS_NAME);

            var lastModifiedPropertyName =
                typeof(Contact).GetPropertyName(
                    nameof(Contact.LastModifiedDate),
                    NameSource.Json);

            IOnePlaceWhereCondition where = null;

            if (modifiedStartDate.HasValue)
            {
                where = new OnePlaceWhereCondition()
                    .WhereGreaterOrEqualThan(
                        lastModifiedPropertyName,
                        modifiedStartDate.Value);
            }

            var orderBy =
                $"{lastModifiedPropertyName} ASC";

            IList<Contact> contactList = null;
            string message = null;

            var isSuccess = AsyncContext.Run(() =>
                _onePlaceDataService.TryGetList(
                    query,
                    out contactList,
                    out message,
                    where: where,
                    orderBy: orderBy,
                    topN: topN));

            contacts = contactList;
            errorMessage = message;

            return isSuccess;
        }

        /// <inheritdoc />
        public bool TryGetContact(
            string id,
            IList<string> columnNames,
            out Contact contact,
            out string errorMessage)
        {
            return _onePlaceDataService.TryGetByMainReference(
                id,
                TryGetContacts,
                columnNames,
                out contact,
                out errorMessage);
        }

        public bool TryGetContactMergeParent(
            string id,
            IList<string> columnNames,
            out Contact contact,
            out string errorMessage)
        {
            contact = null;

            if (string.IsNullOrWhiteSpace(id))
            {
                errorMessage = "Cannot look up merge parent for an empty contact ID";

                return false;
            }

            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetContacts,
                PageType_OnePlaceQueries.CLASS_NAME);
            
            var mergedContactIdsPropertyName =
                typeof(Contact).GetPropertyName(
                    nameof(Contact.MergedContactIds),
                    NameSource.Json);

            var where = new OnePlaceWhereCondition()
                .WhereLike(
                    mergedContactIdsPropertyName,
                    id);

            IList<Contact> contacts = null;
            string message = null;
            
            var isSuccess =
                AsyncContext.Run(() =>
                    _onePlaceDataService.TryGetList(
                        query,
                        out contacts,
                        out message,
                        columnNames,
                        where,
                        topN: 1));

            contact = contacts?.FirstOrDefault();
            errorMessage = message;

            return isSuccess;
        }

        /// <inheritdoc />
        public bool TryGetContacts(
            IList<string> ids,
            IList<string> columnNames,
            out IList<Contact> contacts,
            out string errorMessage,
            int? topN = null)
        {
            return TryGetContacts(
                ids,
                columnNames,
                false,
                out contacts,
                out errorMessage,
                topN);
        }

        /// <inheritdoc />
        public bool TryGetContacts(
            IList<string> ids,
            IList<string> columnNames,
            bool includeMergeParents,
            out IList<Contact> contacts,
            out string errorMessage,
            int? topN = null)
        {
            contacts = null;

            if ((ids == null) || (ids.Count < 1))
            {
                errorMessage = "List of IDs for contact lookup was empty";

                return false;
            }

            var columnNamesList = columnNames ?? new List<string>();

            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetContacts,
                PageType_OnePlaceQueries.CLASS_NAME);

            var idPropertyName =
                typeof(Contact).GetPropertyName(
                    nameof(Contact.Id),
                    NameSource.Json);

            IOnePlaceWhereCondition where = null;

            where = new OnePlaceWhereCondition()
                .WhereIn(
                    idPropertyName,
                    ids);

            if (includeMergeParents)
            {
                var mergedContactIdsPropertyName =
                    typeof(Contact).GetPropertyName(
                        nameof(Contact.MergedContactIds),
                        NameSource.Json);

                columnNamesList.Add(
                    mergedContactIdsPropertyName);

                foreach (var id in ids)
                {
                    where = where
                        .Or(new OnePlaceWhereCondition()
                                .WhereLike(
                                    mergedContactIdsPropertyName,
                                    id));
                }
            }

            var orderBy =
                $"{idPropertyName} ASC";

            IList<Contact> contactList = null;
            string message = null;

            var isSuccess =
                AsyncContext.Run(() =>
                    _onePlaceDataService.TryGetList(
                        query,
                        out contactList,
                        out message,
                        columnNamesList,
                        where,
                        orderBy,
                        topN));

            contacts = contactList;
            errorMessage = message;

            return isSuccess;
        }

        #endregion
    }
}

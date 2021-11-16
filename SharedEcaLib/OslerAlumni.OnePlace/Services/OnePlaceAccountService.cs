using System.Collections.Generic;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Services;
using Nito.AsyncEx;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceAccountService
        : ServiceBase, IOnePlaceAccountService
    {
        #region "Private fields"

        private readonly IOnePlaceDataService _onePlaceDataService;

        #endregion

        public OnePlaceAccountService(
            IOnePlaceDataService onePlaceDataService)
        {
            _onePlaceDataService = onePlaceDataService;
        }

        #region "Methods"

        /// <inheritdoc />
        public bool TryGetAccount(
            string id,
            IList<string> columnNames,
            out Account account,
            out string errorMessage)
        {
            return _onePlaceDataService.TryGetByMainReference(
                id,
                TryGetAccounts,
                columnNames,
                out account,
                out errorMessage);
        }

        /// <inheritdoc />
        public bool TryGetAccountByName(
            string name,
            IList<string> columnNames,
            out Account account,
            out string errorMessage)
        {
            return _onePlaceDataService.TryGetByMainReference(
                name,
                TryGetAccountsByName,
                columnNames,
                out account,
                out errorMessage);
        }

        /// <inheritdoc />
        public bool TryGetAccounts(
            IList<string> ids,
            IList<string> columnNames,
            out IList<Account> accounts,
            out string errorMessage,
            int? topN = null)
        {
            return TryGetAccounts(
                nameof(Account.Id),
                ids,
                columnNames,
                out accounts,
                out errorMessage,
                topN);
        }

        /// <inheritdoc />
        public bool TryGetAccountsByName(
            IList<string> names,
            IList<string> columnNames,
            out IList<Account> accounts,
            out string errorMessage,
            int? topN = null)
        {
            return TryGetAccounts(
                nameof(Account.Name),
                names,
                columnNames,
                out accounts,
                out errorMessage,
                topN);
        }

        #endregion

        #region "Helper methods"

        protected bool TryGetAccounts(
            string referenceProperty,
            IList<string> references,
            IList<string> columnNames,
            out IList<Account> accounts,
            out string errorMessage,
            int? topN = null)
        {
            accounts = null;

            if ((references == null) || (references.Count < 1))
            {
                errorMessage = "List of references for account lookup was empty";

                return false;
            }

            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetAccounts,
                PageType_OnePlaceQueries.CLASS_NAME);

            var referencePropertyName =
                typeof(Account).GetPropertyName(
                    referenceProperty,
                    NameSource.Json);

            IOnePlaceWhereCondition where = null;

            where = new OnePlaceWhereCondition()
                .WhereIn(
                    referencePropertyName,
                    references);

            var orderBy =
                $"{referencePropertyName} ASC";

            IList<Account> accountList = null;
            string message = null;

            var isSuccess =
                AsyncContext.Run(() =>
                    _onePlaceDataService.TryGetList(
                        query,
                        out accountList,
                        out message,
                        columnNames,
                        where,
                        orderBy,
                        topN));

            accounts = accountList;
            errorMessage = message;

            return isSuccess;
        }

        #endregion
    }
}

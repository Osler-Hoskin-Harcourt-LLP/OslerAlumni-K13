using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceAccountService
        : IService
    {
        bool TryGetAccount(
            string id,
            IList<string> columnNames,
            out Account account,
            out string errorMessage);

        bool TryGetAccountByName(
            string name,
            IList<string> columnNames,
            out Account account,
            out string errorMessage);

        bool TryGetAccounts(
            IList<string> ids,
            IList<string> columnNames,
            out IList<Account> accounts,
            out string errorMessage,
            int? topN = null);

        bool TryGetAccountsByName(
            IList<string> names,
            IList<string> columnNames,
            out IList<Account> accounts,
            out string errorMessage,
            int? topN = null);
    }
}

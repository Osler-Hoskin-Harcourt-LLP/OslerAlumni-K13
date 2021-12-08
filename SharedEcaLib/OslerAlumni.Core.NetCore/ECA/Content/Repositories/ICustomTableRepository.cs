using System;
using System.Collections.Generic;
using CMS.CustomTables;
using ECA.Core.Repositories;

namespace ECA.Content.Repositories
{
    /// <remarks>
    /// This class and its implementations do NOT use caching.
    /// Caching should be applied on the results of corrensponding methods,
    /// depending on the individual usage context.
    /// </remarks>
    public interface ICustomTableRepository 
        : IRepository
    {
        string EnabledColumnName { get; set; }

        T GetCustomTableItemByGuid<T>(
            Guid guid,
            IEnumerable<string> columnNames = null,
            bool enabledOnly = true,
            string siteName = null)
            where T : CustomTableItem, new();
        
        List<T> GetCustomTableItemsByGuids<T>(
            List<Guid> guids,
            IEnumerable<string> columnNames = null,
            bool enabledOnly = true,
            string orderBy = null,
            string siteName = null)
            where T : CustomTableItem, new();

        List<T> GetAllCustomTableItems<T>(
            IEnumerable<string> columnNames = null,
            bool enabledOnly = true,
            string orderBy = null,
            string siteName = null)
            where T : CustomTableItem, new();
    }
}

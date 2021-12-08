using System;
using System.Collections.Generic;
using System.Linq;
using CMS.CustomTables;
using CMS.DataEngine;
using ECA.Core.Extensions;
using ECA.Core.Models;

namespace ECA.Content.Repositories
{
    /// <inheritdoc />
    public class CustomTableRepository
        : ICustomTableRepository
    {
        #region "Private fields"

        protected readonly ContextConfig _context;

        #endregion

        #region "Properties"

        public string EnabledColumnName { get; set; }
            = "Enabled";

        #endregion

        public CustomTableRepository(
            ContextConfig context)
        {
            _context = context;
        }

        #region "Methods"

        public T GetCustomTableItemByGuid<T>(
            Guid guid,
            IEnumerable<string> columnNames = null,
            bool enabledOnly = true,
            string siteName = null)
            where T : CustomTableItem, new()
        {
            T item = GetDefaultQuery<T>(
                    columnNames,
                    enabledOnly,
                    siteName: siteName)
                .WhereEquals(
                    nameof(CustomTableItem.ItemGUID), guid).FirstOrDefault();

            return item;
        }

        public List<T> GetCustomTableItemsByGuids<T>(
            List<Guid> guids,
            IEnumerable<string> columnNames = null,
            bool enabledOnly = true,
            string orderBy = null,
            string siteName = null)
            where T : CustomTableItem, new()
        {
            List<T> items = GetDefaultQuery<T>(
                    columnNames,
                    enabledOnly,
                    orderBy,
                    siteName)
                .WhereIn(nameof(CustomTableItem.ItemGUID), guids)
                .ToList();

            return items;
        }

        public List<T> GetAllCustomTableItems<T>(
            IEnumerable<string> columnNames = null,
            bool enabledOnly = true,
            string orderBy = null,
            string siteName = null)
            where T : CustomTableItem, new()
        {
            return GetDefaultQuery<T>(
                    columnNames,
                    enabledOnly,
                    orderBy,
                    siteName)
                .ToList();
        }

        #endregion

        #region "Helper methods"

        private ObjectQuery<T> GetDefaultQuery<T>(
            IEnumerable<string> columnNames = null,
            bool enabledOnly = true,
            string orderBy = null,
            string siteName = null)
            where T : CustomTableItem, new()
        {
            var query = CustomTableItemProvider
                .GetItems<T>()
                .Columns(columnNames)
                .OnSite(siteName.ReplaceIfEmpty(_context.Site?.SiteName));

            if (enabledOnly && !string.IsNullOrWhiteSpace(EnabledColumnName))
            {
                query = query.WhereTrue(EnabledColumnName);
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderByAscending(orderBy);
            }

            return query;
        }

        #endregion
    }
}

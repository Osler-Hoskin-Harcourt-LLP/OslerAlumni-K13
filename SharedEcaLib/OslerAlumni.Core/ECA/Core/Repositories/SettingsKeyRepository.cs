using System;
using CMS.DataEngine;
using CMS.Helpers;
using ECA.Core.Extensions;
using ECA.Core.Models;

namespace ECA.Core.Repositories
{
    /// <remarks>
    /// Note that site settings do NOT need to use caching,
    /// because Kentico uses a hash table to store them for the duration of the application lifetime.
    /// Whenever site settings are updated via CMS admin area, Kentico updates the hashtable record for them as well.
    /// Changes made directly in the DB will not be reflected in the hashtable without clearing the site cache.
    /// </remarks>
    public class SettingsKeyRepository
        : ISettingsKeyRepository
    {
        #region "Private fields"

        private readonly ContextConfig _context;

        #endregion

        public SettingsKeyRepository(
            ContextConfig context)
        {
            _context = context;
        }

        #region "Methods"

        public T GetValue<T>(
            string keyName,
            string culture = null,
            string siteName = null)
        {
            if (!string.IsNullOrWhiteSpace(culture))
            {
                keyName = $"{keyName}_{CultureHelper.GetShortCultureCode(culture).ToUpper()}";
            }

            var site = siteName.ReplaceIfEmpty(_context.Site?.SiteName);

            object value = null;

            var type = typeof(T);

            if (type == typeof(string))
            {
                value = SettingsKeyInfoProvider.GetValue(keyName, site);
            }
            else if (type == typeof(bool))
            {
                value = SettingsKeyInfoProvider.GetBoolValue(keyName, site);
            }
            else if (type == typeof(int))
            {
                value = SettingsKeyInfoProvider.GetIntValue(keyName, site);
            }
            else if (type == typeof(decimal))
            {
                value = SettingsKeyInfoProvider.GetDecimalValue(keyName, site);
            }
            else if (type == typeof(double))
            {
                value = SettingsKeyInfoProvider.GetDoubleValue(keyName, site);
            }
            else if (type == typeof(Guid))
            {
                value = ValidationHelper.GetGuid(
                    SettingsKeyInfoProvider.GetValue(keyName, site),
                    Guid.Empty);
            }

            return value == null
                ? default(T)
                : (T)value;
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Localization;

namespace ECA.Caching.Extensions
{
    public static class BaseInfoExtensions
    {
        public static IList<string> GetResourceStringCacheKeys<T>(
            this IList<T> infoItems,
            string columnName)
            where T : BaseInfo
        {
            if (DataHelper.DataSourceIsEmpty(infoItems)
                || string.IsNullOrWhiteSpace(columnName))
            {
                return null;
            }

            return infoItems
                .Select(item =>
                {
                    var match = ResHelper.RegExLocalize.Match(item.GetStringValue(columnName, string.Empty));

                    if (!match.Success || (match.Groups.Count < 1))
                    {
                        return null;
                    }

                    var resStringName = match.Groups[match.Groups.Count - 1].Value;

                    return string.IsNullOrWhiteSpace(resStringName)
                        ? null
                        : $"{ResourceStringInfo.OBJECT_TYPE}|byname|{resStringName}";
                })
                .Where(key => key != null)
                .ToList();
        }
    }
}

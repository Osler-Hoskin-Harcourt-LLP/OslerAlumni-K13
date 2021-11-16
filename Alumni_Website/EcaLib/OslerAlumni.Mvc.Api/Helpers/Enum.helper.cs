using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Extensions;

namespace OslerAlumni.Mvc.Api.Helpers
{
    public static class EnumHelpers
    {
        public static List<SearchFilterOption> ToSearchFilters<T>(string culture)
        {

            var enumList = GetEnumList<T>();

            var output = enumList.Select(arg =>
            {
                var enumValue = arg as Enum;

                if (enumValue == null)
                {
                    return null;
                }

                return new SearchFilterOption()
                {
                    CodeName = enumValue.ToStringRepresentation(),
                    DisplayName = ResHelper.GetString(enumValue.GetDisplayName(), culture)
                };

            }).OrderBy(sf => sf?.DisplayName);

            return output.ToList();
        }

        private static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
        }
    }
}

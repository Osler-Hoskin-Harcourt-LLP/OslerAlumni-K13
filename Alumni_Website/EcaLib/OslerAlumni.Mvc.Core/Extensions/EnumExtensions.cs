using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using CMS.Helpers;
using OslerAlumni.Mvc.Core.Attributes.Html;

namespace OslerAlumni.Mvc.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            return enumValue?.GetType()?
                       .GetMember(enumValue?.ToString())?
                       .First()?
                       .GetCustomAttribute<T>();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return GetAttribute<DisplayAttribute>(enumValue)?.GetName() ?? string.Empty;
        }

        public static string GetLocalizedDisplayName(this Enum enumValue)
        {
            return ResHelper.GetString(GetDisplayName(enumValue));
        }

        public static Dictionary<string,object> GetCustomHtmlAttributes(this Enum enumValue)
        {
            return GetAttribute<CustomHtmlAttributesAttribute>(enumValue)?.Attributes ?? new Dictionary<string, object>();
        }
    }
}

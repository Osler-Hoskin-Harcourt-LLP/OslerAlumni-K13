using CMS.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OslerAlumni.Mvc.Core.Helpers
{
    public static class ModelMetaDataHelper
    {
        public static string GetModelLocalizedDisplayName(IHtmlHelper html, Type modelType, string propName)
        {
            return ResHelper.GetString(html.MetadataProvider
            .GetMetadataForProperties(modelType)
            .Where(prop => prop.PropertyName == propName)
            .FirstOrDefault()?.DisplayName);
        }

        public static string GetModelLocalizedDescription(IHtmlHelper html, Type modelType, string propName)
        {
            return ResHelper.GetString(html.MetadataProvider
            .GetMetadataForProperties(modelType)
            .Where(prop => prop.PropertyName == propName)
            .FirstOrDefault()?.DisplayName);
        }
    }
}

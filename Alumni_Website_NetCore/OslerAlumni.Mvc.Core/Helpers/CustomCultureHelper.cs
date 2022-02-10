using CMS.Helpers;
using CMS.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OslerAlumni.Mvc.Core.Helpers
{
    public static class CustomCultureHelper
    {
        /// <summary>
        /// This method extract culture code from url. It returns null if there is no culture found.
        /// </summary>
        /// <param name="url"></param>
        public static CultureInfo GetCultureCodeFromUrl(string url)
        {
            CultureInfo result = CultureInfoProvider.GetCultureInfo("en-CA");
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            url = url.TrimStart('/');
            string cultureCode = url.Split('/')[0];

            if (cultureCode.Length == 5)
            {
                result = CultureInfoProvider.GetCultureInfo(cultureCode);
            }

            if (cultureCode.Length == 2)
            {
                result = CultureInfoProvider.GetCultures().WhereEquals(nameof(CultureInfo.CultureAlias), cultureCode).FirstOrDefault();
            }

            

            return result;
        }
    }
}

using CMS.Helpers;
using CMS.Localization;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Services;
using System;

namespace OslerAlumni.Mvc.Extensions
{
    public static partial class OslerHtmlHelperExtensions
    {
        #region "Methods"

        public static string AppendReleaseNumber(
            this OslerExtensionPoint<IHtmlHelper> html,
            string url)
        {
            var pageUrlService =
                CMS.Core.Service.Resolve<IPageUrlService>();

            var configurationService =
                CMS.Core.Service.Resolve<IConfigurationService>();

            var releaseVersion = configurationService
                .GetWebConfigSetting<string>(
                    GlobalConstants.Config.ReleaseNumber);

            if (!string.IsNullOrWhiteSpace(releaseVersion))
            {
                url = pageUrlService.GetUrl(
                    url,
                    // This should generate the query string of "v=x.y.z"
                    new
                    {
                        v = releaseVersion
                    });
            }
            
            return ResolveUrl(html, url);
        }

        public static string GetPageUrl(
            this OslerExtensionPoint<IHtmlHelper> html,
            StandalonePageType standalonePageType,
            ContextConfig context = null,
            object queryStrObj = null)
        {
            var cultureName = LocalizationContext.CurrentCulture.CultureCode ;

            var pageUrlService =
                CMS.Core.Service.Resolve<IPageUrlService>();

            string url;

            if (!pageUrlService.TryGetPageMainUrl(
                    standalonePageType,
                    cultureName,
                    out url))
            {
                return null;
            }

            return pageUrlService.GetUrl(
                url,
                queryStrObj);
        }

        public static string GetPageUrl(
            this OslerExtensionPoint<IHtmlHelper> html,
            Guid nodeGuid,
            ContextConfig context = null,
            object queryStrObj = null)
        {
            var cultureName = html
                .GetCurrentCultureName(ref context);

            var pageUrlService =
                CMS.Core.Service.Resolve<IPageUrlService>();

            string url;

            if (!pageUrlService.TryGetPageMainUrl(
                    nodeGuid,
                    cultureName,
                    out url))
            {
                return null;
            }

            return pageUrlService.GetUrl(
                url,
                queryStrObj);
        }

        public static string ResolveUrl(
            this OslerExtensionPoint<IHtmlHelper> html,
            string url)
        {
            return URLHelper.ResolveUrl(
                url);
        }

        #endregion
    }
}

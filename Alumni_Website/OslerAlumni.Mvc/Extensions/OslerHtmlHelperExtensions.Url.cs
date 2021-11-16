using System;
using System.Web.Mvc;
using CMS.Helpers;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Services;

namespace OslerAlumni.Mvc.Extensions
{
    public static partial class OslerHtmlHelperExtensions
    {
        #region "Methods"

        public static string AppendReleaseNumber(
            this OslerExtensionPoint<HtmlHelper> html,
            string url)
        {
            var pageUrlService =
                DIResolver.GetService<IPageUrlService>();

            var configurationService =
                DIResolver.GetService<IConfigurationService>();

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
            this OslerExtensionPoint<HtmlHelper> html,
            StandalonePageType standalonePageType,
            ContextConfig context = null,
            object queryStrObj = null)
        {
            var cultureName = html
                .GetCurrentCultureName(ref context);

            var pageUrlService =
                DIResolver.GetService<IPageUrlService>();

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
            this OslerExtensionPoint<HtmlHelper> html,
            Guid nodeGuid,
            ContextConfig context = null,
            object queryStrObj = null)
        {
            var cultureName = html
                .GetCurrentCultureName(ref context);

            var pageUrlService =
                DIResolver.GetService<IPageUrlService>();

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
            this OslerExtensionPoint<HtmlHelper> html,
            string url)
        {
            return URLHelper.ResolveUrl(
                url);
        }

        #endregion
    }
}
using System;
using CMS.DocumentEngine;
using CMS.SiteProvider;
using ECA.Core.Services;
using ECA.PageURL.Definitions;
using ECA.PageURL.Kentico.Models;

namespace ECA.PageURL.Services
{
    public interface IPageUrlService
        : IService
    {
        string ConvertToUrl(
            CustomTable_PageURLItem urlItem);

        bool DeleteUrls(
            TreeNode page);

        /// <summary>
        /// Generates a valid URL path for the provided page (based on its DocumentNamePath)
        /// and recursively checks if the URL path is in use by any other page in the same culture.
        /// Returns the URL path with the next available increment appended.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="urlItem">
        /// Output parameter that will contain the existing URL path record,
        /// if the URL path is in use by the specified page.
        /// </param>
        /// <returns></returns>
        string GetAvailableUrlPath(
            TreeNode page,
            out CustomTable_PageURLItem urlItem);

        string GetAvailableUrlPath(
            CustomTable_PageURLItem urlItem,
            string siteName);
        
        /// <summary>
        /// Returns the provided URL with the query string
        /// populated from the provided object properties.
        /// </summary>
        /// <param name="url">Original URL.</param>
        /// <param name="queryStrObj">
        /// Anonymous object, whose property values should be turned into a query string.
        /// </param>
        /// <returns></returns>
        string GetUrl(
            string url,
            object queryStrObj);


        /// <summary>
        /// Generates a valid URL path for the provided page based on its DocumentNamePath.
        /// Removes forbidden URL characters.
        /// Does NOT include culture prefix.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        string GetUrlPath(
            TreeNode page);

        string GetUrlPath(
            string namePath,
            string siteName);

        /// <summary>
        /// Checks if the page allows for being navigated to directly
        /// and if it should have a URL as a result.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        bool IsNavigable(
            TreeNode page);

        bool IsRootUrl(
            string url);

        string ResolveToAbsolute(
            string url,
            SiteInfo site);

        /// <inheritdoc cref="TryGetPageMainUrl(Guid, string, out string, string)"/>
        bool TryGetPageMainUrl(
            TreeNode page,
            out string url);

        /// <summary>
        /// Attempts to get the main page URL
        /// and apply culture-based localization to it.
        /// </summary>
        /// <param name="nodeGuid"></param>
        /// <param name="cultureName"></param>
        /// <param name="siteName"></param>
        /// <param name="url"></param>
        /// <returns>
        /// Main URL of the page, including the culture prefix,
        /// if applicable.
        /// </returns>
        bool TryGetPageMainUrl(
            Guid nodeGuid,
            string cultureName,
            out string url,
            string siteName = null);

        bool TryGetPageMainUrl(
            StandalonePageType standalonePageType,
            string cultureName,
            out string url,
            string siteName = null);


    }
}

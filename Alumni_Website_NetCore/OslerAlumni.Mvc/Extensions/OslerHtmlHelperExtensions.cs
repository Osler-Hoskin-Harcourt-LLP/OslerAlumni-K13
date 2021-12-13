using CMS.Localization;
using ECA.Core.Extensions;
using ECA.Core.Models;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Mvc.Extensions
{
    /// <summary>
    /// This class uses a very popular builder pattern approach that have been used in 
    /// many production ready 3rd party software tools such KendoUI and Kentico.
    /// It has been modified to fit our needs
    /// TODO: Eventually all html extensions should use this approach.
    /// </summary>
    public static partial class OslerHtmlHelperExtensions
    {
        #region "Private fields"

        private static readonly object _lockDIResolver = new object();
        private static readonly object _lockExtensionPoint = new object();

        private static OslerExtensionPoint<IHtmlHelper> _extensionPoint;

        #endregion

        #region "Properties"


        #endregion

        #region "Extension points"

        /// <summary>
        /// Returns an object that provides methods to render HTML fragments.
        /// </summary>
        /// <param name="target">The instance of the <see cref="T:System.Web.Mvc.HtmlHelper" /> class.</param>
        /// <returns>The object that provides methods to render HTML fragments.</returns>
        public static OslerExtensionPoint<IHtmlHelper> Osler(
            this IHtmlHelper target)
        {
            lock (_lockExtensionPoint)
            {
                if (_extensionPoint == null || _extensionPoint.Target != target)
                {
                    _extensionPoint = new OslerExtensionPoint<IHtmlHelper>(target);
                }

                return _extensionPoint;
            }
        }

        /// <summary>
        /// Returns an object that provides methods to render HTML fragments.
        /// </summary>
        /// <param name="target">The instance of the <see cref="T:System.Web.Mvc.HtmlHelper" /> class.</param>
        /// <returns>The object that provides methods to render HTML fragments.</returns>
        public static OslerExtensionPoint<IHtmlHelper<TModel>> OslerForModel<TModel>(
            this IHtmlHelper<TModel> target)
        {
            return new OslerExtensionPoint<IHtmlHelper<TModel>>(target);
        }

        #endregion

        #region "Methods"

        public static string GetCurrentLanguage(
            this OslerExtensionPoint<IHtmlHelper> html,
            ContextConfig context = null)
        {
            var cultureName = html
                .GetCurrentCultureName(ref context);

            string lang;

            if (GlobalConstants.Cultures.AllowedCultureCodes
                .TryGetKeyByOrdinalValue(
                    cultureName,
                    out lang))
            {
                return lang;
            }

            return null;
        }

        public static string GetCurrentCultureName(
            this OslerExtensionPoint<IHtmlHelper> html,
            ContextConfig context = null)
        {
            return LocalizationContext.CurrentCulture.CultureCode;
        }

        private static string GetCurrentCultureName(
            this OslerExtensionPoint<IHtmlHelper> html,
            ref ContextConfig context)
        {
            PopulateContextIfEmpty(html, ref context);

            return (LocalizationContext.CurrentCulture.CultureCode)
                .ReplaceIfEmpty(GlobalConstants.Cultures.Default);
        }


        public static bool IsPreviewMode(
            this OslerExtensionPoint<IHtmlHelper> html,
            ContextConfig context = null)
        {
            return html
                .IsPreviewMode(ref context);
        }

        private static bool IsPreviewMode(
            this OslerExtensionPoint<IHtmlHelper> html,
            ref ContextConfig context)
        {
            PopulateContextIfEmpty(html, ref context);

            return CMS.Core.Service.Resolve<IHttpContextAccessor>()?.HttpContext?.Kentico().Preview().Enabled ?? false;
        }

        private static void PopulateContextIfEmpty(
            OslerExtensionPoint<IHtmlHelper> html,
            ref ContextConfig context)
        {
            if (context == null)
            {
                context = html.Target.ViewBag.Context;
            }
        }

        #endregion
    }
}

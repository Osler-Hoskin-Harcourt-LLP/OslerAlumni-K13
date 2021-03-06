using System.Web.Mvc;
using ECA.Core.Extensions;
using ECA.Core.Models;
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

        private static IDependencyResolver _diResolver;
        private static OslerExtensionPoint<HtmlHelper> _extensionPoint;

        #endregion

        #region "Properties"

        public static IDependencyResolver DIResolver
        {
            get
            {
                lock (_lockDIResolver)
                {
                    return _diResolver;
                }
            }
            set
            {
                lock (_lockDIResolver)
                {
                    _diResolver = value;
                }
            }
        }

        #endregion

        #region "Extension points"

        /// <summary>
        /// Returns an object that provides methods to render HTML fragments.
        /// </summary>
        /// <param name="target">The instance of the <see cref="T:System.Web.Mvc.HtmlHelper" /> class.</param>
        /// <returns>The object that provides methods to render HTML fragments.</returns>
        public static OslerExtensionPoint<HtmlHelper> Osler(
            this HtmlHelper target)
        {
            lock (_lockExtensionPoint)
            {
                if (_extensionPoint == null || _extensionPoint.Target != target)
                {
                    _extensionPoint = new OslerExtensionPoint<HtmlHelper>(target);
                }

                return _extensionPoint;
            }
        }

        /// <summary>
        /// Returns an object that provides methods to render HTML fragments.
        /// </summary>
        /// <param name="target">The instance of the <see cref="T:System.Web.Mvc.HtmlHelper" /> class.</param>
        /// <returns>The object that provides methods to render HTML fragments.</returns>
        public static OslerExtensionPoint<HtmlHelper<TModel>> OslerForModel<TModel>(
            this HtmlHelper<TModel> target)
        {
            return new OslerExtensionPoint<HtmlHelper<TModel>>(target);
        }

        #endregion

        #region "Methods"

        public static string GetCurrentLanguage(
            this OslerExtensionPoint<HtmlHelper> html,
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
            this OslerExtensionPoint<HtmlHelper> html,
            ContextConfig context = null)
        {
            return html
                .GetCurrentCultureName(ref context);
        }

        private static string GetCurrentCultureName(
            this OslerExtensionPoint<HtmlHelper> html,
            ref ContextConfig context)
        {
            PopulateContextIfEmpty(html, ref context);

            return (context?.CultureName)
                .ReplaceIfEmpty(GlobalConstants.Cultures.Default);
        }


        public static bool IsPreviewMode(
            this OslerExtensionPoint<HtmlHelper> html,
            ContextConfig context = null)
        {
            return html
                .IsPreviewMode(ref context);
        }

        private static bool IsPreviewMode(
            this OslerExtensionPoint<HtmlHelper> html,
            ref ContextConfig context)
        {
            PopulateContextIfEmpty(html, ref context);

            return (context?.IsPreviewMode) ?? false;
        }

        private static void PopulateContextIfEmpty(
            OslerExtensionPoint<HtmlHelper> html,
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

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using CMS.MediaLibrary;

using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace OslerAlumni.Mvc.Extensions.OslerControls.Image
{
    public static class OslerImageExtensions
    {
        /// <summary>
        /// Useful for rendering an Image.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="mediaFile"></param>
        /// <returns></returns>
        public static OslerImageBuilder Image(
            this OslerExtensionPoint<HtmlHelper> instance,
            MediaFileInfo mediaFile)
        {
            return new OslerImageBuilder(instance.Target, mediaFile);
        }

        public static OslerImageBuilder Image(
            this OslerExtensionPoint<HtmlHelper> instance,
            string relativePath)
        {
            return new OslerImageBuilder(instance.Target, relativePath);
        }


        public class OslerImageBuilder
        {
            #region "Constants"

            private const string SvgExtension = ".svg";
            private const string ExtensionParameter = "ext";

            #endregion

            #region "Private fields"

            private readonly HtmlHelper _html;
            private readonly string _relativePath;
            private readonly MediaFileInfo _mediaFile;
            private readonly TagBuilder _image = new TagBuilder("img");
            private SizeConstraint _sizeConstraint = SizeConstraint.Empty;

            #endregion

            public OslerImageBuilder(HtmlHelper html, MediaFileInfo mediaFile)
            {
                _html = html;
                _mediaFile = mediaFile;
                WithAltText(_mediaFile.FileTitle);
            }

            public OslerImageBuilder(HtmlHelper html, string relativePath)
            {
                _html = html;
                _relativePath = relativePath;
            }

            #region "Methods"

            /// <summary>
            /// Add Css Class to the image
            /// </summary>
            /// <param name="cssClass"></param>
            /// <returns></returns>
            public OslerImageBuilder AddClass(string cssClass)
            {
                _image.AddCssClass(cssClass);
                return this;
            }

            /// <summary>
            /// Add Image Alt Text
            /// </summary>
            /// <param name="altText"></param>
            /// <returns></returns>
            public OslerImageBuilder WithAltText(string altText)
            {
                _image.MergeAttribute("alt", altText, true);
                return this;
            }

            /// <summary>
            /// Add Image Size Constraint. Does not work for SVGs.
            /// </summary>
            /// <param name="sizeConstraint"></param>
            /// <returns></returns>
            public OslerImageBuilder WithSizeConstraint(
                SizeConstraint sizeConstraint)
            {
                _sizeConstraint = sizeConstraint;
                return this;

            }

            /// <summary>
            /// Add attributes to the anchor tag
            /// </summary>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public OslerImageBuilder WithAttributes(object attributes)
            {
                var htmlAttributesDictionary = (IDictionary<string, object>)
                    HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);

                _image.MergeAttributes(htmlAttributesDictionary);

                return this;
            }

            /// <summary>
            /// Render the Tag
            /// </summary>
            /// <returns></returns>
            public MvcHtmlString ToHtmlString()
            {
                var urlHelper = new UrlHelper(_html.ViewContext.RequestContext);

                var url = _relativePath;

                if (_mediaFile != null)
                {
                    var imgUrl = MediaLibraryHelper.GetPermanentUrl(_mediaFile); // TODO: [DF] this line was updated after the K12 upgrade. Test to ensure this works

                    url = urlHelper.Kentico().ImageUrl(
                        imgUrl,
                        _sizeConstraint);

                    if (IsSvg(_mediaFile))
                    {
                        url = HandleSvg(url);
                    }
                }
                else if (IsSvg(url))
                {
                    url = HandleSvg(url);
                }

                _image.Attributes.Add("src", urlHelper.Kentico().ImageUrl(url,_sizeConstraint));

                return MvcHtmlString.Create(_image.ToString());
            }

            #endregion

            #region "Helper methods"

            private bool IsSvg(MediaFileInfo mediaFile)
            {
                return string.Equals(
                    mediaFile.FileExtension,
                    SvgExtension,
                    StringComparison.OrdinalIgnoreCase);
            }

            private bool IsSvg(string relativePath)
            {
                var uriParts = (relativePath ?? string.Empty).Split('?');

                var url = uriParts[0];
                var queryParts = HttpUtility.ParseQueryString(
                    (uriParts.Length > 1) ? uriParts[1] : string.Empty);

                return url.ToLower().EndsWith(SvgExtension)
                    || (queryParts[ExtensionParameter]?.ToLower().Equals(SvgExtension) ?? false);
            }

            private string HandleSvg(
                string url)
            {
                url = url.Contains("?")
                    ? $"{url}&"
                    : $"{url}?";

                url = $"{url}randomString={SvgExtension}"; //Need for SVG Injector

                AddClass("c-svg");

                return url;
            }

            #endregion
        }
    }
}


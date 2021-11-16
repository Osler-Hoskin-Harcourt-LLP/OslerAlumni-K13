using System.Collections.Generic;
using System.Web.Mvc;
using CMS.Helpers;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Extensions.OslerControls.Button
{
    public static class OslerFormButtonExtensions
    {
        /// <summary>
        /// Useful for generating a generic submit button on Forms.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static OslerSubmitButtonBuilder FormSubmitButton(
            this OslerExtensionPoint<HtmlHelper> instance)
        {
            return new OslerSubmitButtonBuilder(instance.Target);
        }

        public class OslerSubmitButtonBuilder
        {
            private readonly HtmlHelper _html;

            private TagBuilder _button;

            private TagBuilder _divContainer;

            public OslerSubmitButtonBuilder(HtmlHelper html)
            {
                _html = html;

                _divContainer = new TagBuilder("div");
                _divContainer.AddCssClass("c-form-field");

                _button = new TagBuilder("button");
                _button.Attributes.Add("type", "submit");

                _button.AddCssClass("c-form-submit");
                _button.AddCssClass("c-button");
                _button.AddCssClass("c-button-primary");

                _button.InnerHtml = ResHelper.GetString(Constants.ResourceStrings.Submit);
            }

            /// <summary>
            /// Add a Resource string
            /// </summary>
            /// <param name="name">Kentico Resource string ID</param>
            /// <returns></returns>
            public OslerSubmitButtonBuilder WithText(string name)
            {
                _button.InnerHtml = ResHelper.GetString(name);
                return this;
            }

            /// <summary>
            /// Add attributes to the button label
            /// </summary>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public OslerSubmitButtonBuilder WithAttributes(object attributes)
            {
                var htmlAttributesDictionary = (IDictionary<string, object>)
                    HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);

                _button.MergeAttributes(htmlAttributesDictionary);

                return this;
            }

            /// <summary>
            /// Add Css Class to the button
            /// </summary>
            /// <param name="cssClass"></param>
            /// <returns></returns>
            public OslerSubmitButtonBuilder AddClass(string cssClass)
            {
                _button.AddCssClass(cssClass);
                return this;
            }

            public MvcHtmlString ToHtmlString()
            {
                _divContainer.InnerHtml = _button.ToString();
                return MvcHtmlString.Create(_divContainer.ToString());
            }
        }
    }
}


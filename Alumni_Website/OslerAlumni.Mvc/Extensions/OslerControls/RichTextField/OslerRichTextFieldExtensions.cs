using System.Web.Mvc;
using Kentico.Web.Mvc;
using OslerAlumni.Mvc.Extensions.OslerControls.RichTextField.Models.Helpers;

namespace OslerAlumni.Mvc.Extensions.OslerControls.RichTextField
{
    public static class OslerRichTextFieldExtensions
    {
        /// <summary>
        /// Useful for generating rich text field content from Kentico.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="content"></param>
        /// <param name="resolveInlineWidgets"></param>
        /// <returns></returns>
        public static OslerRichTextFieldBuilder<TModel> RichTextFieldFor<TModel>(
            this OslerExtensionPoint<HtmlHelper<TModel>> instance, string content, bool resolveInlineWidgets = true)
        {
            return new OslerRichTextFieldBuilder<TModel>(instance.Target, content, resolveInlineWidgets);
        }

        public class OslerRichTextFieldBuilder<TModel>
        {
            private readonly HtmlHelper<TModel> _html;
            private readonly bool _resolveInlineWidgets;
            private string _content;


            private readonly TagBuilder _divContainer;

            public OslerRichTextFieldBuilder(HtmlHelper<TModel> html, string content, bool resolveInlineWidgets = true)
            {
                _html = html;
                _resolveInlineWidgets = resolveInlineWidgets;
                _content = _html.Kentico().ResolveUrls(content).ToHtmlString();

                _divContainer = new TagBuilder("div");
                _divContainer.AddCssClass("s-richtext");
            }


            /// <summary>
            /// Add Css Class to the button
            /// </summary>
            /// <param name="cssClass"></param>
            /// <returns></returns>
            public OslerRichTextFieldBuilder<TModel> AddClass(string cssClass)
            {
                _divContainer.AddCssClass(cssClass);
                return this;
            }

            public MvcHtmlString ToHtmlString()
            {
                if (_resolveInlineWidgets)
                {
                    var inlineWidgetResolver =
                        OslerHtmlHelperExtensions.DIResolver.GetService<InlineWidgetResolver>();

                    _content = inlineWidgetResolver.ToResolvedWidgetContent(_content, _html);
                }

                _divContainer.InnerHtml = _content;

                return MvcHtmlString.Create(_divContainer.ToString());

            }

        }
    }
}


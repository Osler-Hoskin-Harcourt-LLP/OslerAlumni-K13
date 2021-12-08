using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
            this OslerExtensionPoint<IHtmlHelper<TModel>> instance, string content, bool resolveInlineWidgets = true)
        {
            return new OslerRichTextFieldBuilder<TModel>(instance.Target, content, resolveInlineWidgets);
        }

        public class OslerRichTextFieldBuilder<TModel>
        {
            private readonly IHtmlHelper<TModel> _html;
            private readonly bool _resolveInlineWidgets;
            private string _content;


            private readonly TagBuilder _divContainer;

            public OslerRichTextFieldBuilder(IHtmlHelper<TModel> html, string content, bool resolveInlineWidgets = true)
            {
                _html = html;
                _resolveInlineWidgets = resolveInlineWidgets;
                _content = html.Kentico().ResolveUrls(content).ToString();

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

            public IHtmlContent ToHtmlString()
            {
                if (_resolveInlineWidgets)
                {
                    var inlineWidgetResolver =
                        CMS.Core.Service.Resolve<InlineWidgetResolver>();

                    _content = inlineWidgetResolver.ToResolvedWidgetContent(_content, _html);
                }

                _divContainer.InnerHtml.AppendHtml(_content);

                return _divContainer;

            }

        }
    }
}


using CMS.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Extensions.OslerControls.CheckBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OslerAlumni.Mvc.Extensions
{
    public static partial class OslerHtmlHelperExtensions
    {
        #region "Constants"

        public const string MaxFileSizeAttributeName = "data-max-file-size";
        public const string AllowedFileTypesAttributeName = "data-allowed-file-types";

        #endregion

        /// <summary>
        /// Useful to generate html markup for resusable form fields. Contains wrapper markup.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the displayed properties.</param>
        /// <param name="labelMarkup">If provided, will use this markup, otherwise will output a generic LabelFor()</param>
        /// <param name="editorMarkup">If provided, will use this markup, otherwise will output a generic EditorFor()</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample Usage:
        /// @Html.StyledEditorFor(model => model.FirstName)
        /// @Html.StyledEditorFor(model => model.Message, Html.TextAreaFor(model => model.Message))
        /// </remarks>
        public static IHtmlContent StyledEditorFor<TModel, TValue>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            HtmlString labelMarkup = null,
            IHtmlContent editorMarkup = null,
            object htmlAttributes = null)
        {
            return GetStyledEditor(
                html,
                expression,
                labelMarkup,
                editorMarkup,
                htmlAttributes);
        }


        /// <summary>
        /// Useful to generate html markup for generic check box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the displayed properties.</param>
        /// <param name="labelMarkup">If provided, will use this markup, otherwise will output a generic LabelFor()</param>
        /// <param name="editorMarkup">If provided, will use this markup, otherwise will output a generic EditorFor()</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample Usage:
        /// @Html.StyledEditorFor(model => model.FirstName)
        /// @Html.StyledEditorFor(model => model.Message, Html.TextAreaFor(model => model.Message))
        /// </remarks>
        public static IHtmlContent StyledCheckboxFor<TModel>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, bool>> expression,
            object htmlAttributes = null)
        {
            return GetStyledEditor(
                html,
                expression,
                html.OslerForModel().CheckBoxFor(expression).ToHtmlString(),  /*The order is switched on purpose, since label appear after for checkboxes*/
                html.LabelFor(expression),
                htmlAttributes);
        }


        public static IHtmlContent StyledFileUploaderFor<TModel, TValue>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            IHtmlContent labelMarkup = null,
            object htmlAttributes = null,
            string jsValidationError = null)
        {
            int? maxFileSize = null;
            IList<string> allowedExtensions = null;

            var maxFileSizeValidationAttribute =
                GetMetadataAttribute<MaxFileSizeValidationAttribute, TModel, TValue>(expression);

            if (maxFileSizeValidationAttribute != null)
            {
                maxFileSize = maxFileSizeValidationAttribute.MaxFileSizeInBytes;
            }

            var fileTypeValidationAttribute =
                GetMetadataAttribute<FileTypeValidationAttribute, TModel, TValue>(expression);

            if (fileTypeValidationAttribute != null)
            {
                allowedExtensions = fileTypeValidationAttribute.AllowedExtensionList;
            }

            Func<IDictionary<string, object>, IDictionary<string, object>> extendHtmlAttributes =
                (htmlAttrs) =>
                {
                    if (maxFileSize.HasValue)
                    {
                        htmlAttrs.Add(MaxFileSizeAttributeName, maxFileSize);
                    }

                    if ((allowedExtensions != null) && (allowedExtensions.Count > 0))
                    {
                        htmlAttrs.Add(
                            AllowedFileTypesAttributeName,
                            string.Join(
                                ",",
                                allowedExtensions
                                    .Select(GetJSFileTypesFromExtension)));
                    }

                    return htmlAttrs;
                };

            var viewData = new ViewDataDictionary(html.MetadataProvider, html.ViewContext.ModelState);



            if (!string.IsNullOrWhiteSpace(jsValidationError))
            {
                viewData.Add("JsValidationError", jsValidationError);
            }

            return GetStyledEditor(
                html,
                expression,
                labelMarkup,
                viewName: "_StyledUploadEditorFor",
                htmlAttributes: htmlAttributes,
                extendHtmlAttributes: extendHtmlAttributes,
                viewData: viewData);
        }

        public static IHtmlContent StyledRadioButtonListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TEnum>> expression)
            where TEnum : struct, IConvertible
        {
            //Cannot constraint to Enum for some reason. This is the most I can do
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            var elementId = $"{html.IdFor(expression)}";

            var emumValues = Enum.GetValues(expression.ReturnType);

            string radioButtonList = "";

            foreach (var enumValue in emumValues)
            {
                var customHtmlAttributes = ((Enum)enumValue).GetCustomHtmlAttributes();

                var inputId = $"{elementId}-{enumValue}";

                customHtmlAttributes.Add("id", inputId);

                radioButtonList += $@"
                    <div>
                        {html.RadioButtonFor(expression, enumValue, customHtmlAttributes)}
                        <label for={inputId}>{((Enum)enumValue).GetLocalizedDisplayName()}</label>
                    </div>";
            }

            return html.Partial("_StyledRadioButtonListFor", new ViewDataDictionary(html.MetadataProvider, html.ViewContext.ModelState)
            {
                {"DivId", $"{elementId}_{Guid.NewGuid()}" },
                {"ElementId",elementId },
                {"Legend", html.LocalizedDisplayNameFor(expression)},
                {"RadioButtonList", radioButtonList}
            });
        }

        public static IHtmlContent StyledEnumDropDownListFor<TModel, TValue>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, Enum options)
        {
            var elementId = $"{html.IdFor(expression)}";

            var emumValues = Enum.GetValues(options.GetType());

            var optionList = new List<SelectListItem>();

            foreach (var enumValue in emumValues)
            {
                optionList.Add(new SelectListItem()
                {
                    Value = ((Enum)enumValue).ToStringRepresentation(),
                    Text = ((Enum)enumValue).GetLocalizedDisplayName()
                });
            }

            return html.Partial("_StyledEnumDropDownListFor", new ViewDataDictionary(html.MetadataProvider, html.ViewContext.ModelState)
            {
                {"DivId", $"{elementId}_{Guid.NewGuid()}" },
                {"Label", html.LabelFor(expression).ToString()},
                {"DropDownList", html.DropDownListFor(expression, optionList).ToString()}
            });
        }

        public static string LocalizedDescriptionFor<TModel, TValue>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var descriptionAttribute =
                GetMetadataAttribute<DescriptionAttribute, TModel, TValue>(expression);

            return ResHelper.GetString(descriptionAttribute?.Description ?? string.Empty);
        }

        public static string LocalizedDisplayNameFor<TModel, TValue>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var displayAttribute =
                GetMetadataAttribute<DisplayAttribute, TModel, TValue>(expression);

            return ResHelper.GetString(displayAttribute?.GetName() ?? string.Empty);
        }

        public static IHtmlContent GoogleCaptchaFor<TModel, TValue>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return html.Partial("_GoogleCaptchaFor");
        }

        /// <summary>
        /// Useful for generating a generic Forms Error Summary.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name">Kentico Resource string ID</param>
        /// <returns></returns>
        public static IHtmlContent FormErrorSummary(
            this IHtmlHelper html,
            string name = Constants.ResourceStrings.Form.GlobalError)
        {
            return html.Partial("_FormErrorSummary", ResHelper.GetString(name));
        }

        #region "Helper methods"

        private static string GetFormFieldCssClassByType(Type type)
        {
            if (type == typeof(string))
            {
                return "c-form-field-text";
            }

            if (type == typeof(bool))
            {
                return "c-form-field-checkbox";
            }

            if (type == typeof(IFormFile))
            {
                return "c-form-field-file";
            }

            return string.Empty;
        }

        private static T GetMetadataAttribute<T, TModel, TValue>(
            Expression<Func<TModel, TValue>> expression)
        {
            Type type = typeof(TModel);

            MemberExpression memberExpression = (MemberExpression)expression.Body;

            string propertyName =
                ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : string.Empty);

            var attribute = (T)(type.GetProperty(propertyName)
                ?.GetCustomAttributes(typeof(T), true).SingleOrDefault());

            return attribute;
        }

        private static IHtmlContent GetStyledEditor<TModel, TValue>(
            IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            IHtmlContent labelMarkup = null,
            IHtmlContent editorMarkup = null,
            object htmlAttributes = null,
            Func<IDictionary<string, object>, IDictionary<string, object>> extendHtmlAttributes = null,
            ViewDataDictionary viewData = null,
            string viewName = "_StyledEditorFor")
        {
            var divContainer = new TagBuilder("div");

            divContainer.Attributes.Add("id", $"{html.IdFor(expression)}-{Guid.NewGuid()}");

            if (htmlAttributes != null)
            {
                var htmlAttributesDictionary = (IDictionary<string, object>)
                    HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                if (extendHtmlAttributes != null)
                {
                    htmlAttributesDictionary =
                        extendHtmlAttributes(htmlAttributesDictionary);
                }

                divContainer.MergeAttributes(htmlAttributesDictionary);
            }

            //Note default classes must be added in later otherwise they will be wiped out
            divContainer.AddCssClass(GetFormFieldCssClassByType(typeof(TValue)));
            divContainer.AddCssClass("c-form-field");

            var label = labelMarkup?.ToString() ?? html.LabelFor(expression).ToString();
            var editor = editorMarkup?.ToString() ?? html.EditorFor(expression).ToString();

            if (viewData == null)
            {
                viewData = new ViewDataDictionary(html.MetadataProvider, html.ViewContext.ModelState);
            }

            viewData.Add("Label", label);
            viewData.Add("Editor", editor);
            viewData.Add("ExplanationText", html.DisplayTextFor(expression));

            divContainer.InnerHtml.AppendHtml(
                html
                .Partial(viewName, viewData)
                .ToString());

            return divContainer;
        }

        private static string GetJSFileTypesFromExtension(
            string extension)
        {
            extension = extension?.Trim().TrimStart('.').Trim();

            if (string.IsNullOrEmpty(extension))
            {
                return null;
            }

            switch (extension)
            {
                case "doc":
                    return "application/msword";

                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                case "pdf":
                    return "application/pdf";

                case "rtf":
                    return "application/rtf";

                case "txt":
                    return "text/plain";

                case "jpg":
                    return "image/jpeg";

                case "png":
                    return "image/png";
            }

            return null;
        }

        #endregion
    }
}

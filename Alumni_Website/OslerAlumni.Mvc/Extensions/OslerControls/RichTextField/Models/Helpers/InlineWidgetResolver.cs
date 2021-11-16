using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using ECA.Core.Extensions;
using ECA.Core.Repositories;
using OslerAlumni.Core.Definitions;
using Microsoft.Ajax.Utilities;
using OslerAlumni.Mvc.Extensions.OslerControls.RichTextField.Models.InlineWidgetModels;

namespace OslerAlumni.Mvc.Extensions.OslerControls.RichTextField.Models.Helpers
{
    public class InlineWidgetResolver
    {

        #region Constants

        private const string WidgetTagName = "widget";

        #endregion

        private readonly IEventLogRepository _eventLogRepository;

        public InlineWidgetResolver(IEventLogRepository eventLogRepository)
        {
            _eventLogRepository = eventLogRepository;
        }

        public string ToResolvedWidgetContent<TModel>(string content, HtmlHelper<TModel> htmlHelperObject)
        {
            //Doing a contains check first since its faster than a regular expression
            if (!content.Contains("{^" + WidgetTagName))
            {
                return content;
            }

            //change content into xml markup
            content = ReplaceWidgetsIntoTags(content);

            //Can now parse as xml
            var htmlInput = new HtmlParser().ParseDocument(content);

            var widgets = htmlInput.GetElementsByTagName(WidgetTagName);

            //take the widget fields and put them as xml attributes
            widgets.ForEach((w) =>
            {
                ParseAndSetWidgetAttributes(ref w);
                w.InnerHtml = String.Empty;
            });

            //Now replace the xml with the html for the display template.
            htmlInput.Body.Children.ForEach((block) => { ReplaceWidgetsWithMarkup(ref block, htmlHelperObject); });

            return htmlInput.Body.OuterHtml;
        }

        /// <summary>
        /// Replace strings containing {^widget(*)}
        /// to <widget>(*)</widget>
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string ReplaceWidgetsIntoTags(string content)
        {
            //Note this will only resolve top level widgets, widgets within widgets are url encoded
            //So they won't get resolved here.
            var pattern = $@"\{{\^{WidgetTagName}\|(.*)\^}}";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.Replace(content, $"<{WidgetTagName}>$1</{WidgetTagName}>");
        }

        /// <summary>
        /// Replaces <widget>(name)foobar</widget> to <widget name="foobar"></widget>
        /// </summary>
        /// <param name="widget"></param>
        private void ParseAndSetWidgetAttributes(ref IElement widget)
        {
            var properties = widget.InnerHtml.Split('|');

            foreach (var property in properties)
            {
                Regex regex = new Regex(@"\((.*?)\)(.*)", RegexOptions.IgnoreCase);
                Match match = regex.Match(property);

                if (match.Success)
                {
                    var attributeName = match.Groups[1].Value;
                    var attributeValue = match.Groups[2].Value;
                    widget.SetAttribute(attributeName, attributeValue);
                }
            }
        }

        /// <summary>
        /// Replaces <widget name="youtube"></widget> to corresponding strongly typed display template
        /// </summary>
        /// <param name="node"></param>
        /// <param name="htmlHelperObject"></param>
        private void ReplaceWidgetsWithMarkup<TModel>(ref IElement node, HtmlHelper<TModel> htmlHelperObject)
        {
            node.Children.ForEach(child => ReplaceWidgetsWithMarkup(ref child, htmlHelperObject));

            if (String.Equals(node.TagName, WidgetTagName, StringComparison.OrdinalIgnoreCase))
            {
                var viewModel = ToWidgetViewModel(node);

                if (viewModel != null)
                {
                    node.OuterHtml = htmlHelperObject.DisplayFor(vm => viewModel).ToString();
                }
                else
                {
                    //Remove unrecognized inline widgets
                    node.Remove();
                }
            }
        }

        /// <summary>
        /// Replaces <widget name="youtube"></widget> to corresponding inline-display template
        /// </summary>
        /// <param name="node"></param>
        private object ToWidgetViewModel(IElement widget)
        {
            var inlineWidgetName = widget.GetAttribute(nameof(IInlineWidetModel.Name));

            //Replace SitePrefix
            inlineWidgetName = inlineWidgetName?.Replace($"{GlobalConstants.SiteCodeName}.", string.Empty);

            var inlineWidgetModelType = typeof(IInlineWidetModel);

            var type = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t =>
                    !t.IsInterface &&
                    inlineWidgetModelType.IsAssignableFrom(t)
                    && t.Name == inlineWidgetName
                );

            if (type == null)
            {
                return null;
            }

            try
            {
                object instance = Activator.CreateInstance(type);

                PropertyInfo[] prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var widgetAttribute in widget.Attributes)
                {
                    try
                    {
                        var property = prop.FirstOrDefault(p =>
                            String.Equals(p.Name, widgetAttribute.Name, StringComparison.OrdinalIgnoreCase));

                        if (property != null && !string.IsNullOrWhiteSpace(widgetAttribute.Value))
                        {
                            var decodedValue = HttpUtility.UrlDecode(widgetAttribute.Value);

                            var propValue = decodedValue?
                                .ChangeType(
                                    property.PropertyType);

                            property.SetValue(instance, propValue);
                        }
                    }
                    catch (Exception)
                    {
                        _eventLogRepository.LogError(GetType(), nameof(ToWidgetViewModel),
                            $"Could not parse widget property: {widgetAttribute.Name}");
                    }

                }

                return instance;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(GetType(), nameof(ToWidgetViewModel), ex);

                return null;
            }
        }

    }

}

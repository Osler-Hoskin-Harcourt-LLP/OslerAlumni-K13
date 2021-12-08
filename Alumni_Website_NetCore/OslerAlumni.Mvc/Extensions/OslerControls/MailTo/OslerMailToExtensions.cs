using CMS.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OslerAlumni.Mvc.Extensions.OslerControls.MailTo
{
    public static class OslerMailToExtensionsExtensions
    {
        private static class Constants
        {
            public const string Subject = "Subject";
            public const string Body = "Body";
        }

        /// <summary>
        /// Useful for generating a generic submit button on Forms.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static OslerMailToBuilder MailTo(
            this OslerExtensionPoint<HtmlHelper> instance)
        {
            return new OslerMailToBuilder(instance.Target);
        }

        public class OslerMailToBuilder
        {
            private readonly HtmlHelper _html;

            private readonly TagBuilder _anchor;

            private string _email;
            private readonly List<Tuple<string, string>> _mailToProperties = new List<Tuple<string, string>>();


            public OslerMailToBuilder(HtmlHelper html)
            {
                _html = html;

                _anchor = new TagBuilder("a");
                _anchor.AddCssClass("c-mail-to");
                _anchor.Attributes.Add("target", "_top");
                _anchor.InnerHtml.AppendHtml($"<span class='show-for-sr'>{ResHelper.GetString(Core.Definitions.Constants.ResourceStrings.ScreenReader.ShareThisPage)}</span>");
            }

            /// <summary>
            /// Add email for mailto
            /// </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            public OslerMailToBuilder WithEmailAddress(string email)
            {
                _email = email;
                return this;
            }

            /// <summary>
            /// Add subject for mailto
            /// </summary>
            /// <param name="subject"></param>
            /// <returns></returns>
            public OslerMailToBuilder WithEmailSubject(string subject)
            {
                if (!string.IsNullOrWhiteSpace(subject))
                {

                    _mailToProperties.Add(new Tuple<string, string>(Constants.Subject, subject));
                }

                return this;
            }

            /// <summary>
            /// Add body for mailto
            /// </summary>
            /// <param name="body"></param>
            /// <returns></returns>
            public OslerMailToBuilder WithEmailBody(string body)
            {
                if (!string.IsNullOrWhiteSpace(body))
                {
                    _mailToProperties.Add(new Tuple<string, string>(Constants.Body, body));
                }

                return this;
            }


            /// <summary>
            /// Add inner html to anchor
            /// </summary>
            /// <param name="body"></param>
            /// <returns></returns>
            public OslerMailToBuilder WithContent(string html)
            {
                if (!string.IsNullOrWhiteSpace(html))
                {
                    _anchor.InnerHtml.AppendHtml(html);
                }

                return this;
            }

            /// <summary>
            /// Add attributes to the anchor tag
            /// </summary>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public OslerMailToBuilder WithAttributes(object attributes)
            {
                var htmlAttributesDictionary = (IDictionary<string, object>)
                    HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);

                _anchor.MergeAttributes(htmlAttributesDictionary);

                return this;
            }

            /// <summary>
            /// Add Css Class to the anchor tag
            /// </summary>
            /// <param name="cssClass"></param>
            /// <returns></returns>
            public OslerMailToBuilder AddClass(string cssClass)
            {
                _anchor.AddCssClass(cssClass);
                return this;
            }

            public HtmlString ToHtmlString()
            {
                _anchor.Attributes.Add("href", $"mailto:{_email}?{ParametersToUrl(_mailToProperties)}");
                return new HtmlString(_anchor.ToString());
            }

            private string ParametersToUrl(List<Tuple<string, string>> parameters)
            {
                var paramsEncoded = parameters.Select(p => $"{p.Item1}={Uri.EscapeDataString(p.Item2)}");

                return string.Join("&", paramsEncoded);
            }
        }
    }
}


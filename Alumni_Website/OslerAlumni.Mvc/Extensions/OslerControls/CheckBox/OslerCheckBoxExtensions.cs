using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace OslerAlumni.Mvc.Extensions.OslerControls.CheckBox
{
    public static class OslerCheckBoxExtensions
    {
        /// <summary>
        /// Useful for generating a generic check box for on Forms.
        /// The default CheckBoxFor by ASP.NET has too much markup
        /// that conflicts with our application.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static OslerCheckBoxBuilder<TModel> CheckBoxFor<TModel>(
            this OslerExtensionPoint<HtmlHelper<TModel>> instance, Expression<Func<TModel, bool>> expression)
        {
            return new OslerCheckBoxBuilder<TModel>(instance.Target, expression);
        }

        public class OslerCheckBoxBuilder<TModel>
        {
            private readonly HtmlHelper<TModel> _html;

            private TagBuilder input;

            public OslerCheckBoxBuilder(HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression)
            {
                _html = html;

                Func<TModel, bool> method = expression.Compile();

                bool isChecked = method(html.ViewData.Model);

                input = new TagBuilder("input");

                input.Attributes.Add("id", _html.IdFor(expression).ToString());
                input.Attributes.Add("name", _html.NameFor(expression).ToString());
                input.Attributes.Add("type", "checkbox");

                if (isChecked)
                {
                    input.Attributes.Add("checked", "");
                }
            }

            public MvcHtmlString ToHtmlString()
            {
                return MvcHtmlString.Create(input.ToString());
            }

        }

     
    }
}

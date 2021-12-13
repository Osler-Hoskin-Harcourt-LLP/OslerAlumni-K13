using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OslerAlumni.Mvc.Core.Helpers
{
    public static class HtmlContentHelper
    {
        public static string GetString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}

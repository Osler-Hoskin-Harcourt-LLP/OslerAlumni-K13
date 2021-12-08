using System;
using System.Collections.Generic;
using System.Linq;

namespace OslerAlumni.Mvc.Core.Attributes.Html
{
    // TODO: Remove this if possible and refactor.
    public class CustomHtmlAttributesAttribute 
        : Attribute
    {
        public string HtmlAttributes { get; set; }

        public Dictionary<string, object> Attributes
        {
            get
            {
                var dict = HtmlAttributes?
                    .Split(';')
                    .Select(keyvalue =>
                    {
                        var attr = keyvalue.Split(':');
                        return new KeyValuePair<string, object>(attr[0], attr[1]);
                    })
                    .ToDictionary(mc => mc.Key, mc => mc.Value);

                return dict ?? new Dictionary<string, object>();
            }
        }
    }
}

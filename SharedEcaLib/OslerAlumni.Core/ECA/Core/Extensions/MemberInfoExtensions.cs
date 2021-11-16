using System.Reflection;
using System.Xml.Serialization;
using ECA.Core.Definitions;
using Newtonsoft.Json;

namespace ECA.Core.Extensions
{
    public static class MemberInfoExtensions
    {
        public static string GetPropertyName(
            this MemberInfo property,
            NameSource propertyNameSource)
        {
            if (property == null)
            {
                return null;
            }

            switch (propertyNameSource)
            {
                case NameSource.Json:
                    return property
                        .GetCustomAttribute<JsonPropertyAttribute>(true)
                        ?.PropertyName;

                case NameSource.Xml:
                {
                    var result = property
                        .GetCustomAttribute<XmlElementAttribute>(true)
                        ?.ElementName;

                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }

                    result = property
                        .GetCustomAttribute<XmlAttributeAttribute>(true)
                        ?.AttributeName;

                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }

                    result = property
                        .GetCustomAttribute<XmlArrayAttribute>(true)
                        ?.ElementName;

                    return result;
                }
                default:
                    return property.Name;
            }
        }
    }
}

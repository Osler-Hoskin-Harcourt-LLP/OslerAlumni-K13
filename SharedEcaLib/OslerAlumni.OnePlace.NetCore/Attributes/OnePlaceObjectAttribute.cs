using System;
using System.Linq;

namespace OslerAlumni.OnePlace.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OnePlaceObjectAttribute 
        : Attribute
    {
        public string ObjectName { get; set; }

        public OnePlaceObjectAttribute(string objectName)
        {
            ObjectName = objectName;
        }

        public static string GetObjectName(Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }

            var attribute =
                type
                    .GetCustomAttributes(
                        typeof(OnePlaceObjectAttribute),
                        true)
                    .FirstOrDefault();

            if (attribute == null)
            {
                return string.Empty;
            }

            return ((OnePlaceObjectAttribute)attribute).ObjectName;
        }
    }
}

using System;
using CMS.Search.Azure;

namespace OslerAlumni.Mvc.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AzureSearchFieldAttribute
        : Attribute
    {
        #region "Properties"

        public string FieldName { get; }

        #endregion

        public AzureSearchFieldAttribute(
            string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException(
                    nameof(fieldName),
                    "Azure Search fields cannot be null or empty");
            }

            FieldName = NamingHelper.GetValidFieldName(fieldName);
        }
    }
}

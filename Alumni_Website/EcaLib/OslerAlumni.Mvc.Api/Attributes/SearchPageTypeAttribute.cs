using System;

namespace OslerAlumni.Mvc.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field, AllowMultiple = false)]
    public class SearchPageTypeAttribute
        : Attribute
    {
        #region "Properties"

        public string PageTypeName { get; }

        #endregion

        public SearchPageTypeAttribute(
            string pageTypeName)
        {
            if (string.IsNullOrWhiteSpace(pageTypeName))
            {
                throw new ArgumentNullException(
                    nameof(pageTypeName),
                    "Page Type name cannot be null or empty");
            }

            PageTypeName = pageTypeName.ToLower();
        }
    }
}

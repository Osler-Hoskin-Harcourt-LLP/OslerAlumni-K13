using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CMS.Base;

namespace OslerAlumni.Mvc.Api.Attributes.Validation
{
    public class AllowedValuesAttribute
        : ValidationAttribute
    {
        private readonly bool _convertToLowerCase;

        #region "Properties"

        private IList<string> _values;

        private IList<string> Values
        {
            get { return GetAllowableValues(); }
            set
            {
                _values =
                    _convertToLowerCase
                        ? value?.Select(v => v?.ToLowerCSafe()).ToArray()
                        : value;
            }
        }

        public virtual IList<string> GetAllowableValues()
        {
            return _values;
        }

        public bool IgnoreCase { get; }

        #endregion


        public AllowedValuesAttribute(
            string[] values,
            bool ignoreCase = false,
            bool convertToLowerCase = false)
        {
            _convertToLowerCase = convertToLowerCase;

            Values = values;

            IgnoreCase = ignoreCase;
        }

        #region "Methods"

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                // If the field is required, "[Required]" attribute should be used to indicate that.
                // Checking for the presence of a value is not in scope for this attribute.
                return true;
            }

            var valueType = value.GetType();

            IList<string> actualValues;

            if (valueType.IsArray || (valueType.IsGenericType && valueType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) ))
            {
                actualValues = value as IList<string>;

                if (actualValues == null)
                {
                    return false;
                }
            }
            else
            {
                actualValues = new[] { (string)value };
            }

            foreach (var actualValue in actualValues)
            {
                if (!Values.Any(v =>
                        string.Equals(
                            v,
                            actualValue,
                            IgnoreCase
                                ? StringComparison.OrdinalIgnoreCase
                                : StringComparison.InvariantCulture)))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}

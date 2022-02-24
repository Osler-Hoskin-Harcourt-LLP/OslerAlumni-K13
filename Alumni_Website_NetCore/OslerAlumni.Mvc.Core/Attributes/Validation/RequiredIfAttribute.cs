using System;
using System.ComponentModel.DataAnnotations;
using CMS.Helpers;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    /// <summary>
    /// This attribute is useful if some field is required based on some other field's selected value
    /// </summary>
    public class RequiredIfAttribute
        : RequiredAttribute,  IKenticoValidateAttribute
    {
        private String PropertyName { get; set; }
        private Object DesiredValue { get; set; }

        public RequiredIfAttribute(String propertyName, Object desiredvalue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredvalue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue?.ToString() == DesiredValue.ToString())
            {
                ValidationResult result = base.IsValid(value, context);
                return result;
            }
            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessageString;
        }
    }
}

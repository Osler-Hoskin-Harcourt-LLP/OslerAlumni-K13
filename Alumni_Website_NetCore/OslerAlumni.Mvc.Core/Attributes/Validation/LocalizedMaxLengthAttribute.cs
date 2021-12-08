using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using CMS.Helpers;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class LocalizedMaxLengthAttribute 
        : MaxLengthAttribute
    {
        /// <summary>
        /// Adds default validation error message containing the field's display name and character count.
        /// If ErrorMessage Field is specified, it is used instead.
        /// </summary>
        /// <param name="length"></param>
        public LocalizedMaxLengthAttribute(int length) : base(length)
        {

        }

        public override string FormatErrorMessage(string name)
        {
            var error = Constants.ResourceStrings.Form.GlobalMaxLengthError;

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                error = ErrorMessage;
            }

            return string.Format(ResHelper.GetString(error), name,
                Length);
        }
    }
}

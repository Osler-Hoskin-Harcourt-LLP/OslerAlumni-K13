using System.ComponentModel.DataAnnotations;
using CMS.Helpers;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    /// <summary>
    /// This allows to wrap the default .NET validation class to use Kentico Resource strings instead of .NET Resx files.
    /// Specifcy the ResourceString Id in the ErrorMessage Parameter of the attribute.
    /// e.g[ValidationAttribute(ErrorMessage = Constants.ResourceStrings.Form.ResetPassword.PasswordError)]
    /// </summary>
    public class KenticoValidateAttribute : ValidationAttribute, IKenticoValidateAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return ResHelper.GetString(ErrorMessageString);
        }
    }
}

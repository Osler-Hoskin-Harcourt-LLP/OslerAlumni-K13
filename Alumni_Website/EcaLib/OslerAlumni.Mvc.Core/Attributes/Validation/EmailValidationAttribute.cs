using System.Text.RegularExpressions;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class EmailValidationAttribute 
        : KenticoValidateAttribute
    {
        public override bool IsValid(object value)
        {
            string email = value?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            Regex regex = new Regex(GlobalConstants.RegexExpressions.EmailRegex);
            Match match = regex.Match(email);

            return match.Success;
        }
    }
}

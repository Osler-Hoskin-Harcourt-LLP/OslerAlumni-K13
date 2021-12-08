using System;
using System.Text.RegularExpressions;
using OslerAlumni.Mvc.Core.Definitions;


namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class StringNotContainsValidationAttribute 
        : KenticoValidateAttribute
    {
        private readonly string _charSequence;

        public StringNotContainsValidationAttribute(string charSequence)
        {
            _charSequence = charSequence;
        }

        public override bool IsValid(object value)
        {
            string input = value?.ToString() ?? string.Empty;

            return !input.Contains(_charSequence);
        }
    }
}

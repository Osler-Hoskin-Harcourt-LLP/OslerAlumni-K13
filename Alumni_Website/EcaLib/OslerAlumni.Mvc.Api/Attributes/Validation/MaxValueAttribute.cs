using System.ComponentModel.DataAnnotations;

namespace OslerAlumni.Mvc.Api.Attributes.Validation
{
    public class MaxValueAttribute
        : ValidationAttribute
    {
        #region "Properties"

        public int MaxValue { get; }

        #endregion

        public MaxValueAttribute(
            int maxValue)
        {
            MaxValue = maxValue;
        }

        #region "Methods"

        public override bool IsValid(object value)
        {
            return (int) value <= MaxValue;
        }

        #endregion
    }
}

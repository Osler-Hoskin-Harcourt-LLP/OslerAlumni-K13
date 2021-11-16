using System.ComponentModel.DataAnnotations;

namespace OslerAlumni.Mvc.Api.Attributes.Validation
{
    public class MinValueAttribute
        : ValidationAttribute
    {
        #region "Properties"

        public int MinValue { get; }

        #endregion

        public MinValueAttribute(
            int minValue)
        {
            MinValue = minValue;
        }

        #region "Methods"

        public override bool IsValid(object value)
        {
            return (int) value >= MinValue;
        }

        #endregion
    }
}

using System.Linq;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Mvc.Api.Attributes.Validation
{
    public class AllowedCultureAttribute
        : AllowedValuesAttribute
    {
        public AllowedCultureAttribute()
            : base(GlobalConstants.Cultures.AllowedCultureCodes.Values.ToArray())
        { }
    }
}

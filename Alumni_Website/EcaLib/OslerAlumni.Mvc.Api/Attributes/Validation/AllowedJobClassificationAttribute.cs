using System;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Api.Attributes.Validation
{
    public class AllowedJobClassificationAttribute
        : AllowedValuesAttribute
    {
        public AllowedJobClassificationAttribute()
            : base(Enum.GetNames(typeof(JobClassification)))
        { }
    }
}

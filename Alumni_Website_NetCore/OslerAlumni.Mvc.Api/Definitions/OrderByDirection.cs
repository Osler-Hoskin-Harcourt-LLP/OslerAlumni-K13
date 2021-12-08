using CMS.Helpers;

namespace OslerAlumni.Mvc.Api.Definitions
{
    public enum OrderByDirection
    {
        [EnumStringRepresentation("asc")]
        Ascending,
        [EnumStringRepresentation("desc")]
        Descending
    }
}

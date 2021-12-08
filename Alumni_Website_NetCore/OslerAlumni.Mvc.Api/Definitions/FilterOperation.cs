namespace OslerAlumni.Mvc.Api.Definitions
{
    public enum FilterOperation
    {
        Equal = 0,
        EqualOrEmpty = 1,
        NotEqual = 2,
        In = 3,
        NotIn = 4,
        LessThan = 5,
        LessOrEqualThan = 6,
        GreaterThan = 7,
        GreaterOrEqualThan = 8,
        LessThanOrEmpty = 9,
        LessOrEqualThanOrEmpty = 10,
        GreaterThanOrEmpty = 11,
        GreaterOrEqualThanOrEmpty = 12,
        Matches = 13
    }
}

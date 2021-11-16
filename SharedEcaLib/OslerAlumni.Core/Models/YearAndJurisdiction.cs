using System.Collections.Generic;

namespace OslerAlumni.Core.Models
{
    public class YearAndJurisdiction
    {
        public string Year { get; set; }

        public string Jurisdiction { get; set; }
    }

    /// <summary>
    /// Sorts YearAndJurisdictions by year of call
    /// </summary>
    public class YearAndJurisdictionComparer : IComparer<YearAndJurisdiction>
    {
        public int Compare(YearAndJurisdiction item1, YearAndJurisdiction item2)
        {
            if (item1 != null && item2 != null)
            {
                int year1; int year2;

                if (int.TryParse(item1.Year, out year1) && int.TryParse(item2.Year, out year2))
                {
                    return year1.CompareTo(year2);
                }
            }

            return 0;
        }
    }
}

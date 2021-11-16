using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CMS.Base;
using ECA.Core.Extensions;

using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Kentico.Helpers
{
    public static class UserProfileMappingHelper
    {
        private static readonly string[] LawDegrees =
        {
            "LL.B.",
            "LL.B./B.C.L.",
            "J.D.",
            "LL.L.",
            "LL.M.",
            "D.D.N.",
            "LL.B./M.B.A.",
            "J.D./M.B.A.",
            "H.B.A./LL.B.",
            "B.C.L./LL.B./M.B.A.",
            "J.D./LL.B.",
            "H.B.A./LL.B."
        };

        public static List<YearAndJurisdiction> YearOfCallAndJurisdictionsList(
            string yearsAndJurisdictions)
        {
            var result = new List<YearAndJurisdiction>();

            var jurisdictionsList = yearsAndJurisdictions.SplitOn(';', ',');

            jurisdictionsList.ForEach(item =>
            {
                var regex = new Regex(GlobalConstants.RegexExpressions.YearAndJurisdictionRegex);
                int outVar;
                var match = regex.Match(item);

                if (match.Success && int.TryParse(match.Groups[1]?.ToString()?.Trim(), out outVar))
                {
                    result.Add(new YearAndJurisdiction
                    {
                        Year = match.Groups[1]?.ToString()?.Trim(),
                        Jurisdiction = match.Groups[2]?.ToString()
                    });
                }
                else
                {
                    regex = new Regex(GlobalConstants.RegexExpressions.YearAndJurisdictionReversedRegex);
                    match = regex.Match(item);
                    if (match.Success && int.TryParse(match.Groups[1]?.ToString()?.Trim(), out outVar))
                    {
                        result.Add(new YearAndJurisdiction
                        {
                            Year = match.Groups[2]?.ToString()?.Trim(),
                            Jurisdiction = match.Groups[1]?.ToString()
                        });
                    }
                }
            });
            
            return result;
        }

        public static List<EducationRecord> ToEducationHistory(string educationHistoryStr)
        {
            var result = new List<EducationRecord>();

            var educationHistoryList = educationHistoryStr.SplitOn(';');

            var regex = new Regex(GlobalConstants.RegexExpressions.EducationHistoryRegex);

            educationHistoryList.ForEach(item =>
            {
                var match = regex.Match(item);

                if (match.Success)
                {
                    result.Add(new EducationRecord()
                    {
                        University = match.Groups[1].ToString().Trim(),
                        Year = match.Groups[2].ToInteger(0),
                        Degree = match.Groups[3].ToString().Trim()
                    });
                }
            });

            result = result.Where(educationRecord =>
                    educationRecord.Year > 0 && LawDegrees.ContainsCaseInsensitive(educationRecord.Degree))
                .ToList();

            result = result.OrderByDescending(educationRecord => educationRecord.Year).ToList();

            return result;
        }
    }
}

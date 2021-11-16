using Newtonsoft.Json;

namespace OslerAlumni.Admin.OnePlace.Models
{

    public class DataSubmissionQueueCleanerTaskSettings
    {
        [JsonProperty("dayCount")] public int ModifiedDayCount { get; set; } = 30;

        [JsonProperty("attemptCount")] public int FailedAttemptCount { get; set; } = 5;

        [JsonProperty("topN")] public int TopN { get; set; } = 100;
    }
}




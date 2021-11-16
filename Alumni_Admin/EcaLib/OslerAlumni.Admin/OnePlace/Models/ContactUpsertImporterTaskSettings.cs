using System;
using CMS.Scheduler;
using Newtonsoft.Json;
using OslerAlumni.Admin.OnePlace.Tasks;

namespace OslerAlumni.Admin.OnePlace.Models
{
    /// <summary>
    /// Class for the configuration parameters of the scheduled task
    /// defined by <see cref="ContactUpsertImporterTask"/>.
    /// The configuration is set in the form of a JSON string in the
    /// <see cref="TaskInfo.TaskData"/> property of the scheduled task in the CMS.
    /// </summary>
    public class ContactUpsertImporterTaskSettings
    {
        [JsonProperty("batchSize")]
        public int BatchSize { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("batchSizeIncrement")]
        public int BatchSizeIncrement { get; set; }

        [JsonProperty("batchSizeIncrementCount")]
        public int BatchSizeIncrementCount { get; set; }

        [JsonProperty("defaultBatchSize")]
        public int DefaultBatchSize { get; set; }

        [JsonIgnore]
        public bool IsMaxBatchIncrementCountReached
            => (BatchSizeIncrement <= 0) 
               || ((BatchSize - DefaultBatchSize) / BatchSizeIncrement >= BatchSizeIncrementCount);
    }
}

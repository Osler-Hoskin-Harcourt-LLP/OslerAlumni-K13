using CMS.Scheduler;
using Newtonsoft.Json;
using OslerAlumni.Admin.OnePlace.Tasks;

namespace OslerAlumni.Admin.OnePlace.Models
{
    /// <summary>
    /// Class for the configuration parameters of the scheduled task
    /// defined by <see cref="ContactDeleteImporterTask"/>.
    /// The configuration is set in the form of a JSON string in the
    /// <see cref="TaskInfo.TaskData"/> property of the scheduled task in the CMS.
    /// </summary>
    public class ContactDeleteImporterTaskSettings
    {
        [JsonProperty("batchSize")]
        public int BatchSize { get; set; }
    }
}

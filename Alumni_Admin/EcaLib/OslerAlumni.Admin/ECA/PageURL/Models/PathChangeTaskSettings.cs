using CMS.Scheduler;
using ECA.Admin.PageURL.Tasks;
using Newtonsoft.Json;

namespace ECA.Admin.PageURL.Models
{
    /// <summary>
    /// Class for the configuration parameters of the scheduled task
    /// defined by <see cref="PathChangePageUrlUpdaterTask"/>.
    /// The configuration is set in the form of a JSON string in the
    /// <see cref="TaskInfo.TaskData"/> property of the scheduled task in the CMS.
    /// </summary>
    public class PathChangeTaskSettings
    {
        [JsonProperty("pathChangeBatchSize")]
        public int PathChangeBatchSize { get; set; }

        [JsonProperty("descendantPageBatchSize")]
        public int DescendantPageBatchSize { get; set; }
    }
}

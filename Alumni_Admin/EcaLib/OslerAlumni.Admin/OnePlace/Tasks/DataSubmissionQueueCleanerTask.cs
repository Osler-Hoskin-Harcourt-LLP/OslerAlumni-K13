using System;
using CMS.Scheduler;
using Newtonsoft.Json;
using OslerAlumni.Admin.OnePlace.Models;
using OslerAlumni.OnePlace.Repositories;

namespace OslerAlumni.Admin.OnePlace.Tasks
{
    public partial class DataSubmissionQueueCleanerTask
        : BaseOnePlaceTask
    {
        #region "Properties"
        public IDataSubmissionQueueItemRepository DataSubmissionQueueItemRepository { get; set; }

        #endregion
        
        #region "Helper methods"

        protected override string ExecuteInternal(
            TaskInfo task)
        { 
            var errorMessage = string.Empty;

            var totalCount = 0;
            var unprocessedCount = 0;
            var delCount = 0;

            try
            {

                var settings = GetTaskSettings(task);

                delCount = DataSubmissionQueueItemRepository.PurgeDataSubmissionQueueItems(
                    settings.FailedAttemptCount, settings.ModifiedDayCount, settings.TopN);


                unprocessedCount = DataSubmissionQueueItemRepository
                    .GetDataSubmissionQueueItemCount(false);

                totalCount =
                    DataSubmissionQueueItemRepository
                        .GetDataSubmissionQueueItemCount(null);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = $"Error occurred: {errorMessage} ";
            }

            return
                $"{errorMessage}{delCount} item(s) removed. {unprocessedCount} unprocessed item(s) and {totalCount} item(s) in total left in the queue.";
        }

        protected DataSubmissionQueueCleanerTaskSettings GetTaskSettings(TaskInfo task)
        {
            return
                JsonConvert.DeserializeObject<DataSubmissionQueueCleanerTaskSettings>(task.TaskData)
                ??
                new DataSubmissionQueueCleanerTaskSettings();
        }

        #endregion
    }
}

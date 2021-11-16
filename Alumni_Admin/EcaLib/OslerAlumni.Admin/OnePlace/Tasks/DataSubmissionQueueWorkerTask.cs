using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.Scheduler;
using ECA.Core.Repositories;
using Newtonsoft.Json;
using OslerAlumni.Admin.OnePlace.Models;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Repositories;
using OslerAlumni.OnePlace.Services;

namespace OslerAlumni.Admin.OnePlace.Tasks
{
    public class DataSubmissionQueueWorkerTask
        : BaseOnePlaceTask
    {
        #region "Constants"

        protected const string ErrorResultMessage =
            "An error occurred when processing queued data submission tasks. Please check the event log for error details.";

        #endregion

        #region "Properties"

        public IDataSubmissionQueueItemRepository DataSubmissionQueueItemRepository { get; set; }

        public IEventLogRepository EventLogRepository { get; set; }

        public IDataSubmissionQueueService DataSubmissionQueueService { get; set; }

        #endregion

        #region "Helper methods"

        protected override string ExecuteInternal(
            TaskInfo task)
        {
            try
            {
                var settings = GetTaskSettings(task);

                // Get unprocessed items from the queue
                // NOTE: This actually does have a potential for getting stuck,
                // if there are too many unprocessed tasks in the queue,
                // i.e. it will keep picking them up from the top of the table.
                // However, if that is the case, the problem with failures needs to be addressed, 
                // instead of us trying to take that into account and complicating the query logic in this task,
                // which would result in a performance hit
                var items = DataSubmissionQueueItemRepository
                    .GetUnprocessedDataSubmissionQueueItems(
                        settings.TopN,
                        settings.MaxAttemptCount);

                string message;

                TryProcessQueuedTasks(
                    items,
                    settings,
                    out message);

                return message;
            }
            catch (Exception ex)
            {
                EventLogRepository.LogError(
                    GetType(),
                    nameof(Execute),
                    ex);

                return ErrorResultMessage;
            }
        }

        protected DataSubmissionQueueWorkerTaskSettings GetTaskSettings(
            TaskInfo task)
        {
            var settings =
                JsonConvert.DeserializeObject<DataSubmissionQueueWorkerTaskSettings>(
                    task?.TaskData);

            return settings ?? new DataSubmissionQueueWorkerTaskSettings
            {
                MaxAttemptCount = 5,
                // 2000 is the max value QA'd before the Salesforce 2k
                // list retrieval limit was lifted for ticket #OcrmProd-440,
                // so this seems a sensible processing size to keep.
                TopN = 2000
            };
        }

        protected bool TryProcessQueuedTasks(
            IList<CustomTable_DataSubmissionQueueItem> items,
            DataSubmissionQueueWorkerTaskSettings settings,
            out string message)
        {
            if (DataHelper.DataSourceIsEmpty(items))
            {
                message = "There are no data submissions tasks to process.";

                return true;
            }

            var hasError = false;
            var retriedTaskCount = 0;
            var unprocessedTaskCount = 0;

            try
            {
                foreach (var item in items)
                {
                    try
                    {
                        var dependsOnItemIds = item.DependsOnItemIdsArray;

                        // Check if all of the "parent" tasks (the dependencies) still exist and have been processed
                        if ((dependsOnItemIds != null) && (dependsOnItemIds.Length > 0))
                        {
                            item.DependsOnItemsArray = dependsOnItemIds.Select(
                                dependsOnItemId =>
                                    DataSubmissionQueueItemRepository
                                        .GetDataSubmissionQueueItem(dependsOnItemId))
                                .ToArray();

                            if (item.DependsOnItemsArray.Any(
                                    dependsOnItem =>
                                        (dependsOnItem == null) || !dependsOnItem.IsProcessed))
                            {
                                continue;
                            }
                        }

                        // Check if there are "older" unprocessed tasks for this contact or user: 
                        // these tasks should take precedence
                        if ((item.ContextObjectId > 0)
                            && DataSubmissionQueueItemRepository
                                .ExistSharedContextUnprocessedDataSubmissionQueueItems(
                                    item,
                                    settings.MaxAttemptCount))
                        {
                            continue;
                        }

                        DataSubmissionQueueService.Retry(item);

                        retriedTaskCount++;
                    }
                    catch (Exception ex)
                    {
                        hasError = true;

                        EventLogRepository.LogError(
                            GetType(),
                            nameof(TryProcessQueuedTasks),
                            ex);
                    }
                }

                unprocessedTaskCount = DataSubmissionQueueItemRepository
                    .GetDataSubmissionQueueItemCount(false);
            }
            catch (Exception ex)
            {
                hasError = true;

                EventLogRepository.LogError(
                    GetType(),
                    nameof(TryProcessQueuedTasks),
                    ex);
            }

            message =
                (hasError
                    ? $"{ErrorResultMessage} "
                    : string.Empty)
                + $"{retriedTaskCount} tasks(s) (re)tried. {unprocessedTaskCount} unprocessed task(s) left in the queue.";

            return !hasError;
        }

        #endregion
    }
}

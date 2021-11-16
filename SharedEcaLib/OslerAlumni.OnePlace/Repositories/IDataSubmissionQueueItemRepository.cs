using System.Collections.Generic;
using ECA.Core.Repositories;
using OslerAlumni.OnePlace.Kentico.Models;

namespace OslerAlumni.OnePlace.Repositories
{
    public interface IDataSubmissionQueueItemRepository
        : IRepository
    {
        bool ExistSharedContextUnprocessedDataSubmissionQueueItems(
            CustomTable_DataSubmissionQueueItem item,
            int? maxAttemptCount = null);

        CustomTable_DataSubmissionQueueItem GetDataSubmissionQueueItem(
            int itemId);

        int GetDataSubmissionQueueItemCount(
            bool? processed);

        IList<CustomTable_DataSubmissionQueueItem> GetUnprocessedDataSubmissionQueueItems(
            int count,
            int? maxAttemptCount = null);

        void Save(
            CustomTable_DataSubmissionQueueItem item);

        /// <summary>
        /// returns number of deleted rows
        /// </summary>
        /// <param name="failedAttemptCount"></param>
        /// <param name="modifiedDayCount"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        int PurgeDataSubmissionQueueItems(int failedAttemptCount,
            int modifiedDayCount, int count);
    }
}

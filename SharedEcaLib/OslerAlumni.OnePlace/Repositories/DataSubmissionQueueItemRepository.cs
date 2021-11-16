using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DataEngine.Query;
using CMS.Helpers;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Queries;

namespace OslerAlumni.OnePlace.Repositories
{
    public class DataSubmissionQueueItemRepository
        : IDataSubmissionQueueItemRepository
    {
        #region "Methods"

        public bool ExistSharedContextUnprocessedDataSubmissionQueueItems(
            CustomTable_DataSubmissionQueueItem item,
            int? maxAttemptCount = null)
        {
            if ((item == null)
                || (item.ItemID < 1)
                || (item.ContextObjectId < 1)
                || string.IsNullOrWhiteSpace(item.ContextObjectType))
            {
                return false;
            }

            var query =
                CustomTableItemProvider
                    .GetItems<CustomTable_DataSubmissionQueueItem>()
                    .Columns(
                        nameof(CustomTable_DataSubmissionQueueItem.ItemID))
                    .TopN(1)
                    .WhereEquals(
                        nameof(CustomTable_DataSubmissionQueueItem.ContextObjectId), 
                        item.ContextObjectId)
                    .WhereEquals(
                        nameof(CustomTable_DataSubmissionQueueItem.ContextObjectType),
                        item.ContextObjectType)
                    .WhereFalse(
                        nameof(CustomTable_DataSubmissionQueueItem.IsProcessed))
                    .WhereLessThan(
                        nameof(CustomTable_DataSubmissionQueueItem.ItemID), 
                        item.ItemID);

            if ((maxAttemptCount ?? 0) > 0)
            {
                query = query
                    .WhereLessThan(
                        nameof(CustomTable_DataSubmissionQueueItem.TotalAttempts),
                        maxAttemptCount);
            }

            return query.GetCount() > 0;
        }

        public CustomTable_DataSubmissionQueueItem GetDataSubmissionQueueItem(
            int itemId)
        {
            if (itemId < 1)
            {
                return null;
            }

            return CustomTableItemProvider
                .GetItem<CustomTable_DataSubmissionQueueItem>(itemId);
        }

        public int GetDataSubmissionQueueItemCount(
            bool? processed)
        {
            var query = CustomTableItemProvider
                .GetItems<CustomTable_DataSubmissionQueueItem>()
                .Columns(
                    nameof(CustomTable_DataSubmissionQueueItem.ItemID));

            if (processed.HasValue)
            {
                query = query
                    .WhereEquals(
                        nameof(CustomTable_DataSubmissionQueueItem.IsProcessed),
                        processed.Value);
            }

            return query
                .GetCount();
        }

        public IList<CustomTable_DataSubmissionQueueItem> GetUnprocessedDataSubmissionQueueItems(
            int count,
            int? maxAttemptCount = null)
        {
            if (count < 1)
            {
                return null;
            }

            var query = CustomTableItemProvider
                .GetItems<CustomTable_DataSubmissionQueueItem>()
                .TopN(count)
                .WhereFalse(
                    nameof(CustomTable_DataSubmissionQueueItem.IsProcessed))
                .OrderByAscending(
                    nameof(CustomTable_DataSubmissionQueueItem.ItemID));

            // Check for max attempt count
            if ((maxAttemptCount ?? 0) > 0)
            {
                query = query
                    .WhereLessThan(
                        nameof(CustomTable_DataSubmissionQueueItem.TotalAttempts), 
                        maxAttemptCount);
            }

            return query.ToList();
        }


        /// <inheritdoc />
        public int PurgeDataSubmissionQueueItems(int failedAttemptCount, int modifiedDayCount, int count)
        {
            // Get processed (without unprocessed dependant tasks)
            // or "hopeless" (too many failed attempts or missing dependency) items from the queue

            var query = new DataQuery
            {
                CustomQueryText = DataSubmissionQueueQueries.PurgeDataSubmissionQueueItems,
                Parameters = new QueryDataParameters
                {
                    {"@TotalAttempts", failedAttemptCount},
                    {"@Count", count},
                    {"@ModifiedDayCount", modifiedDayCount}
                }
            };

            var dataSet = query.Result;

            DataTable dt = dataSet.Tables[0];

            //TODO: Should be able to use DataQuery's .GetScalarResult instead
            return ValidationHelper.GetInteger(dt.Rows[0]["Deleted"], 0);
        }

        public void Save(
            CustomTable_DataSubmissionQueueItem item)
        {
            if (item == null)
            {
                return;
            }

            CustomTableItemProvider.SetItem(item);
        }

        #endregion
    }
}

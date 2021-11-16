using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Scheduler;
using ECA.Admin.Core.Tasks;
using ECA.Admin.PageURL.Models;
using ECA.Admin.PageURL.Repositories;
using ECA.Admin.PageURL.Services;
using ECA.Core.Repositories;
using ECA.PageURL.Kentico.Models;
using ECA.PageURL.Services;
using Newtonsoft.Json;

namespace ECA.Admin.PageURL.Tasks
{
    public class PathChangePageUrlUpdaterTask
        : BaseTask
    {
        #region "Properties"

        public IEventLogRepository EventLogRepository { get; set; }

        public IPathChangeItemRepository PathChangeItemRepository { get; set; }

        public IPageUrlService PageUrlService { get; set; }

        public IPathChangeService PathChangeService { get; set; }

        #endregion

        #region "Methods"

        public override string Execute(
            TaskInfo task)
        {
            var settings = GetTaskSettings(task);

            IList<CustomTable_PathChangeItem> completedPathChanges;
            string message;

            if (TryProcessPathChanges(
                    settings,
                    out completedPathChanges,
                    out message))
            {
                // If there were no errors encountered, we can delete the path changes
                // that rendered 0 results for affected pages, so that the task doesn't
                // keep picking them up for processing
                PathChangeItemRepository.Delete(completedPathChanges);
            }

            return message;
        }

        #endregion

        #region "Helper methods"

        protected PathChangeTaskSettings GetTaskSettings(
            TaskInfo task)
        {
            var settings =
                JsonConvert.DeserializeObject<PathChangeTaskSettings>(
                    task?.TaskData);

            return settings ?? new PathChangeTaskSettings
            {
                PathChangeBatchSize = 5,
                DescendantPageBatchSize = 20
            };
        }

        protected bool TryProcessPathChanges(
            PathChangeTaskSettings settings,
            out IList<CustomTable_PathChangeItem> completedPathChanges,
            out string message)
        {
            completedPathChanges = new List<CustomTable_PathChangeItem>();

            var hasError = false;
            var pathChangeCount = 0;
            var pageCount = 0;

            try
            {
                // Get top N path changes, in chronological order
                var pathChanges = PathChangeItemRepository
                    .GetPathChangeItems(settings.PathChangeBatchSize);

                if (DataHelper.DataSourceIsEmpty(pathChanges))
                {
                    message = "There are no path changes to process.";

                    return true;
                }

                foreach (var pathChange in pathChanges)
                {
                    TreeNode pathChangePage;

                    // If the document name/path change had been reverted,
                    // i.e. the current name/path of the page is the same as it was before the path change
                    // that we are trying to process, there is no point in doing so:
                    // - either the sub-pages have the path in their main URLs and it was never changed to anything else;
                    // - or there was another path change that was processed before this,
                    // and their main URLs were already updated to use their ancenstor's current name/path
                    if (PathChangeService.IsPathChangeReverted(pathChange, out pathChangePage))
                    {
                        // We will still count it as success :)
                        pathChangeCount++;

                        // Add it to the list that is to be returned,
                        // so that the path change is treated as fully processed and is deleted if necessary
                        completedPathChanges.Add(pathChange);

                        continue;
                    }

                    int currentPageCount;

                    // For each path change pull the list of potentially affected pages.
                    // These are being determined as pages, whose main URLs still start with the old prefix.
                    hasError = !TryProcessAffectedPages(
                        pathChange,
                        pathChangePage,
                        settings,
                        out currentPageCount);

                    pageCount += currentPageCount;

                    if (hasError)
                    {
                        break;
                    }

                    pathChangeCount++;

                    // Add it to the list that is to be returned,
                    // so that the path change is treated as fully processed and is deleted if necessary
                    completedPathChanges.Add(pathChange);
                }
            }
            catch (Exception ex)
            {
                hasError = true;

                EventLogRepository.LogError(
                    GetType(),
                    nameof(TryProcessPathChanges),
                    ex);
            }

            message =
                (hasError
                    ? "An error occurred when processing path changes. Please check the event log for error details. "
                    : string.Empty)
                + $"Successfully processed {pathChangeCount} path changes. Processed {pageCount} pages in total.";

            return !hasError;
        }

        protected bool TryProcessAffectedPages(
            CustomTable_PathChangeItem pathChange,
            TreeNode pathChangePage,
            PathChangeTaskSettings settings,
            out int pageCount)
        {
            pageCount = 0;

            IList<TreeNode> previousAffectedPages = null;

            do
            {
                IList<TreeNode> affectedPages;

                // For each path change pull the list of potentially affected pages.
                // These are being determined as pages, whose main URLs still start with the old prefix.
                if (!PathChangeService.TryGetPagesInPath(
                        pathChange,
                        pathChangePage,
                        settings.DescendantPageBatchSize,
                        out affectedPages))
                {
                    return false;
                }

                // If there are no affected pages, or all of them have been processed already,
                // we count it as success
                if (DataHelper.DataSourceIsEmpty(affectedPages))
                {
                    // Break condition
                    return true;
                }

                // NOTE: This should never happen, but this is a safeguard to ensure that the task 
                // is not looping through the same set of pages again and again
                if ((previousAffectedPages != null)
                    && affectedPages.All(page1 =>
                        previousAffectedPages.Any(page2 => page2.NodeGUID == page1.NodeGUID)))
                {
                    // If the task has started looping through the same set of pages,
                    // assume that there is nothing to modify, i.e. this would mean that
                    // - either the pages already have the correct name of the ancestor;
                    // - or that the task is not allowed to update them
                    // Either way, there is nothing for the task to do here,
                    // So this is break condition
                    return true;
                }

                foreach (var affectedPage in affectedPages)
                {
                    // Trigger the logic of automatically determining the default URL of a page
                    // for each of the affected descendant pages
                    if (!PageUrlService.TrySetPageMainUrl(affectedPage))
                    {
                        return false;
                    }

                    pageCount++;
                }

                previousAffectedPages = affectedPages;
            }
            // We will keep going until:
            // - there is an error;
            // - we started looping through the same set of pages, meaning the task can't do anything further;
            // - we have successfully processed all affected pages.
            // These are reflected in break conditions inside the loop.
            while (true);
        }

        #endregion
    }
}

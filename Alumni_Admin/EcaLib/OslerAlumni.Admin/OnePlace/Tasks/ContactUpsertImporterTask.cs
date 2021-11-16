using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.Scheduler;
using ECA.Core.Repositories;
using Newtonsoft.Json;
using OslerAlumni.Admin.OnePlace.Models;
using OslerAlumni.Admin.OnePlace.Services;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Services;

namespace OslerAlumni.Admin.OnePlace.Tasks
{
    public class ContactUpsertImporterTask
        : BaseOnePlaceTask
    {
        #region "Constants"

        protected const string ErrorResultMessage =
            "An error occurred when importing Alumni contacts from OnePlace. Please check the event log for error details.";

        #endregion

        #region "Properties"

        public IEventLogRepository EventLogRepository { get; set; }

        public IOnePlaceContactService OnePlaceContactService { get; set; }

        public IOnePlaceUserImportService OnePlaceUserImportService { get; set; }

        #endregion

        #region "Helper methods"

        protected override string ExecuteInternal(
            TaskInfo task)
        {
            try
            {
                var settings = GetTaskSettings(task);

                IList<Contact> contacts;
                string message;

                if (!OnePlaceContactService.TryGetAlumniContacts(
                        settings.BatchSize,
                        out contacts,
                        out message,
                        settings.StartDate))
                {
                    return message;
                }

                if (!CanImportContacts(
                        task,
                        contacts,
                        settings,
                        out message))
                {
                    return message;
                }

                if (TryImportContacts(
                        contacts,
                        settings,
                        out message))
                {
                    // Save the LastModifiedDate of the last contact in the current batch,
                    // so that the next batch can retrieve contacts modified on or after that date.
                    // This will ensure that the batch window moves along, provided the list of contacts
                    // retrieved from OnePlace is order by LastModifiedDate column, in ascending order.
                    // We are also assuming that LastModifiedDate has the combined latest modification date
                    // of the contact record and any other related records (e.g. Account, Board Member relationships, etc.),
                    // since SOQL syntax does not support aggregating through values of multiple columns
                    settings.StartDate = contacts.Last().LastModifiedDate;

                    // Reset the batch size, so that the task can start increment against this baseline
                    // the next time it encounters the issue with the first and last contact in the batch
                    // having the same LastModifiedDate
                    settings.BatchSize = settings.DefaultBatchSize;

                    SetTaskSettings(task, settings);
                }
                else
                {
                    // In case of a processing error, disable the task until the issue can be resolved manually
                    task.TaskEnabled = false;
                }

                return message;
            }
            catch (Exception ex)
            {
                EventLogRepository.LogError(
                    GetType(),
                    nameof(Execute),
                    ex);

                // In case of a processing error, disable the task until the issue can be resolved manually
                task.TaskEnabled = false;

                return ErrorResultMessage;
            }
        }

        protected bool CanImportContacts(
            TaskInfo task,
            IList<Contact> contacts,
            ContactUpsertImporterTaskSettings settings,
            out string message)
        {
            message = string.Empty;
            
            if (DataHelper.DataSourceIsEmpty(contacts))
            {
                message = "There are no new or updated Alumni contacts to import from OnePlace.";

                return false;
            }

            // Before trying to process the contacts, make sure that we will be able to move the batch window afterwards:
            // first and last contact in the batch should have different LastModifiedDate values in order to do that.
            // Note that if the number of contacts is less than the batch size, then there are no more contacts to import
            // at this time anyways, so we should proceed with the task execution.
            if ((contacts.Count >= settings.BatchSize) 
                && (contacts.First().LastModifiedDate == contacts.Last().LastModifiedDate))
            {
                // Check that we can still increment the batch size
                if (settings.IsMaxBatchIncrementCountReached)
                {
                    // Disable the task until the issue can be resolved manually
                    task.TaskEnabled = false;

                    // Reset the batch size to the initial value
                    settings.BatchSize = settings.DefaultBatchSize;

                    SetTaskSettings(task, settings);

                    message =
                        "Reached the maximum number of batch size increments. The issue will require manual intervention before the task execution can continue.";

                    return false;
                }

                settings.BatchSize += settings.BatchSizeIncrement;

                SetTaskSettings(task, settings);

                message =
                    "The first and last contacts within the retrieved batch have the same modification date, which means the batch window cannot be shifted. Trying to increment the batch size to get around the issue.";

                return false;
            }

            return true;
        }

        protected ContactUpsertImporterTaskSettings GetTaskSettings(
            TaskInfo task)
        {
            var settings =
                JsonConvert.DeserializeObject<ContactUpsertImporterTaskSettings>(
                    task?.TaskData);

            return settings ?? new ContactUpsertImporterTaskSettings
            {
                BatchSize = 50,
                BatchSizeIncrement = 10,
                BatchSizeIncrementCount = 5,
                DefaultBatchSize = 50
            };
        }

        protected bool TryImportContacts(
            IList<Contact> contacts,
            ContactUpsertImporterTaskSettings settings,
            out string message)
        {
            var hasError = false;
            var newContactCount = 0;
            var updatedContactCount = 0;
            var skippedContactCount = 0;

            try
            {
                foreach (var contact in contacts)
                {
                    ImportAction importAction;

                    if (!OnePlaceUserImportService.ImportAsUser(
                            contact,
                            out importAction))
                    {
                        hasError = true;

                        break;
                    }

                    switch (importAction)
                    {
                        case ImportAction.Create:
                            newContactCount++;

                            break;
                        case ImportAction.Update:
                            updatedContactCount++;

                            break;
                        case ImportAction.Skip:
                            skippedContactCount++;

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                hasError = true;

                EventLogRepository.LogError(
                    GetType(),
                    nameof(TryImportContacts),
                    ex);
            }

            message =
                (hasError
                    ? $"{ErrorResultMessage} "
                    : string.Empty)
                + $"Successfully imported {newContactCount} new and {updatedContactCount} updated Alumni contacts."
                + ((skippedContactCount > 0)
                    ? $" Skipped {skippedContactCount} records due to the likelihood that information in OnePlace is out of date."
                    : string.Empty);

            return !hasError;
        }

        protected void SetTaskSettings(
            TaskInfo task,
            ContactUpsertImporterTaskSettings settings)
        {
            task.TaskData =
                JsonConvert.SerializeObject(
                    settings,
                    Formatting.Indented);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.Scheduler;
using ECA.Core.Repositories;
using Newtonsoft.Json;
using OslerAlumni.Admin.Core.Repositories;
using OslerAlumni.Admin.OnePlace.Models;
using OslerAlumni.Admin.OnePlace.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Services;

namespace OslerAlumni.Admin.OnePlace.Tasks
{
    public class ContactDeleteImporterTask
        : BaseOnePlaceTask
    {
        #region "Constants"

        protected const string ErrorResultMessage =
            "An error occurred when checking for deleted, merged, deceased and re-hired Alumni contacts in OnePlace. Please check the event log for error details.";

        #endregion

        #region "Properties"

        public IEventLogRepository EventLogRepository { get; set; }

        public IAdminUserRepository UserRepository { get; set; }

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

                string message;

                if (!TryProcessAlumniUsers(
                        settings,
                        out message))
                {
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

        protected ContactDeleteImporterTaskSettings GetTaskSettings(
            TaskInfo task)
        {
            var settings =
                JsonConvert.DeserializeObject<ContactDeleteImporterTaskSettings>(
                    task?.TaskData);

            return settings ?? new ContactDeleteImporterTaskSettings
            {
                BatchSize = 50
            };
        }

        protected bool TryProcessAlumniUsers(
            ContactDeleteImporterTaskSettings settings,
            out string message)
        {
            var hasError = false;
            var deletedCount = 0;
            var badTermsCount = 0;
            var rehiredCount = 0;
            var deceasedCount = 0;

            try
            {
                // This is the starting UserID for the first batch,
                // it will be updated after each batch
                // Assuming identity column with starting ID of 1 here
                var startingUserID = 1;

                while (true)
                {
                    // We actually have to pull all columns here,
                    // otherwise Kentico throws an error during an update
                    var users = UserRepository.GetAlumniUsers(
                        settings.BatchSize,
                        startingUserID,
                        orderByColumnName: nameof(IOslerUserInfo.UserID),
                        orderDirection: OrderDirection.Ascending);

                    int deletedSubCount;
                    int badTermsSubCount;
                    int rehiredSubCount;
                    int deceasedSubCount;

                    if (!TryVerifyAlumniUserStatuses(
                            users,
                            out deletedSubCount,
                            out badTermsSubCount,
                            out rehiredSubCount,
                            out deceasedSubCount,
                            out message))
                    {
                        hasError = true;
                    }

                    deletedCount += deletedSubCount;
                    badTermsCount += badTermsSubCount;
                    rehiredCount += rehiredSubCount;
                    deceasedCount += deceasedSubCount;

                    // Keep processing users in batches, until we encounter an error or the number of users we pulled
                    // is less than the size of the batch, which indicates there are no more users to process
                    if (hasError || (users == null) || (users.Count < settings.BatchSize))
                    {
                        break;
                    }

                    var lastUser = users.LastOrDefault();

                    // Should never happen, just a precaution
                    if ((lastUser == null) || (lastUser.UserID < startingUserID))
                    {
                        EventLogRepository.LogError(
                            GetType(),
                            nameof(TryProcessAlumniUsers),
                            $"Unable to shift the batch window. Last user in the current batch (UserID = {lastUser?.UserID ?? 0}) is less than the starting user ID used for that batch ({startingUserID}).");

                        break;
                    }

                    // Get the ID of the last user from the batch, so that we can shift the batch window
                    startingUserID = lastUser.UserID + 1;
                }
            }
            catch (Exception ex)
            {
                hasError = true;

                EventLogRepository.LogError(
                    GetType(),
                    nameof(TryProcessAlumniUsers),
                    ex);
            }

            message =
                (hasError
                    ? $"{ErrorResultMessage} "
                    : string.Empty)
                + $"Successfully detected {deletedCount} deleted or merged Alumni contacts; {badTermsCount} Alumni, who were identified as having left on bad terms; {rehiredCount} Alumni, who were re-hired by Osler; {deceasedCount} Alumni, who are deceased.";

            return !hasError;
        }

        protected bool TryVerifyAlumniUserStatuses(
            IList<IOslerUserInfo> users,
            out int deletedCount,
            out int badTermsCount,
            out int rehiredCount,
            out int deceasedCount,
            out string errorMessage)
        {
            deletedCount = 0;
            badTermsCount = 0;
            rehiredCount = 0;
            deceasedCount = 0;
            errorMessage = null;

            if ((users == null) || (users.Count < 1))
            {
                // Empty user list doesn't constitute an error
                return true;
            }

            IList<Contact> contacts;

            if (!OnePlaceContactService.TryGetAlumniContacts(
                    users.Select(user => user.OnePlaceReference).ToList(),
                    out contacts,
                    out errorMessage,
                    includeMergeParents: true))
            {
                return false;
            }

            foreach (var user in users)
            {
                AlumniChangeType changeType;

                if (!OnePlaceUserImportService.UpdateUserStatus(
                        user,
                        contacts,
                        out changeType))
                {
                    errorMessage =
                        $"An error occurred when update user status for user '{user.UserName}'";

                    return false;
                }

                switch (changeType)
                {
                    case AlumniChangeType.Deleted:
                        deletedCount++;

                        break;
                    case AlumniChangeType.LeftOnBadTerms:
                        badTermsCount++;

                        break;
                    case AlumniChangeType.Rehired:
                        rehiredCount++;

                        break;
                    case AlumniChangeType.Deceased:
                        deceasedCount++;

                        break;
                }
            }

            return true;
        }

        #endregion
    }
}

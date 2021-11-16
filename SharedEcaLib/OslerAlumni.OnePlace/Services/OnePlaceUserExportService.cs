using System;
using System.Collections.Generic;
using System.Linq;
using ECA.Core.Extensions;
using ECA.Core.Repositories;
using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceUserExportService
        : ServiceBase, IOnePlaceUserExportService
    {
        #region "Constants"

        protected static readonly string[] OnePlaceSourcedFields =
        {
            nameof(IOslerUserInfo.Company),
            nameof(IOslerUserInfo.FirstName),
            nameof(IOslerUserInfo.LastName),
            nameof(IOslerUserInfo.Email),
            nameof(IOslerUserInfo.JobTitle),
            nameof(IOslerUserInfo.City),
            nameof(IOslerUserInfo.Province),
            nameof(IOslerUserInfo.Country),
            nameof(IOslerUserInfo.BoardMemberships),
            nameof(IOslerUserInfo.SubscriptionPreferences),
            nameof(IOslerUserInfo.CommunicationPreferences)
        };

        #endregion

        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDataSubmissionQueueService _dataSubmissionQueueService;

        #endregion

        public OnePlaceUserExportService(
            IEventLogRepository eventLogRepository,
            IUserRepository userRepository,
            IDataSubmissionQueueService dataSubmissionQueueService)
        {
            _eventLogRepository = eventLogRepository;
            _userRepository = userRepository;
            _dataSubmissionQueueService = dataSubmissionQueueService;
        }

        #region "Methods"

        /// <inheritdoc />
        public bool SubmitUserUpdateToOnePlace(
            ref IOslerUserInfo user)
        {
            if (user?.UserInfo == null)
            {
                return false;
            }

            try
            {
                if (!ShouldUpdateOnePlace(user))
                {
                    return true;
                }

                var oldUser = _userRepository.GetById(user.UserID, true);

                // Create or update Account/Company object,
                // as we need Account ID reference to update the contact
                var account = GetAccount(user, oldUser);

                // If company name didn't change, we don't need to create a queue task for it
                DataSubmissionResult accountQueueTask = null;

                if (account != null)
                {
                    // If the company name DID change, then we need to clear Current Industry value,
                    // as it no will no longer reflect the correct company's Industry value
                    user.CurrentIndustry = null;

                    accountQueueTask =
                        _dataSubmissionQueueService.Upsert(user, account);
                }

                // Update Contact object
                var contact = GetContact(user, oldUser);

                int[] contactDependencies = null;

                if (accountQueueTask != null)
                {
                    contact.AccountId =
                        accountQueueTask.GetObjectId();

                    // Depends on the Account task being processed successfully
                    contactDependencies = new[]
                    {
                        accountQueueTask.TaskId
                    };
                }

                var contactQueueTask =
                    _dataSubmissionQueueService
                        .Update(
                            user,
                            user.OnePlaceReference,
                            contact,
                            contactDependencies);

                // Create or update board membership Account/Company objects
                var boardAccounts = GetBoardAccounts(user, oldUser);

                if (boardAccounts == null)
                {
                    return true;
                }

                foreach (var boardAccount in boardAccounts)
                {
                    var boardAccountQueueTask =
                        _dataSubmissionQueueService
                            .Upsert(
                                user,
                                boardAccount);

                    // Note that this is a relationship object between Account and Contact,
                    // as such it doesn't have any other fields that contain meaningful information
                    var boardMembership = new BoardMembership
                    {
                        FromContactId =
                            contactQueueTask.GetObjectId(),
                        ToAccountId =
                            boardAccountQueueTask.GetObjectId()
                    };

                    var boardMembershipQueueTask =
                        _dataSubmissionQueueService
                            .Upsert(
                                user,
                                boardMembership,
                                new[]
                                {
                                        contactQueueTask.TaskId,
                                        boardAccountQueueTask.TaskId
                                });
                }

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SubmitUserUpdateToOnePlace),
                    ex);

                return false;
            }
        }

        #endregion

        #region "Helper methods"

        protected bool ShouldUpdateOnePlace(
            IOslerUserInfo user)
        {
            if (user?.UserInfo == null)
            {
                return false;
            }

            // Make sure that this user update is not coming from a OnePlace import,
            // so that we don't end up with circular updates. Treat empty value as 
            // the absence of circular updates.
            if (user.UpdateOnePlace.HasValue && !user.UpdateOnePlace.Value)
            {
                return false;
            }

            // If the user is missing OnePlace reference, do not generate data submission tasks
            if (string.IsNullOrWhiteSpace(user.OnePlaceReference))
            {
                return false;
            }

            // Check if any of the OnePlace-sourced fields were modified.
            // Otherwise, there is no need to generate data submission tasks for OnePlace.
            return OnePlaceSourcedFields.Any(
                opField => user.ItemChanged(opField));
        }

        protected Account GetAccount(
            IOslerUserInfo user,
            IOslerUserInfo oldUser)
        {
            // If company name didn't change, we don't need to create a queue task for it
            if (string.Equals(
                    user.Company,
                    oldUser?.Company,
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            // Note that the following fields are read-only,
            // so we shouldn't be trying to pass that into OnePlace during updates:
            // - Industry
            return new Account
            {
                Name = user.Company.ReplaceIfEmpty(DataSubmissionConstants.SpecialCompanies.NoCompany)
            };
        }

        protected IList<Account> GetBoardAccounts(
            IOslerUserInfo user,
            IOslerUserInfo oldUser)
        {
            var boardMemberships = user.BoardMembershipsList;
            var oldBoardMemberships = oldUser?.BoardMembershipsList;

            if (oldBoardMemberships != null)
            {
                // Should only generate queued tasks for new board memberships
                boardMemberships = boardMemberships?
                    .Except(oldBoardMemberships, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }

            return boardMemberships?
                .Select(board => new Account
                {
                    Name = board
                })
                .ToList();
        }

        protected Contact GetContact(
            IOslerUserInfo user,
            IOslerUserInfo oldUser)
        {
            // Note (1) that the following fields are read-only,
            // so we shouldn't be trying to pass that into OnePlace during updates:
            // - Year of Call and Jurisdictions;
            // - Practice Areas while at Osler;
            // - Office Locations while at Osler.
            // 
            // Note (2) that we are currently always creating a contact task - 
            // this is due to the logic in there for looking up a new contact's
            // OnePlace reference, in the case of a merge. Would be nice to refactor
            // However, below we are nullifying any fields that did not change
            return new Contact
            {
                FirstName = user.FirstName
                    .ReplaceIfEqual(oldUser?.FirstName, null),
                LastName = user.LastName
                    .ReplaceIfEqual(oldUser?.LastName, null),
                Email = user.Email
                    .ReplaceIfEqual(oldUser?.Email, null),
                JobTitle = user.JobTitle
                    .ReplaceIfEqual(oldUser?.JobTitle, null),
                City = user.City
                    .ReplaceIfEqual(oldUser?.City, null),
                StateProvince = user.Province
                    .ReplaceIfEqual(oldUser?.Province, null),
                Country = user.Country
                    .ReplaceIfEqual(oldUser?.Country, null),
                SubscriptionPreferences = user.SubscriptionPreferences
                    .ReplaceIfEqual(oldUser?.SubscriptionPreferences, null),
                CommunicationPreferences = user.CommunicationPreferences
                    .ReplaceIfEqual(oldUser?.CommunicationPreferences, null),
            };
        }

        #endregion
    }
}

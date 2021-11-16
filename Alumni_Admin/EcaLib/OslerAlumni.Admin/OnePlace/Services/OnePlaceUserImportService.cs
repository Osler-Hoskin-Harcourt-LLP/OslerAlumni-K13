using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CMS.DataEngine;
using CMS.Helpers;
using ECA.Core.Extensions;
using ECA.Core.Repositories;
using ECA.Core.Services;
using OslerAlumni.Admin.Core.Repositories;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Helpers;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Services;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.Admin.OnePlace.Services
{
    public class OnePlaceUserImportService
        : ServiceBase, IOnePlaceUserImportService
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private readonly IAdminUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IOnePlaceFieldLocalizerService _onePlaceFieldLocalizerService;

        #endregion

        public OnePlaceUserImportService(
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository,
            IAdminUserRepository userRepository,
            IEmailService emailService,
            IOnePlaceFieldLocalizerService onePlaceFieldLocalizerService)
        {
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _onePlaceFieldLocalizerService = onePlaceFieldLocalizerService;
        }

        #region "Methods"

        public IOslerUserInfo ConvertToUser(
            Contact contact,
            IOslerUserInfo existingUser = null)
        {
            if (contact == null)
            {
                return existingUser;
            }

            var user = existingUser ?? new OslerUserInfo();

            user.OnePlaceReference = contact.Id;
            user.IsAlumni = contact.IsAlumni ?? false;

            // Personal
            // Some contacts don't have the first name, but have "Goes By" field value 
            user.FirstName = DataHelper.GetNotEmpty(contact.FirstName, contact.GoesBy);
            user.LastName = contact.LastName;

            user.Email = contact.Email;
            user.JobTitle = contact.JobTitle;

            user.Company = contact.Account?.Name?.Trim()
                .ReplaceIfEqual(DataSubmissionConstants.SpecialCompanies.NoCompany, null);

            user.City = contact.City;
            user.Province = contact.StateProvince;
            user.Country = contact.Country;

            user.YearsAndJurisdictions =
                ToYearsAndJurisdictionsResourceStrings(
                    contact.YearOfCall,
                    contact.Jurisdictions);

            user.CurrentIndustry =
                ToCurrentIndustryResourceString(
                    contact.Account?.Industry);

            // Osler information
            user.PracticeAreas =
                ToPracticeAreaResourceStrings(
                    contact.PracticeAreas);

            user.OfficeLocations =
                ToOfficeLocationsResourceStrings(
                    contact.OfficeLocations);

            // Board memberships are not localized
            if (DataHelper.DataSourceIsEmpty(contact.BoardMembershipList))
            {
                user.BoardMemberships = string.Empty;
            }
            else
            {
                user.BoardMemberships = string.Join(
                    ";",
                    contact.BoardMembershipList
                        .Select(bm => bm.ToAccount?.Name)
                        .ToList());
            }

            user.IsCompetitor = contact?.IsCompetitor ?? false;

            user.UnsubscribeAllCommunications = contact?.UnsubscribeCommunications ?? false;

            user.SubscriptionPreferences = contact?.SubscriptionPreferences;

            user.CommunicationPreferences = contact?.CommunicationPreferences;

            user.StartDateAtOsler = contact?.StartDateAtOsler;

            user.EndDateAtOsler = contact?.EndDateAtOsler;

            user.EducationOverview = contact?.EducationOverview;

            return user;
        }

        public string GetAvailableUserName(
            IOslerUserInfo user)
        {
            if ((user == null)
                || string.IsNullOrWhiteSpace(user.LastName))
            {
                return null;
            }

            var baseUserName = user.LastName;

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                baseUserName = user.FirstName.Trim()[0] + baseUserName;
            }

            baseUserName =
                _userRepository.SanitizeUserName(baseUserName);

            var userName = baseUserName;
            var increment = 0;

            while (_userRepository.GetUserByUserName(userName) != null)
            {
                increment++;

                userName = baseUserName + increment;
            }

            return userName;
        }

        public bool ImportAsUser(
            Contact contact,
            out ImportAction importAction)
        {
            importAction = ImportAction.None;

            if (contact == null)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(ImportAsUser),
                    "Tried to import an empty OnePlace contact record.");

                return false;
            }

            try
            {
                IOslerUserInfo user;

                var isNew = !TryGetMappedUser(contact, out user) || (user == null);

                // For existing users, check if Kentico record was updated at a later time
                // than OnePlace record, to account for cases where Kentico changes haven't
                // yet made their way to OnePlace. This should prevent out-of-date information
                // from OnePlace from overwriting a more up-to-date information in Kentico.
                if (!isNew && IsOnePlaceOutOfDate(contact, user))
                {
                    importAction = ImportAction.Skip;

                    _eventLogRepository.LogWarning(
                        GetType(),
                        nameof(ImportAsUser),
                        $"User {user.Email} was skipped because the last modified date ({user.UserLastModified}) is later than the OnePlace modified date ({contact.LastModifiedDate})");

                    return true;
                }

                user =
                    ConvertToUser(contact, user);

                if (isNew)
                {
                    importAction = ImportAction.Create;

                    user.Enabled = true;

                    // Generate new user name based on the format:
                    // <first letter of the FirstName><LastName><(optional)increment>
                    user.UserName =
                        GetAvailableUserName(user);

                    // Generate a temporary based on the site password policy
                    var tempPassword =
                        _userRepository.GenerateNewPassword();

                    _userRepository.SetTemporaryPassword(
                        user,
                        tempPassword);
                }
                else
                {
                    importAction = ImportAction.Update;

                    // Check for a custom flag, set by the scheduled task that disables Alumni accounts
                    // for users that should no longer have access to the portal, before potentially re-enabling the account.
                    // This will ensure that we re-enabling only accounts that were disabled based on rules. 
                    // If an account was disabled manually, we should not overwrite it.
                    if (!user.Enabled && user.IsDisabledAutomatically)
                    {
                        user.Enabled = true;
                    }

                    // Do not generate OnePlace data submission tasks for this user update
                    user.UpdateOnePlace = false;
                }

                _userRepository.Save(user);

                if (user.UserID < 1)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(ImportAsUser),
                        $"Something went wrong when creating or updating the user '{user.UserName}' ({user.FirstName} {user.LastName}).");

                    return false;
                }

                if (importAction == ImportAction.Create)
                {

                    // Send the email notification for the newly created users,
                    // so that they can log in using their credentials

                    var token = _userRepository
                        .GetPasswordResetToken(user.UserGUID);

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        _eventLogRepository.LogError(
                            GetType(),
                            nameof(ImportAsUser),
                            $"Something went wrong with creating login token for '{user.UserName}' ({user.FirstName} {user.LastName}).");

                        return false;
                    }

                    _emailService
                        .SendNewAlumniUserAccountNotificationEmail(
                            user,
                            token);
                }

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(ImportAsUser),
                    ex);

                return false;
            }
        }

        public bool TryGetMappedUser(
            Contact contact,
            out IOslerUserInfo user)
        {
            user = null;

            if (contact == null)
            {
                return false;
            }

            // First, try looking up the user with OnePlace reference matching the contact ID
            user =
                _userRepository.GetUserByOnePlaceReference(contact.Id);

            if (user != null)
            {
                return true;
            }

            // If user wasn't found, see if there is a user for one of the contacts
            // that got merged into the current contact. This is needed so that we don't create
            // duplicate users for the same contact
            foreach (var mergedContactId in contact.MergedContactIdList)
            {
                user =
                    _userRepository.GetUserByOnePlaceReference(mergedContactId);

                if (user != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool UpdateUserStatus(
            IOslerUserInfo user,
            IList<Contact> contacts,
            out AlumniChangeType changeType)
        {
            changeType = AlumniChangeType.None;

            if (user == null)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(UpdateUserStatus),
                    "Tried to update an empty user record.");

                return false;
            }

            if (user.UserID < 1)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(UpdateUserStatus),
                    "Tried to update a user record that does not exist in the database yet, i.e. that is missing a valid UserID.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(user.OnePlaceReference))
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(UpdateUserStatus),
                    "Tried to update a user record, that was not sourced from OnePlace, based on OnePlace information.");

                return false;
            }

            try
            {
                contacts = contacts ?? new List<Contact>();

                var contact = contacts
                    .FirstOrDefault(c =>
                        string.Equals(c.Id, user.OnePlaceReference, StringComparison.OrdinalIgnoreCase));

                if (contact == null)
                {
                    // If we weren't able to look up a contact with the same ID,
                    // we should look for a contact into which this contact might have been merged
                    contact = contacts
                        .FirstOrDefault(c =>
                            c.MergedContactIdList.Any(mergedId =>
                                string.Equals(mergedId, user.OnePlaceReference, StringComparison.OrdinalIgnoreCase)));
                }

                if (contact == null)
                {
                    changeType = AlumniChangeType.Deleted;
                }
                else if (contact.HasLeftOnBadTerms ?? false)
                {
                    changeType = AlumniChangeType.LeftOnBadTerms;
                }
                else if (!contact.IsAlumni ?? true)
                {
                    changeType = AlumniChangeType.Rehired;
                }
                else if (contact.IsDeceased)
                {
                    changeType = AlumniChangeType.Deceased;
                }

                if ((changeType == AlumniChangeType.None)
                    || !user.Enabled)
                {
                    return true;
                }

                user.Enabled = false;
                user.IsDisabledAutomatically = true;

                // Do not generate OnePlace data submission tasks for this user update
                user.UpdateOnePlace = false;

                _userRepository.Save(user);

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(UpdateUserStatus),
                    ex);

                return false;
            }
        }

        #endregion

        #region "Helper methods"

        protected bool IsOnePlaceOutOfDate(
            Contact contact,
            IOslerUserInfo user)
        {
            var updateTimeMargin = _settingsKeyRepository.GetValue<int>(
                GlobalConstants.Settings.OnePlace.UpdateTimeMargin);

            if ((user == null) || (contact == null))
            {
                return false;
            }

            // Update time margin allows for a specific length of time that it might have taken
            // for the Kentico update to go through to OnePlace:
            // - If there were any Kentico updates made during this short length of time, OnePlace
            // data shouldn't be overwriting them, even though the Kentico Last Modified Date will
            // be earlier than the OnePlace Last Modified Date of the contact in this case;
            // - Similarly, if there were any OnePlace updates made during this time, they would be
            // ignored as they would be considered to be the result of the Kentico submission.
            return user.UserLastModified.IsLaterThan(
                contact.LastModifiedDate,
                -updateTimeMargin);
        }

        private string ToCurrentIndustryResourceString(
            string industry)
        {
            return _onePlaceFieldLocalizerService
                .GetResourceStringCodeNameForCurrentIndustry(industry);
        }

        private string ToPracticeAreaResourceStrings(
            string practiceAreas)
        {
            var practiceAreaResStrings = practiceAreas?.SplitOn(';')?
                .Select(area => _onePlaceFieldLocalizerService
                    .GetResourceStringCodeNameForPracticeArea(area));

            return practiceAreaResStrings?.Join(";");
        }

        private string ToOfficeLocationsResourceStrings(
            string offices)
        {
            var officesResStrings = offices?.SplitOn(';')?
                .Select(loc => _onePlaceFieldLocalizerService
                    .GetResourceStringCodeNameForOfficeLocation(loc));

            return officesResStrings?.Join(";");
        }

        private string ToYearsAndJurisdictionsResourceStrings(
            DateTime? yearOfCall,
            string jurisdictions)
        {
            // Add the year to the first one that does not have year in it
            string yearsAndJurisdictionsStr = jurisdictions;
            if (yearOfCall.HasValue)
            {
                List<string> temp = jurisdictions.SplitOn(';', ',').Select(item => item.Trim()).ToList();
                for(int i = 0; i < temp.Count; i++)
                {
                    var regex1 = new Regex(GlobalConstants.RegexExpressions.YearAndJurisdictionRegex);
                    var regex2 = new Regex(GlobalConstants.RegexExpressions.YearAndJurisdictionReversedRegex);
                    if (!regex1.IsMatch(temp[i]) && !regex2.IsMatch(temp[i]))
                    {
                        temp[i] = $"{yearOfCall.Value.Year} {temp[i]}";
                    }
                }

                yearsAndJurisdictionsStr = string.Join(";", temp);
            }

            var yearsAndJurisdictionsList = UserProfileMappingHelper.YearOfCallAndJurisdictionsList(yearsAndJurisdictionsStr);

            yearsAndJurisdictionsList
                ?.ForEach(item =>
                {
                    item.Jurisdiction =
                        _onePlaceFieldLocalizerService.GetResourceStringCodeNameForJurisdiction(
                            item.Jurisdiction);
                });

            var yearsAndJurisdictionsStrList = yearsAndJurisdictionsList
                ?.Select(yj => $"{yj.Year} {yj.Jurisdiction}")
                .ToList();

            return yearsAndJurisdictionsStrList
                ?.Join(";");
        }

        #endregion
    }
}

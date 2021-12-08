using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using ECA.Core.Extensions;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Helpers;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Kentico.Models
{
    /// <summary>
    /// OslerUserInfo will contain extra custom fields that UserInfo won't.
    /// </summary>
    public class OslerUserInfo
        : IOslerUserInfo
    {
        #region "Properties"

        public UserInfo UserInfo { get; }

        public string OnePlaceReference
        {
            get { return UserInfo.GetValue(nameof(OnePlaceReference), string.Empty); }
            set { UserInfo.SetValue(nameof(OnePlaceReference), value); }
        }

        public bool IsAlumni
        {
            get { return UserInfo.GetBooleanValue(nameof(IsAlumni), false); }
            set { UserInfo.SetValue(nameof(IsAlumni), value); }
        }

        public bool IsDisabledAutomatically
        {
            get { return UserInfo.GetBooleanValue(nameof(IsDisabledAutomatically), false); }
            set { UserInfo.SetValue(nameof(IsDisabledAutomatically), value); }
        }

        public string AlumniEmail
        {
            get { return UserInfo.GetValue(nameof(AlumniEmail), string.Empty); }
            set { UserInfo.SetValue(nameof(AlumniEmail), value); }
        }

        public DateTime? StartDateAtOsler
        {
            get
            {
                var dateTime = UserInfo.GetDateTimeValue(nameof(StartDateAtOsler), DateTime.MinValue);

                if (dateTime == DateTime.MinValue)
                {
                    return null;
                }

                return dateTime;
            }
            set { UserInfo.SetValue(nameof(StartDateAtOsler), value); }
        }

        public DateTime? EndDateAtOsler
        {
            get
            {
                var dateTime = UserInfo.GetDateTimeValue(nameof(EndDateAtOsler), DateTime.MinValue);

                if (dateTime == DateTime.MinValue)
                {
                    return null;
                }

                return dateTime;
            }
            set { UserInfo.SetValue(nameof(EndDateAtOsler), value); }
        }

        public string ProfileImage
        {
            get { return UserInfo.GetValue(nameof(ProfileImage), string.Empty); }
            set { UserInfo.SetValue(nameof(ProfileImage), value); }
        }

        public string Company
        {
            get { return UserInfo.GetValue(nameof(Company), string.Empty); }
            set { UserInfo.SetValue(nameof(Company), value); }
        }

        public string JobTitle
        {
            get { return UserInfo.GetValue(nameof(JobTitle), string.Empty); }
            set { UserInfo.SetValue(nameof(JobTitle), value); }
        }

        public string Country
        {
            get { return UserInfo.GetValue(nameof(Country), string.Empty); }
            set { UserInfo.SetValue(nameof(Country), value); }
        }

        public string Province
        {
            get { return UserInfo.GetValue(nameof(Province), string.Empty); }
            set { UserInfo.SetValue(nameof(Province), value); }
        }

        public string City
        {
            get { return UserInfo.GetValue(nameof(City), string.Empty); }
            set { UserInfo.SetValue(nameof(City), value); }
        }

        public string YearsAndJurisdictions
        {
            get { return UserInfo.GetValue(nameof(YearsAndJurisdictions), string.Empty); }
            set { UserInfo.SetValue(nameof(YearsAndJurisdictions), value); }
        }

        public string CurrentIndustry
        {
            get { return UserInfo.GetValue(nameof(CurrentIndustry), string.Empty); }
            set { UserInfo.SetValue(nameof(CurrentIndustry), value); }
        }

        public string LinkedInUrl
        {
            get { return UserInfo.GetValue(nameof(LinkedInUrl), string.Empty); }
            set { UserInfo.SetValue(nameof(LinkedInUrl), value); }
        }

        public string TwitterUrl
        {
            get { return UserInfo.GetValue(nameof(TwitterUrl), string.Empty); }
            set { UserInfo.SetValue(nameof(TwitterUrl), value); }
        }

        public string InstagramUrl
        {
            get { return UserInfo.GetValue(nameof(InstagramUrl), string.Empty); }
            set { UserInfo.SetValue(nameof(InstagramUrl), value); }
        }

        public bool IncludeEmailInDirectory
        {
            get { return UserInfo.GetBooleanValue(nameof(IncludeEmailInDirectory), false); }
            set { UserInfo.SetValue(nameof(IncludeEmailInDirectory), value); }
        }

        public bool DisplayImageInDirectory
        {
            get { return UserInfo.GetBooleanValue(nameof(DisplayImageInDirectory), false); }
            set { UserInfo.SetValue(nameof(DisplayImageInDirectory), value); }
        }


        //TODO: If Osler Asks for more email preferences in the future then we should implement this like we do on osler.com
        //      For now since there is only 1, it's fine.
        public bool SubscribeToEmailUpdates
        {
            get
            {
                return !UnsubscribeAllCommunications &&
                       SubscriptionPreferences
                           .Contains(GlobalConstants.OnePlace.EmailPreferences.Subscribe.AlumniCommunications) &&
                       !CommunicationPreferences
                           .Contains(GlobalConstants.OnePlace.EmailPreferences.Unsubscribe.AlumniCommunications);
            }
            set
            {
                if (value)
                {
                    if (!SubscriptionPreferences.Contains(GlobalConstants.OnePlace.EmailPreferences.Subscribe
                        .AlumniCommunications))
                    {
                        SubscriptionPreferences = string.Join(";",
                            new[]
                            {
                                SubscriptionPreferences,
                                GlobalConstants.OnePlace.EmailPreferences.Subscribe.AlumniCommunications
                            });
                    }

                    if (CommunicationPreferences.Contains(GlobalConstants.OnePlace.EmailPreferences.Unsubscribe
                        .AlumniCommunications))
                    {
                        CommunicationPreferences = CommunicationPreferences.Split(';').Where(cp =>
                            !string.Equals(cp,
                                GlobalConstants.OnePlace.EmailPreferences.Unsubscribe.AlumniCommunications)).Join(";");
                    }
                }
                else
                {
                    if (!CommunicationPreferences.Contains(GlobalConstants.OnePlace.EmailPreferences.Unsubscribe
                        .AlumniCommunications))
                    {
                        CommunicationPreferences = string.Join(";",
                            new[]
                            {
                                CommunicationPreferences,
                                GlobalConstants.OnePlace.EmailPreferences.Unsubscribe
                                    .AlumniCommunications
                            });
                    }

                    if (SubscriptionPreferences.Contains(GlobalConstants.OnePlace.EmailPreferences.Subscribe
                        .AlumniCommunications))
                    {
                        SubscriptionPreferences = SubscriptionPreferences.Split(';').Where(cp =>
                            !string.Equals(cp,
                                GlobalConstants.OnePlace.EmailPreferences.Subscribe.AlumniCommunications)).Join(";");
                    }
                }
            }
        }

        public bool UnsubscribeAllCommunications {
            get { return UserInfo.GetValue(nameof(UnsubscribeAllCommunications), false); }
            set { UserInfo.SetValue(nameof(UnsubscribeAllCommunications), value); }
        }

        public string SubscriptionPreferences {
            get { return UserInfo.GetValue(nameof(SubscriptionPreferences), string.Empty); }
            set { UserInfo.SetValue(nameof(SubscriptionPreferences), value); }
        }

        public string CommunicationPreferences
        {
            get { return UserInfo.GetValue(nameof(CommunicationPreferences), string.Empty); }
            set { UserInfo.SetValue(nameof(CommunicationPreferences), value); }
        }

        public string PracticeAreas
        {
            get { return UserInfo.GetValue(nameof(PracticeAreas), string.Empty); }
            set { UserInfo.SetValue(nameof(PracticeAreas), value); }
        }

        public string OfficeLocations
        {
            get { return UserInfo.GetValue(nameof(OfficeLocations), string.Empty); }
            set { UserInfo.SetValue(nameof(OfficeLocations), value); }
        }

        [MaxLength(1000)]
        public string BoardMemberships
        {
            get { return UserInfo.GetValue(nameof(BoardMemberships), string.Empty); }
            set { UserInfo.SetValue(nameof(BoardMemberships), value); }
        }


        public List<string> BoardMembershipsList
        {
            get { return BoardMemberships.SplitOn(';'); }
            set { BoardMemberships = string.Join(";", value); }
        }

        public List<string> OfficeLocationsList
        {
            get { return OfficeLocations.SplitOn(';'); }
            set { OfficeLocations = string.Join(";", value); }
        }

        public List<string> PracticeAreasList
        {
            get { return PracticeAreas.SplitOn(';'); }
            set { PracticeAreas = string.Join(";", value); }
        }

        public List<YearAndJurisdiction> YearOfCallAndJurisdictionsList
        {
            get
            {
                return UserProfileMappingHelper.YearOfCallAndJurisdictionsList(YearsAndJurisdictions);
            }
            set { }
        }

        public bool? UpdateOnePlace
        {
            get
            {
                object updateOnePlace;

                if (!UserInfo.UserCustomData
                        .TryGetValue("UpdateOnePlace", out updateOnePlace)
                    || (updateOnePlace == null))
                {
                    return null;
                }

                return ValidationHelper.GetBoolean(updateOnePlace, true);
            }
            set
            {
                UserInfo.UserCustomData
                    .SetValue("UpdateOnePlace", value);
            }
        }

        public bool IsCompetitor
        {
            get { return UserInfo.GetBooleanValue(nameof(IsCompetitor), false); }
            set { UserInfo.SetValue(nameof(IsCompetitor), value); }
        }

        public string EducationOverview  
        {
            get { return UserInfo.GetValue(nameof(EducationOverview), string.Empty); }
            set { UserInfo.SetValue(nameof(EducationOverview), value); }
        }
        public List<EducationRecord> EducationOverviewList => UserProfileMappingHelper.ToEducationHistory(EducationOverview);

        #endregion

        public OslerUserInfo(
            UserInfo userInfo = null)
        {
            UserInfo = userInfo ?? new UserInfo();
        }

        #region "Conversion operators"

        /// <summary>
        /// Supports explicit casting to UserInfo.
        /// </summary>
        /// <param name="user"></param>
        public static explicit operator UserInfo(
            OslerUserInfo user)
        {
            return user?.UserInfo;
        }

        #endregion

        #region "Methods"


        /// <inheritdoc />
        public void AutopopulateDependantFields()
        {
            // Kentico doesn't update the full name when first and last name are updated
            if (!string.IsNullOrWhiteSpace(FirstName)
                || !string.IsNullOrWhiteSpace(LastName))
            {
                FullName = string.Join(
                    " ",
                    FirstName,
                    LastName);
            }
        }

        /// <inheritdoc />
        public bool IsAlumniUser(
            string siteName)
        {
            return !string.IsNullOrEmpty(OnePlaceReference)
                   && IsAlumni
                   && (string.IsNullOrEmpty(siteName) || UserInfo.IsInSite(siteName));
        }

        #endregion

        #region "UserInfo properties"

        public List<string> ColumnNames => UserInfo.ColumnNames;

        public SafeDictionary<string, SafeDictionary<string, int?>> SitesRoles => UserInfo.SitesRoles;

        public bool UserIsHidden
        {
            get { return UserInfo.UserIsHidden; }
            set { UserInfo.UserIsHidden = value; }
        }

        public string LastName
        {
            get { return UserInfo.LastName; }
            set { UserInfo.LastName = value; }
        }

        public string FullName
        {
            get { return UserInfo.FullName; }
            set { UserInfo.FullName = value; }
        }

        public DateTime LastLogon
        {
            get { return UserInfo.LastLogon; }
            set { UserInfo.LastLogon = value; }
        }

        public string PreferredCultureCode
        {
            get { return UserInfo.PreferredCultureCode; }
            set { UserInfo.PreferredCultureCode = value; }
        }

        public string MiddleName
        {
            get { return UserInfo.MiddleName; }
            set { UserInfo.MiddleName = value; }
        }

        public string PreferredUICultureCode
        {
            get { return UserInfo.PreferredUICultureCode; }
            set { UserInfo.PreferredUICultureCode = value; }
        }

        public bool UserEnabled
        {
            get { return UserInfo.UserEnabled; }
            set { UserInfo.UserEnabled = value; }
        }

        public string FirstName
        {
            get { return UserInfo.FirstName; }
            set { UserInfo.FirstName = value; }
        }

        public DateTime UserCreated
        {
            get { return UserInfo.UserCreated; }
            set { UserInfo.UserCreated = value; }
        }

        public int UserID
        {
            get { return UserInfo.UserID; }
            set { UserInfo.UserID = value; }
        }

        public string UserPasswordFormat
        {
            get { return UserInfo.UserPasswordFormat; }
            set { UserInfo.UserPasswordFormat = value; }
        }

        public string UserName
        {
            get { return UserInfo.UserName; }
            set { UserInfo.UserName = value; }
        }

        public string Email
        {
            get { return UserInfo.Email; }
            set { UserInfo.Email = value; }
        }

        public bool Enabled
        {
            get { return UserInfo.Enabled; }
            set { UserInfo.Enabled = value; }
        }

        public string PasswordFormat
        {
            get { return UserInfo.PasswordFormat; }
            set { UserInfo.PasswordFormat = value; }
        }

        public string UserStartingAliasPath
        {
            get { return UserInfo.UserStartingAliasPath; }
            set { UserInfo.UserStartingAliasPath = value; }
        }

        public bool UserHasAllowedCultures
        {
            get { return UserInfo.UserHasAllowedCultures; }
            set { UserInfo.UserHasAllowedCultures = value; }
        }

        public Guid UserGUID
        {
            get { return UserInfo.UserGUID; }
            set { UserInfo.UserGUID = value; }
        }

        public DateTime UserLastModified
        {
            get { return UserInfo.UserLastModified; }
            set { UserInfo.UserLastModified = value; }
        }

        // TODO##
        //public string UserVisibility
        //{
        //    get { return UserInfo.UserVisibility; }
        //    set { UserInfo.UserVisibility = value; }
        //}

        public bool UserIsDomain
        {
            get { return UserInfo.UserIsDomain; }
            set { UserInfo.UserIsDomain = value; }
        }

        public Guid UserAuthenticationGUID
        {
            get { return UserInfo.UserAuthenticationGUID; }
            set { UserInfo.UserAuthenticationGUID = value; }
        }

        public bool UserIsExternal
        {
            get { return UserInfo.UserIsExternal; }
            set { UserInfo.UserIsExternal = value; }
        }

        // TODO##
        //public string UserPicture
        //{
        //    get { return UserInfo.UserPicture; }
        //    set { UserInfo.UserPicture = value; }
        //}

        public int UserAvatarID
        {
            get { return UserInfo.UserAvatarID; }
            set { UserInfo.UserAvatarID = value; }
        }

        public string UserSignature
        {
            get { return UserInfo.UserSignature; }
            set { UserInfo.UserSignature = value; }
        }

        public string UserDescription
        {
            get { return UserInfo.UserDescription; }
            set { UserInfo.UserDescription = value; }
        }

        public string UserNickName
        {
            get { return UserInfo.UserNickName; }
            set { UserInfo.UserNickName = value; }
        }

        public string UserURLReferrer
        {
            get { return UserInfo.UserURLReferrer; }
            set { UserInfo.UserURLReferrer = value; }
        }

        public string UserCampaign
        {
            get { return UserInfo.UserCampaign; }
            set { UserInfo.UserCampaign = value; }
        }

        public int UserTimeZoneID
        {
            get { return UserInfo.UserTimeZoneID; }
            set { UserInfo.UserTimeZoneID = value; }
        }

        #endregion

        #region "UserInfo methods"

        public object GetValue(string columnName)
        {
            return UserInfo.GetValue(columnName);
        }

        public bool SetValue(string columnName, object value)
        {
            return UserInfo.SetValue(columnName, value);
        }

        public object this[string columnName]
        {
            get { return UserInfo[columnName]; }
            set { UserInfo[columnName] = value; }
        }

        public bool TryGetValue(string columnName, out object value)
        {
            return UserInfo.TryGetValue(columnName, out value);
        }

        public bool ContainsColumn(string columnName)
        {
            return UserInfo.ContainsColumn(columnName);
        }

        public bool IsPublic()
        {
            return UserInfo.IsPublic();
        }

        public bool IsAuthorizedPerResource(
            string resourceName,
            string permissionName,
            string siteName,
            bool exceptionOnFailure)
        {
            return UserInfo.IsAuthorizedPerResource(resourceName, permissionName, siteName, exceptionOnFailure);
        }

        public bool IsAuthorizedPerClassName(
            string className,
            string permissionName,
            string siteName,
            bool exceptionOnFailure)
        {
            return UserInfo.IsAuthorizedPerClassName(className, permissionName, siteName, exceptionOnFailure);
        }

        public void FilterSearchResults(
            List<string> inRoles,
            List<string> notInRoles,
            ref bool addToIndex)
        {
            UserInfo.FilterSearchResults(inRoles, notInRoles, ref addToIndex);
        }

        public bool CheckPrivilegeLevel(
            UserPrivilegeLevelEnum privilegeLevel,
            string siteName = null)
        {
            return UserInfo.CheckPrivilegeLevel(privilegeLevel, siteName);
        }

        #endregion

        #region "IInfo properties"

        public bool HasChanged
            => UserInfo.HasChanged;

        public bool IsComplete
            => UserInfo.IsComplete;

        public List<string> Properties
            => UserInfo.Properties;

        public object RelatedData
        {
            get { return UserInfo.RelatedData; }
            set { UserInfo.RelatedData = value; }
        }

        public ObjectSettingsInfo ObjectSettings
            => UserInfo.ObjectSettings;

        public ObjectTypeInfo TypeInfo
            => UserInfo.TypeInfo;

        public GeneralizedInfo Generalized
            => UserInfo.Generalized;

        public bool AllowPartialUpdate
        {
            get { return UserInfo.AllowPartialUpdate; }
            set { UserInfo.AllowPartialUpdate = value; }
        }

        #endregion

        #region "IInfo methods"

        public void RevertChanges()
        {
            UserInfo.RevertChanges();
        }

        public void ResetChanges()
        {
            UserInfo.ResetChanges();
        }

        public object GetOriginalValue(
            string columnName)
        {
            return UserInfo.GetOriginalValue(
                columnName);
        }

        public bool ItemChanged(
            string columnName)
        {
            return UserInfo.ItemChanged(
                columnName);
        }

        public List<string> ChangedColumns()
        {
            return UserInfo.ChangedColumns();
        }

        public void MakeComplete(
            bool loadFromDb)
        {
            UserInfo.MakeComplete(
                loadFromDb);
        }

        public bool DataChanged(
            string excludedColumns)
        {
            return UserInfo.DataChanged(
                excludedColumns);
        }

        public void GetObjectData(
            SerializationInfo info,
            StreamingContext context)
        {
            UserInfo.GetObjectData(
                info,
                context);
        }

        public object GetProperty(
            string columnName)
        {
            return UserInfo.GetProperty(
                columnName);
        }

        public bool TryGetProperty(
            string columnName,
            out object value)
        {
            return UserInfo.TryGetProperty(
                columnName,
                out value);
        }

        public bool TryGetProperty(
            string columnName,
            out object value,
            bool notNull)
        {
            return UserInfo.TryGetProperty(
                columnName,
                out value,
                notNull);
        }

        public int CompareTo(
            object obj)
        {
            return UserInfo.CompareTo(obj);
        }

        public string ToMacroString()
        {
            return UserInfo.ToMacroString();
        }

        public object MacroRepresentation()
        {
            return UserInfo.MacroRepresentation();
        }

        public void SubmitChanges(
            bool withCollections)
        {
            UserInfo.SubmitChanges(
                withCollections);
        }

        public void Update()
        {
            UserInfo.Update();
        }

        public bool Delete()
        {
            return UserInfo.Delete();
        }

        public bool Destroy()
        {
            return UserInfo.Destroy();
        }

        public void Insert()
        {
            UserInfo.Insert();
        }

        public void ExecuteWithOriginalData(
            Action action)
        {
            UserInfo.ExecuteWithOriginalData(
                action);
        }

        public BaseInfo CloneObject(
            bool clear)
        {
            return UserInfo.CloneObject(clear);
        }

        #endregion
    }
}

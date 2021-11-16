using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.PageURL.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Core.Services
{
    public class ProfileService
        : ServiceBase, IProfileService
    {
        #region "Private fields"

        private readonly ICacheService _cacheService;
        protected readonly IDocumentRepository _documentRepository;
        protected readonly IEventLogRepository _eventLogRepository;
        protected readonly ISettingsKeyRepository _settingsKeyRepository;
        protected readonly IUserRepository _userRepository;
        private readonly IPageUrlService _pageUrlService;

        protected readonly ContextConfig _context;

        #endregion

        public ProfileService(
            ICacheService cacheService,
            IDocumentRepository documentRepository,
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository,
            IUserRepository userRepository,
            IPageUrlService pageUrlService,
            ContextConfig context)
        {
            _cacheService = cacheService;
            _documentRepository = documentRepository;
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;
            _userRepository = userRepository;
            _pageUrlService = pageUrlService;

            _context = context;
        }

        #region "Methods"

        public bool CreateProfile(
            IOslerUserInfo user)
        {
            try
            {
                if (user == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(CreateProfile),
                        "User object was not provided.");

                    return false;
                }

                if (!user.IsAlumniUser(_context.Site?.SiteName))
                {
                    // Don't need to log an error - there are non-Alumni users in the system
                    return false;
                }

                // Need a last name in order to place the user under the correct folder
                if (string.IsNullOrWhiteSpace(user.LastName))
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(CreateProfile),
                        $"User '{user.UserName}' is missing a last name, which is necessary for the creation of a dedicated folder based on the first letter of the last name.");

                    return false;
                }

                // Check if a profile for this user already exists. If so then we don't need to create it
                var profile = _documentRepository
                    .GetDocuments(new WhereCondition()
                            .WhereEquals(nameof(PageType_Profile.UserGuid), user.UserGUID)
                            .ToString(true),
                        PageType_Profile.CLASS_NAME)
                    .FirstOrDefault();

                if (profile != null)
                {
                    // The profile already exists
                    return true;
                }

                var systemUser = _userRepository.GetSystemUser()?.UserInfo;

                if (systemUser == null)
                {
                    // Assume the error was handled in the repo
                    return false;
                }

                if (!_documentRepository.InitilizeDocument(
                    PageType_Profile.CLASS_NAME,
                    systemUser,
                    out profile))
                {
                    // Assume the error was handled in the repo
                    return false;
                }

                if (profile == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(CreateProfile),
                        "Missing profile page object.");

                    return false;
                }

                profile.DocumentCulture = GlobalConstants.Cultures.English;

                profile.SetValue(
                    nameof(PageType_Profile.UserGuid),
                    user.UserGUID);

                profile.SetValue(
                    nameof(PageType_Profile.DefaultController),
                    _settingsKeyRepository.GetValue<string>(
                        GlobalConstants.Settings.DefaultProfilesController));

                profile.SetValue(
                    nameof(PageType_Profile.DefaultAction),
                    _settingsKeyRepository.GetValue<string>(
                        GlobalConstants.Settings.DefaultProfilesAction));

                SetProfileFields(profile, user);

                var folderName = user.LastName.Substring(0, 1).ToUpper();

                var folder = CreateFolderIfNotExists(folderName);

                if (folder == null)
                {
                    // Assume error was handled in the repo or helper method
                    return false;
                }

                // Assume error was handled in the repo or helper method
                return CreatePublishedDocumentInBothCultures(profile, folder);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(CreateProfile),
                    ex);

                return false;
            }
        }

        private TreeNode CreateFolderIfNotExists(string folderName)
        {
            // Create a profile page in the correct place. The parent directory landing page is stored in a custom setting
            var directoryLandingPage = GetDirectoryLandingPage();

            if (directoryLandingPage == null)
            {
                // Assume error was handled in the helper method
                return null;
            }

            // Check if there is a folder already created that matches the first initial of this user's last name.
            // If not, then create it in both cultures
            var folder = _documentRepository
                             .GetDocuments(new WhereCondition()
                                     .WhereEquals(nameof(Folder.DocumentName), folderName)
                                     .ToString(true),
                                 Folder.CLASS_NAME,
                                 null,
                                 $"{directoryLandingPage.NodeAliasPath}/%")
                             .FirstOrDefault()
                         ??
                         CreateFolder(folderName, directoryLandingPage);

            return folder;
        }

        public bool UpdateProfile(
            IOslerUserInfo user)
        {
            try
            {
                if (user == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(UpdateProfile),
                        "User object was not provided.");

                    return false;
                }

                if (!user.IsAlumniUser(_context.Site?.SiteName))
                {
                    // Don't need to log an error - there are non-Alumni users in the system
                    return false;
                }

                var profiles = _documentRepository.GetDocuments(
                        new WhereCondition()
                            .WhereEquals(nameof(PageType_Profile.UserGuid), user.UserGUID)
                            .ToString(true),
                        PageType_Profile.CLASS_NAME,
                        new List<string>
                        {
                        GlobalConstants.Cultures.English,
                        GlobalConstants.Cultures.French
                        });

                if (DataHelper.DataSourceIsEmpty(profiles))
                {
                    // If a profile does not exist for the user then we should create it
                    // Assume the error was handled in the other method
                    return CreateProfile(user);
                }

                //Need this user to create documents
                var systemUser = _userRepository.GetSystemUser()?.UserInfo;

                if (systemUser == null)
                {
                    // Assume the error was handled in the repo
                    return false;
                }

                foreach (var profile in profiles)
                {
                    if (!ProfileFieldsHaveChanged(user, profile))
                    {
                        continue;
                    }

                    if (LastNameFirstCharacterHasChanged(user, profile))
                    {
                        var folderName = user.LastName.Substring(0, 1).ToUpper();
                        var folder = CreateFolderIfNotExists(folderName);

                        if (folder == null)
                        {
                            return false;
                        }

                        profile.SetValue(nameof(PageType_Profile.NodeParentID), folder.NodeID);
                    }

                    bool workflowEnabled;

                    if (!_documentRepository.IsWorkflowEnabled(
                            profile,
                            systemUser,
                            out workflowEnabled))
                    {
                        // Assume the error was handled in the repo
                        return false;
                    }

                    if (workflowEnabled)
                    {
                        if (!_documentRepository.CreateNewVersion(
                                profile,
                                systemUser))
                        {
                            // Assume the error was handled in the repo
                            return false;
                        }

                        SetProfileFields(profile, user);

                        if (!_documentRepository.UpdateDocument(profile))
                        {
                            // Assume the error was handled in the repo
                            return false;
                        }

                        if (!user.UserEnabled)
                        {
                            if (!profile.IsArchived)
                            {
                                if (!_documentRepository.ArchiveDocument(profile))
                                {
                                    // Assume the error was handled in the repo
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (!_documentRepository.PublishDocument(profile))
                            {
                                // Assume the error was handled in the repo
                                return false;
                            }
                        }
                    }
                    else
                    {
                        SetProfileFields(profile, user);

                        if (!_documentRepository.UpdateDocument(profile))
                        {
                            // Assume the error was handled in the repo
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(UpdateProfile),
                    ex);

                return false;
            }
        }

        public bool DeleteProfile(
            IOslerUserInfo user)
        {
            try
            {
                if (user == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(DeleteProfile),
                        "User object was not provided.");

                    return false;
                }

                // Pass null as the site name, so that site check is omitted.
                // User - Site relationship would already have been removed by this point.
                if (!user.IsAlumniUser(null))
                {
                    // Don't need to log an error - there are non-Alumni users in the system
                    return false;
                }

                // Find the profile page mapped to this user and delete it
                var profile = _documentRepository
                    .GetDocuments(
                        new WhereCondition()
                            .WhereEquals(nameof(PageType_Profile.UserGuid), user.UserGUID)
                            .ToString(true),
                        PageType_Profile.CLASS_NAME)
                    .FirstOrDefault();

                // Assume error was handled in the repo
                return _documentRepository.DeleteAllCultures(profile);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(DeleteProfile),
                    ex);

                return false;
            }
        }

        public PageType_Profile GetUserProfile(Guid userGuid)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Prefix +
                           $"{nameof(ProfileService)}|{nameof(GetUserProfile)}|{userGuid.ToString()}",
                AllowNullValue = false,
                CacheDependencies = new List<string>()
                {
                    string.Format(GlobalConstants.Caching.Pages.PagesByType, _context.Site.SiteName,
                        PageType_Profile.CLASS_NAME)
                }
            };

            var response = _cacheService.Get(
                cp =>
                {
                    var profile = (PageType_Profile)_documentRepository
                   .GetDocuments(
                       new WhereCondition()
                           .WhereEquals(nameof(PageType_Profile.UserGuid), userGuid)
                           .ToString(true),
                       PageType_Profile.CLASS_NAME)
                   .FirstOrDefault();

                    return profile;

                }, cacheParameters);

            return response;
        }

        /// <inheritdoc />
        public Dictionary<string, string> GetProfileUrls(string culture)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Prefix +
                           $"{nameof(ProfileService)}|{nameof(GetProfileUrls)}|{culture}",
                AllowNullValue = false,
                CultureCode = culture,
                CacheDependencies = new List<string>()
                {
                    string.Format(GlobalConstants.Caching.Pages.PagesByType, _context.Site.SiteName,
                        PageType_Profile.CLASS_NAME)
                }
            };

            var response = _cacheService.Get(
                cp =>
                {
                    var profiles = _documentRepository
                        .GetDocuments(
                            PageType_Profile.CLASS_NAME,
                            columnNames: new List<string>()
                            {
                                nameof(PageType_Profile.NodeGUID),
                                nameof(PageType_Profile.OnePlaceReference),
                            },
                            cultureName: culture,
                            whereCondition: new WhereCondition()
                                .WhereNotEmpty(nameof(PageType_Profile.OnePlaceReference)));

                    var result = new Dictionary<string, string>();


                    foreach (var profile in profiles)
                    {
                        try
                        {
                            string url;

                            if (_pageUrlService.TryGetPageMainUrl(profile.NodeGUID, culture, out url))
                            {
                                result.Add(((PageType_Profile) profile).OnePlaceReference, url);
                            }
                        }
                        catch (Exception e)
                        {
                            _eventLogRepository.LogError(GetType(), nameof(GetProfileUrls), e);
                        }
                    }


                    return result;

                }, cacheParameters);


            return response;
        }


        /// <inheritdoc />
        public string GetProfileUrl(string oneplaceReferenceId, string culture)
        {
            if (string.IsNullOrWhiteSpace(oneplaceReferenceId) || string.IsNullOrWhiteSpace(culture))
            {
                return null;
            }

            var dict = GetProfileUrls(culture);

            string url;

            dict.TryGetValue(oneplaceReferenceId, out url);

            return url;
        }
        #endregion

        #region "Helper methods"

        private TreeNode CreateFolder(
            string folderName,
            TreeNode parent)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(CreateFolder),
                    "Folder name was not provided.");

                return null;
            }

            if (parent == null)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(CreateFolder),
                    "Folder parent node was not provided.");

                return null;
            }

            var systemUser = _userRepository.GetSystemUser()?.UserInfo;

            if (systemUser == null)
            {
                // Assume the error was handled in the repo
                return null;
            }

            Folder folder;

            if (!_documentRepository.InitilizeDocument(
                    Folder.CLASS_NAME,
                    systemUser,
                    out folder))
            {
                // Assume error was handled in the repo
                return null;
            }

            if (folder == null)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(CreateFolder),
                    $"Failed to create a folder '{folderName}' under '{parent.NodeAliasPath}' content tree location.");

                return null;
            }

            folder.DocumentName = folderName;
            folder.DocumentCulture = GlobalConstants.Cultures.English;

            if (!CreatePublishedDocumentInBothCultures(folder, parent))
            {
                // Assume error was handled in the helper method
                return null;
            }

            return folder;
        }

        private bool CreatePublishedDocumentInBothCultures(
            TreeNode document,
            TreeNode parent)
        {
            // Inserts the new page as a child of the parent
            if (!_documentRepository.CreateDocument(document, parent))
            {
                // Assume error was handled in the repo
                return false;
            }

            // Publish the English document
            if (!_documentRepository.PublishDocument(document))
            {
                // Assume error was handled in the repo
                return false;
            }

            // Create a new culture version of the document in French
            if (!_documentRepository.InsertCultureVersion(
                    document,
                    GlobalConstants.Cultures.French))
            {
                // Assume error was handled in the repo
                return false;
            }

            // Publish the French document
            if (!_documentRepository.PublishDocument(document))
            {
                // Assume error was handled in the repo
                return false;
            }

            return true;
        }

        private TreeNode GetDirectoryLandingPage()
        {
            var directoryLandingPageGuid = _settingsKeyRepository.GetValue<Guid>(
                GlobalConstants.Settings.DirectoryLandingPageGuid);

            TreeNode directoryLandingPage = null;

            if (directoryLandingPageGuid != Guid.Empty)
            {
                // GetDocument() won't work here as we need the entire tree node object in order to correctly
                // insert the new document under it
                directoryLandingPage = _documentRepository
                    .GetDocuments(new WhereCondition()
                            .WhereEquals(nameof(PageType_LandingPage.NodeGUID), directoryLandingPageGuid)
                            .ToString(true),
                        PageType_LandingPage.CLASS_NAME)
                    .FirstOrDefault();
            }

            if (directoryLandingPage == null)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetDirectoryLandingPage),
                    $"Directory landing page is either not set (site setting '{GlobalConstants.Settings.DirectoryLandingPageGuid}') or is missing.");
            }

            return directoryLandingPage;
        }

        private void SetProfileFields(
            TreeNode profile,
            IOslerUserInfo user)
        {
            profile.SetValue(nameof(PageType_Profile.OnePlaceReference), user.OnePlaceReference);
            profile.SetValue(nameof(PageType_Profile.DocumentName), user.FullName);
            profile.SetValue(nameof(PageType_Profile.PageName), user.FullName);
            profile.SetValue(nameof(PageType_Profile.Title), user.FullName);
            profile.SetValue(nameof(PageType_Profile.FirstName), user.FirstName);
            profile.SetValue(nameof(PageType_Profile.LastName), user.LastName);
            profile.SetValue(nameof(PageType_Profile.AlumniEmail), user.Email);
            profile.SetValue(nameof(PageType_Profile.City), user.City);
            profile.SetValue(nameof(PageType_Profile.Province), user.Province);
            profile.SetValue(nameof(PageType_Profile.Country), user.Country);
            profile.SetValue(nameof(PageType_Profile.JobTitle), user.JobTitle);
            profile.SetValue(nameof(PageType_Profile.ProfileCompany), user.Company);
            profile.SetValue(nameof(PageType_Profile.YearsAndJurisdictions), user.YearsAndJurisdictions);
            profile.SetValue(nameof(PageType_Profile.CurrentIndustry), user.CurrentIndustry);
            profile.SetValue(nameof(PageType_Profile.BoardMemberships), user.BoardMemberships);
            profile.SetValue(nameof(PageType_Profile.OfficeLocations), user.OfficeLocations);
            profile.SetValue(nameof(PageType_Profile.PracticeAreas), user.PracticeAreas);
            profile.SetValue(nameof(PageType_Profile.IncludeEmailInDirectory), user.IncludeEmailInDirectory);
            profile.SetValue(nameof(PageType_Profile.DisplayImageInDirectory), user.DisplayImageInDirectory);
            profile.SetValue(nameof(PageType_Profile.StartDateAtOsler), user.StartDateAtOsler);
            profile.SetValue(nameof(PageType_Profile.EndDateAtOsler), user.EndDateAtOsler);
            profile.SetValue(nameof(PageType_Profile.LinkedInUrl), user.LinkedInUrl);
            profile.SetValue(nameof(PageType_Profile.TwitterUrl), user.TwitterUrl);
            profile.SetValue(nameof(PageType_Profile.InstagramUrl), user.InstagramUrl);
            profile.SetValue(nameof(PageType_Profile.ProfileImage), user.ProfileImage);
            profile.SetValue(nameof(PageType_Profile.EducationOverview), user.EducationOverview);
        }

        private bool ProfileFieldsHaveChanged(
            IOslerUserInfo user,
            TreeNode profile)
        {
            return    !string.Equals(profile.GetValue(nameof(PageType_Profile.OnePlaceReference), string.Empty), user.OnePlaceReference)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.DocumentName), string.Empty), user.FullName)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.PageName), string.Empty), user.FullName)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.Title), string.Empty), user.FullName)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.FirstName), string.Empty), user.FirstName)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.LastName), string.Empty), user.LastName)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.AlumniEmail), string.Empty), user.Email)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.City), string.Empty), user.City)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.Province), string.Empty), user.Province)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.Country), string.Empty), user.Country)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.JobTitle), string.Empty), user.JobTitle)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.ProfileCompany), string.Empty), user.Company)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.YearsAndJurisdictions), string.Empty), user.YearsAndJurisdictions)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.CurrentIndustry), string.Empty), user.CurrentIndustry)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.BoardMemberships), string.Empty), user.BoardMemberships)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.OfficeLocations), string.Empty), user.OfficeLocations)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.PracticeAreas), string.Empty), user.PracticeAreas)
                   || (profile.GetBooleanValue(nameof(PageType_Profile.IncludeEmailInDirectory), false) != user.IncludeEmailInDirectory)
                   || (profile.GetBooleanValue(nameof(PageType_Profile.DisplayImageInDirectory), false) != user.DisplayImageInDirectory)
                   || (profile.GetDateTimeValue(nameof(PageType_Profile.StartDateAtOsler), DateTime.MinValue) != user.StartDateAtOsler)
                   || (profile.GetDateTimeValue(nameof(PageType_Profile.EndDateAtOsler), DateTime.MinValue) != user.EndDateAtOsler)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.LinkedInUrl), string.Empty), user.LinkedInUrl)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.TwitterUrl), string.Empty), user.TwitterUrl)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.InstagramUrl), string.Empty), user.InstagramUrl)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.ProfileImage), string.Empty), user.ProfileImage)
                   || !string.Equals(profile.GetValue(nameof(PageType_Profile.EducationOverview), string.Empty), user.EducationOverview)
                   || profile.IsArchived == user.Enabled;
        }

        private bool LastNameFirstCharacterHasChanged(
            IOslerUserInfo user,
            TreeNode profile)
        {
            var profileLastName = profile.GetValue(nameof(PageType_Profile.LastName), string.Empty).Trim();

            return !string.Equals($"{profileLastName[0]}", $"{user.LastName.Trim()[0]}", StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}

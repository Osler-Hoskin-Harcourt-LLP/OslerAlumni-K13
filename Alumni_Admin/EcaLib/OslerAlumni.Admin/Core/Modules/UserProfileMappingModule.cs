using CMS;
using CMS.DataEngine;
using CMS.Membership;
using CMS.SiteProvider;

using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Services;

using BaseModule = ECA.Admin.Core.Modules.BaseModule;

[assembly: RegisterModule(typeof(UserProfileMappingModule))]

namespace OslerAlumni.Admin.Core.Modules
{
    public class UserProfileMappingModule : BaseModule
    {
        #region "Properties"

        public IProfileService ProfileService { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileMappingModule"/> class.
        /// </summary>
        public UserProfileMappingModule()
            : base($"{GlobalConstants.ModulePrefix}.{nameof(UserProfileMappingModule)}")
        {
        }

        /// <summary>
        /// Called when [initialize].
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();

            UserInfo.TYPEINFO.Events.Insert.After += User_Insert_After;
            UserInfo.TYPEINFO.Events.Update.After += User_Update_After;
            UserInfo.TYPEINFO.Events.Delete.After += User_Delete_After;

            // Creating a user and adding them to a site are 2 separate events
            UserSiteInfo.TYPEINFO.Events.Insert.After += UserSite_Insert_After;
        }

        #region "Events"

        protected void User_Insert_After(
            object sender,
            ObjectEventArgs e)
        {
            var userInfo = e.Object as UserInfo;

            if (userInfo == null)
            {
                return;
            }

            IOslerUserInfo user = new OslerUserInfo(userInfo);
            
            ProfileService.CreateProfile(user);
        }

        protected void User_Update_After(
            object sender,
            ObjectEventArgs e)
        {
            var userInfo = e.Object as UserInfo;

            if (userInfo == null)
            {
                return;
            }
           

            IOslerUserInfo user = new OslerUserInfo(userInfo);

            ProfileService.UpdateProfile(user);
        }

        protected void User_Delete_After(
            object sender,
            ObjectEventArgs e)
        {
            // NOTE: It is important that we trigger the profile page in delete AFTER event of the user object,
            // since triggering it in delete BEFORE event would result in the pages being re-created, since Kentico will:
            // - first call the delete BEFORE event;
            // - then delete user's child objects, such as user settings and user site relationships;
            // - then update the user object (triggering update BEFORE and AFTER events),
            // so that the deletion of child objects is reflected in it;
            // - then finally delete the user object;
            // - and finally call the delete AFTER event.

            var userInfo = e.Object as UserInfo;

            if (userInfo == null)
            {
                return;
            }

            IOslerUserInfo user = new OslerUserInfo(userInfo);

            ProfileService.DeleteProfile(user);
        }

        protected void UserSite_Insert_After(
            object sender,
            ObjectEventArgs e)
        {
            var userInfo = (e.Object as UserSiteInfo)?
                .Parent as UserInfo;

            if (userInfo == null)
            {
                return;
            }

            IOslerUserInfo user = new OslerUserInfo(userInfo);

            ProfileService.CreateProfile(user);
        }

        #endregion
    }
}

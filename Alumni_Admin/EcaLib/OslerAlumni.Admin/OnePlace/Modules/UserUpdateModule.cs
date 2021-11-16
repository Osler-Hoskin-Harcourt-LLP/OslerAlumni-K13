using CMS;
using CMS.DataEngine;
using CMS.Membership;
using ECA.Admin.Core.Modules;
using OslerAlumni.Admin.OnePlace.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.OnePlace.Services;

[assembly: RegisterModule(typeof(UserUpdateModule))]

namespace OslerAlumni.Admin.OnePlace.Modules
{
    public class UserUpdateModule
        : BaseModule
    {
        #region "Properties"

        public IOnePlaceUserExportService OnePlaceUserExportService { get; set; }

        #endregion

        public UserUpdateModule()
            : base($"{GlobalConstants.ModulePrefix}.{nameof(UserUpdateModule)}")
        { }

        protected override void OnInit()
        {
            base.OnInit();

            UserInfo.TYPEINFO.Events.Update.Before += User_Update_Before;
        }

        #region "Events"

        protected void User_Update_Before(
            object sender,
            ObjectEventArgs e)
        {
            var userInfo = e.Object as UserInfo;

            if (userInfo == null)
            {
                return;
            }

            IOslerUserInfo user = new OslerUserInfo(userInfo);

            user.AutopopulateDependantFields();

            // If user is enabled, reset the custom flag,
            // so that the next time the user is disabled (manually or automatically),
            // the value of the flag reflects correctly the way the user was disabled
            if (user.Enabled)
            {
                user.IsDisabledAutomatically = false;
            }

            OnePlaceUserExportService
                .SubmitUserUpdateToOnePlace(ref user);

            // Reset this field so that it doesn't affect the next save action
            user.UpdateOnePlace = null;
        }

        #endregion
    }
}

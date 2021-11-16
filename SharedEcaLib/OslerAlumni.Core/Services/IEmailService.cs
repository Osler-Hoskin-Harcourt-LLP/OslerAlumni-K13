using System;
using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Core.Services
{
    public interface IEmailService
        : IService
    {
        void SendNewAlumniUserAccountNotificationEmail(
            IOslerUserInfo user,
            string token);

        void SendPasswordResetEmail(
            Guid userGuid, 
            string token);

        void SendPasswordResetConfirmationEmail(
            Guid userGuid);
    }
}

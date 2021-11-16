using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.Admin.OnePlace.Services
{
    public interface IOnePlaceUserImportService
        : IService
    {
        IOslerUserInfo ConvertToUser(
            Contact contact,
            IOslerUserInfo user = null);

        string GetAvailableUserName(
            IOslerUserInfo user);

        bool ImportAsUser(
            Contact contact,
            out ImportAction importAction);

        bool TryGetMappedUser(
            Contact contact,
            out IOslerUserInfo user);

        bool UpdateUserStatus(
            IOslerUserInfo user,
            IList<Contact> contacts,
            out AlumniChangeType changeType);
    }
}

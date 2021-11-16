using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceUserExportService
        : IService
    {
        /// <summary>
        /// Checks if the changes made to the user object should be submitted to OnePlace;
        /// if so, generates appropriate data submission tasks and adds them to the queue.
        /// Can make modifications to the user object, whenever appropriate (e.g. clearing
        /// Current Industry value when a change to user's Company name is detected), but
        /// does NOT submit the changes to the database.
        /// 
        /// This method is expected to be called during the user update, BEFORE it is about
        /// to be saved in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool SubmitUserUpdateToOnePlace(
            ref IOslerUserInfo user);
    }
}

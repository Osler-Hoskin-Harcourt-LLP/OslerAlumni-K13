using CMS.Scheduler;
using ECA.Admin.Core.Tasks;
using ECA.Core.Repositories;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Admin.OnePlace.Tasks
{
    public abstract class BaseOnePlaceTask
        : BaseTask
    {
        #region "Properties"

        public ISettingsKeyRepository SettingsKeyRepository { get; set; }

        #endregion
        
        #region "Methods"

        public override string Execute(
            TaskInfo task)
        {
            var isOnePlaceEnabled = SettingsKeyRepository.GetValue<bool>(
                GlobalConstants.Settings.OnePlace.Enabled);

            if (!isOnePlaceEnabled)
            {
                return "OnePlace connection is disabled. Check the setting at Settings > Osler Alumni > OnePlace > Data Settings > OnePlace enabled";
            }

            return ExecuteInternal(task);
        }

        #endregion

        #region "Helper methods"

        protected abstract string ExecuteInternal(
            TaskInfo task);

        #endregion
    }
}

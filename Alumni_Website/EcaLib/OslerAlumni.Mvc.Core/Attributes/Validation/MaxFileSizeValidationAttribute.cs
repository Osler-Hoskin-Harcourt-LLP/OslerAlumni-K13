using System.Web;
using System.Web.Mvc;
using ECA.Core.Repositories;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class MaxFileSizeValidationAttribute
        : KenticoValidateAttribute
    {
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private int _maxFileSizeInMb;

        #region "Properties"

        public int MaxFileSizeInMb
        {
            get
            {
                if (_maxFileSizeInMb == 0)
                {
                    var settingsKeyRepository =
                        _settingsKeyRepository ??
                        DependencyResolver.Current.GetService<ISettingsKeyRepository>();

                    _maxFileSizeInMb =
                        settingsKeyRepository?.GetValue<int>(
                            GlobalConstants.Settings.MaximumFileSizeInMb) ?? 0;
                }

                return _maxFileSizeInMb;
            }
        }

        public int MaxFileSizeInBytes
            => MaxFileSizeInMb * 1024 * 1024;

        #endregion

        /// <summary>
        /// FileSize Validator
        /// </summary>
        /// <param name="maxFileSizeInMb">Leave empty to pull from site settings</param>
        public MaxFileSizeValidationAttribute(
            int maxFileSizeInMb = 0)
        {
            _maxFileSizeInMb = maxFileSizeInMb;
        }

        public MaxFileSizeValidationAttribute(
            ISettingsKeyRepository settingsKeyRepository,
            int maxFileSizeInMb = 0)
                    : this(maxFileSizeInMb)
        {
            _settingsKeyRepository = settingsKeyRepository;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            if (file == null)
            {
                return true;
            }

            return file.ContentLength <= MaxFileSizeInBytes;
        }
    }
}

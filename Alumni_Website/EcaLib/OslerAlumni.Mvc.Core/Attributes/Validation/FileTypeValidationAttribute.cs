using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECA.Core.Repositories;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class FileTypeValidationAttribute
        : KenticoValidateAttribute
    {
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private string _allowedExtensions;

        #region "Properties"

        public string AllowedExtensions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_allowedExtensions))
                {
                    var settingsKeyRepository = _settingsKeyRepository ?? 
                        DependencyResolver.Current.GetService<ISettingsKeyRepository>();

                    _allowedExtensions = settingsKeyRepository?.GetValue<string>(
                        GlobalConstants.Settings.AllowableUploadFileTypeExtensions) ?? string.Empty;
                }

                return _allowedExtensions;
            }
        }

        public IList<string> AllowedExtensionList
            => AllowedExtensions?
                .Split(',')
                .Select(ext => ext?.Trim())
                .ToList();

        #endregion

        public FileTypeValidationAttribute(
            string allowableExtensions = null)
        {
            _allowedExtensions = allowableExtensions;
        }

        public FileTypeValidationAttribute(
            ISettingsKeyRepository settingsKeyRepository,
            string allowableExtensions = null)
            : this(allowableExtensions)
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

            var fileExtensionsAttribute = new FileExtensionsAttribute
            {
                Extensions = AllowedExtensions
            };

            return fileExtensionsAttribute.IsValid(file.FileName);
        }
    }
}

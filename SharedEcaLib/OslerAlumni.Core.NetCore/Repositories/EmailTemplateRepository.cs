using System.Collections.Generic;
using CMS.EmailEngine;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Extensions;
using ECA.Core.Models;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Core.Repositories
{
    public class EmailTemplateRepository
        : IEmailTemplateRepository
    {
        #region "Private fields"

        private readonly ICacheService _cacheService;

        private readonly ContextConfig _context;

        #endregion

        public EmailTemplateRepository(
            ICacheService cacheService,
            ContextConfig context)
        {
            _cacheService = cacheService;

            _context = context;
        }

        #region "Methods"
        
        public EmailTemplateInfo GetEmailTemplate(
            string templateName,
            string siteName = null)
        {
            siteName = siteName.ReplaceIfEmpty(_context.Site?.SiteName);

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.Emails.EmailTemplateByName, 
                    templateName),
                IsCultureSpecific = false,
                IsSiteSpecific = true,
                SiteName = siteName,
                // Bust the cache whenever the email template is modified
                CacheDependencies = new List<string>
                {
                    $"{EmailTemplateInfo.OBJECT_TYPE}|byname|{templateName}"
                }
            };

            var result = _cacheService.Get(
                () => 
                    EmailTemplateProvider.GetEmailTemplate(
                        templateName,
                        siteName),
                cacheParameters);

            return result;
        }

        #endregion
    }
}

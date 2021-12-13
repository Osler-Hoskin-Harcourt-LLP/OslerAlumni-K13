using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Extensions;
using ECA.Content.Repositories;
using ECA.Core.Models;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class JobsService
        : ServiceBase, IJobsService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;
        private readonly ContextConfig _context;

        #endregion

        public JobsService(
            IDocumentRepository documentRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
            _context = context;
        }

        #region "Methods"

        public List<PageType_Job> GetLatestJobs(int top)
        {
            if (top <= 0)
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Prefix +
                           $"{nameof(JobsService)}|{nameof(GetLatestJobs)}|top|{top}",
                AllowNullValue = false,
                CultureCode = LocalizationContext.CurrentCulture.CultureCode,
                CacheDependencies = new List<string>()
                {
                    string.Format(GlobalConstants.Caching.Pages.PagesByType, SiteContext.CurrentSiteName,
                        PageType_Job.CLASS_NAME)
                }
            };

            var result = _cacheService.Get(
                cp =>
                {

                    var featuredItems = _documentRepository
                        .GetDocuments(
                            pageTypeName: PageType_Job.CLASS_NAME,
                            columnNames:
                            new[]
                            {
                                nameof(PageType_Job.Title),
                                nameof(PageType_Job.PostedDate),
                                nameof(PageType_Job.JobLocation),
                                nameof(PageType_Job.ExternalUrl)
                            },
                            top: top,
                            orderDirection: OrderDirection.Descending,
                            orderByColumns: new[] { nameof(PageType_Job.PostedDate) });

                    return featuredItems.Select(fi => fi.ToPageType<PageType_Job>()).ToList();
                },
                cacheParameters);

            return result;
        }
        #endregion

    }
}

using CMS.Helpers;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Repositories;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(JobsController), Path = "/Careers/Job-Board", ActionName = nameof(JobsController.Index))]
[assembly: RegisterPageRoute(PageType_Job.CLASS_NAME, typeof(JobsController), ActionName = nameof(JobsController.Details))]

namespace OslerAlumni.Mvc.Controllers
{
    [Authorize(Policy = "PublicPage")]
    public class JobsController 
        : BaseController
    {
        private readonly IJobCategoryItemRepository _jobCategoryItemService;

        public JobsController(
            IPageUrlService pageUrlService,
            IJobCategoryItemRepository jobCategoryItemService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _jobCategoryItemService = jobCategoryItemService;
        }

        #region "Actions"

        // GET: Jobs
        [HttpGet]
        public IActionResult Index(
            )
        {
            var page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new LandingPageViewModel(page);

            return View(landingPageViewModel);
        }
        
        public IActionResult Details()
        {
            var page = _dataRetriever.Retrieve<PageType_Job>().Page;

            var jobsDetailsPageViewModel = new JobsDetailsPageViewModel(page);
            
            jobsDetailsPageViewModel.JobCategoryDisplayName =
                ResHelper.GetString(
                    _jobCategoryItemService
                        .GetByCodeName(jobsDetailsPageViewModel.JobCategoryCodeName)?
                        .DisplayName);

            return View(jobsDetailsPageViewModel);
        }

        #endregion
    }
}

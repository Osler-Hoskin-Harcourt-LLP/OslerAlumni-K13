using CMS.Helpers;
using CMS.Localization;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Repositories;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(BoardOpportunityController), Path = "/Careers/Board-Opportunities", ActionName = nameof(BoardOpportunityController.Index))]
[assembly: RegisterPageRoute(PageType_BoardOpportunity.CLASS_NAME, typeof(BoardOpportunityController), ActionName = nameof(BoardOpportunityController.Details))]

namespace OslerAlumni.Mvc.Controllers
{
    [Authorize(Policy = "PublicPage")]
    public class BoardOpportunityController 
        : BaseController
    {
        private readonly IJobCategoryItemRepository _jobCategoryItemRepository;
        private readonly IBoardOpportunityTypeItemRepository _boardOpportunityTypeItemRepository;
        private readonly IBoardOpportunitySourceItemRepository _boardOpportunitySourceItemRepository;
        public BoardOpportunityController(
            IPageUrlService pageUrlService,
            IJobCategoryItemRepository jobCategoryItemRepository,
            IBoardOpportunityTypeItemRepository boardOpportunityTypeItemRepository,
            IBoardOpportunitySourceItemRepository boardOpportunitySourceItemRepository,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _jobCategoryItemRepository = jobCategoryItemRepository;
            _boardOpportunityTypeItemRepository = boardOpportunityTypeItemRepository;
            _boardOpportunitySourceItemRepository = boardOpportunitySourceItemRepository;
        }

        #region "Actions"

        // GET: Jobs
        [HttpGet]
        public IActionResult Index()
        {
            PageType_LandingPage page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new LandingPageViewModel(page);

            return View(landingPageViewModel);
        }
        
        public IActionResult Details()
        {
            var page = _dataRetriever.Retrieve<PageType_BoardOpportunity>().Page;

            var boardOpportunityDetailsPageViewModel = new BoardOpportunityDetailsPageViewModel(page);

            boardOpportunityDetailsPageViewModel.JobCategoryDisplayName =
                ResHelper.GetString(
                    _jobCategoryItemRepository
                        .GetByCodeName(page.JobCategoryCodeName)?
                        .DisplayName);


            boardOpportunityDetailsPageViewModel.BoardOpportunityTypeDisplayName =
                ResHelper.GetString(
                    _boardOpportunityTypeItemRepository
                        .GetByCodeName(page.BoardOpportunityTypeCodeName)?
                        .DisplayName);


            var source = _boardOpportunitySourceItemRepository.GetByCodeName(page.SourceCodeName);

            boardOpportunityDetailsPageViewModel.SourceDisplayName = ResHelper.GetString(source?.CompanyName);

            boardOpportunityDetailsPageViewModel.SourceCompanyLogo =
                LocalizationContext.CurrentCulture.CultureCode == GlobalConstants.Cultures.English ? source?.LogoEn : source?.LogoFr;

            boardOpportunityDetailsPageViewModel.SourceCompanyLogoAltText = source?.LogoAltText;

            return View(boardOpportunityDetailsPageViewModel);
        }

        #endregion
    }
}

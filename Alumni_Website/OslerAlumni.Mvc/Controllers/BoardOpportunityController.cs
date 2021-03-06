using System.Web.Mvc;
using CMS.Helpers;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Repositories;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(BoardOpportunityController), Path = "/Careers/Board-Opportunities", ActionName = nameof(BoardOpportunityController.Index))]
[assembly: RegisterPageRoute(PageType_BoardOpportunity.CLASS_NAME, typeof(BoardOpportunityController), ActionName = nameof(BoardOpportunityController.Details))]

namespace OslerAlumni.Mvc.Controllers
{
    [OslerAuthorize]
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
        public ActionResult Index()
        {
            PageType_LandingPage page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new LandingPageViewModel(page);

            return View(landingPageViewModel);
        }
        
        public ActionResult Details()
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
                _context.CultureName == GlobalConstants.Cultures.English ? source?.LogoEn : source?.LogoFr;

            boardOpportunityDetailsPageViewModel.SourceCompanyLogoAltText = source?.LogoAltText;

            return View(boardOpportunityDetailsPageViewModel);
        }

        #endregion
    }
}

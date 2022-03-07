using CMS.Helpers;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Models;
using System.Linq;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(ProfilesController), Path = "/Directory", ActionName = nameof(ProfilesController.Index))]
[assembly: RegisterPageRoute(PageType_Profile.CLASS_NAME, typeof(ProfilesController), ActionName = nameof(ProfilesController.Details))]


namespace OslerAlumni.Mvc.Controllers
{
    [Authorize(Policy = "PublicPage")]
    public class ProfilesController
        : BaseController
    {
        private readonly IProfileService _profileService;
        private readonly IUserRepository _userRepository;

        public ProfilesController(
            IPageUrlService pageUrlService,
            IProfileService profileService,
            IUserRepository userRepository,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context,dataRetriever)
        {
            _profileService = profileService;
            _userRepository = userRepository;
        }

        #region "Actions"

        // GET: Alumni
        [HttpGet]
        public IActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var pageViewModel = new ProfileLandingPageViewModel(page);

            var currentUser = _userRepository.CurrentUser;

            pageViewModel.LoggedInUserPage = _profileService.GetUserProfile(currentUser.UserGUID);

            return View(pageViewModel);
        }

        public IActionResult Details()
        {
            var page = _dataRetriever.Retrieve<PageType_Profile>().Page;

            var localizedYearOfCallAndJurisdictionsList = page
                .YearOfCallAndJurisdictionsList
                .Select(yj => new YearAndJurisdiction()
                {
                    Year = yj.Year,
                    Jurisdiction = ResHelper.GetString(yj.Jurisdiction)
                }).ToList();

            string yearsAtOsler = string.Empty;

            if (page.StartDateAtOsler != DateTimeHelper.ZERO_TIME && page.EndDateAtOsler != DateTimeHelper.ZERO_TIME)
            {
                yearsAtOsler = $"{page.StartDateAtOsler.Year} - {page.EndDateAtOsler.Year}";
            }

            var profileDetailsPageViewModel =
                new ProfileDetailsPageViewModel(page)
                {
                    CurrentIndustry = ResHelper.GetString(page.CurrentIndustry),
                    OfficeLocations = page.OfficeLocations
                        .SplitOn(';')
                        ?.Select(office => ResHelper.GetString(office))
                        .ToList(),
                    PracticeAreas = page.PracticeAreas
                        .SplitOn(';')
                        ?.Select(area => ResHelper.GetString(area))
                        .ToList(),
                    BoardMemberships = page.BoardMemberships
                        .SplitOn(';'),
                    YearOfCallAndJurisdictions = localizedYearOfCallAndJurisdictionsList,
                    YearsAtOsler = yearsAtOsler,
                    EducationHistory = page.EducationOverviewList
                };

            profileDetailsPageViewModel.PracticeAreas?.Sort();
            profileDetailsPageViewModel.OfficeLocations?.Sort();
            profileDetailsPageViewModel.BoardMemberships?.Sort();

            profileDetailsPageViewModel.YearOfCallAndJurisdictions.Sort(new YearAndJurisdictionComparer());

            return View(profileDetailsPageViewModel);
        }

        #endregion
    }
}

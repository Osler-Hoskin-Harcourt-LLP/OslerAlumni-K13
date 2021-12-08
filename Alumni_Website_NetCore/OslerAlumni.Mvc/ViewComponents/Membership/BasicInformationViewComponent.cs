using CMS.Helpers;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using OslerAlumniWebsite.ViewComponents.Membership;
using System.Linq;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents
{
    public class BasicInformationViewComponent : MembershipBaseViewComponent
    {
        public BasicInformationViewComponent(IUserRepository userRepository, IGenericPageService genericPageService, ITokenService tokenService, IUserService userService, IPageDataContextRetriever pageDataRetriever) : base(userRepository, genericPageService, tokenService, userService, pageDataRetriever)
        {
        }

        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync(string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return await RenderTab(page, subpageAlias, GetBasicInfoFormModel);
        }

        private MembershipBasicInfoFormModel GetBasicInfoFormModel(IOslerUserInfo currentUser)
        {
            var localizedYearOfCallAndJurisdictionsList = currentUser
                .YearOfCallAndJurisdictionsList
                .Select(yj => new YearAndJurisdiction
                {
                    Year = yj.Year,
                    Jurisdiction = ResHelper.GetString(yj.Jurisdiction)
                }).ToList();

            return new MembershipBasicInfoFormModel
            {
                UserGuid = currentUser.UserGUID,
                UserName = currentUser.UserName,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email,
                AlumniEmail = currentUser.AlumniEmail,
                CompanyName = currentUser.Company,
                City = currentUser.City,
                Province = currentUser.Province,
                Country = currentUser.Country,
                JobTitle = currentUser.JobTitle,
                YearOfCallAndJurisdictions = localizedYearOfCallAndJurisdictionsList,
                CurrentIndustry = ResHelper.GetString(currentUser.CurrentIndustry),
                LinkedInUrl = currentUser.LinkedInUrl,
                TwitterUrl = currentUser.TwitterUrl,
                InstagramUrl = currentUser.InstagramUrl,
                EducationHistory = currentUser.EducationOverviewList
            };
        }
    }
}

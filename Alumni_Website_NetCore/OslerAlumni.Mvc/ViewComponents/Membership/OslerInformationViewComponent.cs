using CMS.Helpers;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Membership
{
    public class OslerInformationViewComponent : MembershipBaseViewComponent
    {
        public OslerInformationViewComponent(IUserRepository userRepository, IGenericPageService genericPageService, ITokenService tokenService, IUserService userService, IPageDataContextRetriever pageDataContextRetriever) : base(userRepository, genericPageService, tokenService, userService, pageDataContextRetriever)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return await RenderTab(page, subpageAlias, GetOslerInfoFormModel);
        }

        private MembershipOslerInfoFormModel GetOslerInfoFormModel(
           IOslerUserInfo currentUser)
        {
            string yearsAtOsler = string.Empty;

            if (currentUser.StartDateAtOsler.HasValue && currentUser.EndDateAtOsler.HasValue)
            {
                yearsAtOsler = $"{currentUser.StartDateAtOsler.Value.Year} - {currentUser.EndDateAtOsler.Value.Year}";
            }

            return new MembershipOslerInfoFormModel
            {
                OslerLocations = currentUser.OfficeLocationsList
                    ?.Select(office => ResHelper.GetString(office))
                    .ToList(),
                OslerPracticeAreas = currentUser.PracticeAreasList
                    ?.Select(area => ResHelper.GetString(area))
                    .ToList(),
                YearsAtOsler = yearsAtOsler
            };
        }
    }
}

using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Membership
{
    public class PreferencesViewComponent : MembershipBaseViewComponent
    {
        public PreferencesViewComponent(IUserRepository userRepository, IGenericPageService genericPageService, ITokenService tokenService, IUserService userService, IPageDataContextRetriever pageDataContextRetriever) : base(userRepository, genericPageService, tokenService, userService, pageDataContextRetriever)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return await RenderTab(page, subpageAlias, GetAccountPreferencesFormModel);

        }


        private MembershipPreferencesFormModel GetAccountPreferencesFormModel(
            IOslerUserInfo currentUser)
        {
            return new MembershipPreferencesFormModel
            {
                UserGuid = currentUser.UserGUID,
                IncludeEmailInDirectory = currentUser.IncludeEmailInDirectory,
                DisplayImageInDirectory = currentUser.DisplayImageInDirectory,
                SubscribeToEmailUpdates = currentUser.SubscribeToEmailUpdates,
            };
        }
    }
}

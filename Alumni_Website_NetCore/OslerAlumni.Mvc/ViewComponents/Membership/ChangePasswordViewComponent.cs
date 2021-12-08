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
    public class ChangePasswordViewComponent : MembershipBaseViewComponent
    {
        public ChangePasswordViewComponent(IUserRepository userRepository, IGenericPageService genericPageService, ITokenService tokenService, IUserService userService, IPageDataContextRetriever pageDataContextRetriever) : base(userRepository, genericPageService, tokenService, userService, pageDataContextRetriever)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return await RenderTab(page, subpageAlias,
                (currentUser) => GetResetPasswordFormModel(currentUser).GetAwaiter().GetResult());
        }



        private async Task<ResetPasswordFormModel> GetResetPasswordFormModel(
            IOslerUserInfo currentUser)
        {
            var token = await _tokenService.GeneratePasswordResetTokenAsync(currentUser.UserGUID);

            return new ResetPasswordFormModel
            {
                UserGuid = currentUser.UserGUID,
                Token = token
            };
        }
    }
}

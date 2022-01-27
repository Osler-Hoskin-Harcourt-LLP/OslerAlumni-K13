using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using System;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Membership
{
    public class ProfileImageViewComponent : MembershipBaseViewComponent
    {
        public ProfileImageViewComponent(IUserRepository userRepository, IGenericPageService genericPageService, ITokenService tokenService, IUserService userService, IPageDataContextRetriever pageDataContextRetriever) : base(userRepository, genericPageService, tokenService, userService, pageDataContextRetriever)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string subpageAlias, string edit)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return await RenderTab(
                page,
                subpageAlias,
                (currentUser) =>
                {
                    var model = GetProfileImageFormModel(currentUser);

                    if (string.Equals(edit.ToLower(),
                        true.ToString().ToLower(),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        model.DisplayMode = Mode.Edit;
                    }

                    return model;
                }
            );
        }

        private MembershipProfileImageFormModel GetProfileImageFormModel(
            IOslerUserInfo currentUser)
        {
            return new MembershipProfileImageFormModel
            {
                DisplayMode = string.IsNullOrWhiteSpace(currentUser.ProfileImage) ? Mode.Edit : Mode.View,
                UserGuid = currentUser.UserGUID,
                ProfileImageUrl = currentUser.ProfileImage
            };
        }
    }
}

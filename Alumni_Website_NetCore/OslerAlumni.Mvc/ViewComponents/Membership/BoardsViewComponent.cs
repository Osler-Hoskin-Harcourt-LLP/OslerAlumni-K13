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
    public class BoardsViewComponent : MembershipBaseViewComponent
    {
        public BoardsViewComponent(IUserRepository userRepository, IGenericPageService genericPageService, ITokenService tokenService, IUserService userService, IPageDataContextRetriever pageDataContextRetriever) : base(userRepository, genericPageService, tokenService, userService, pageDataContextRetriever)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return await RenderTab(page, subpageAlias, GetBoardsFormModel);
        }


        private MembershipBoardsFormModel GetBoardsFormModel(
            IOslerUserInfo currentUser)
        {
            var list = currentUser.BoardMembershipsList;
            list.Sort();

            return new MembershipBoardsFormModel
            {
                UserGuid = currentUser.UserGUID,
                BoardMembershipList = list
            };
        }

    }
}

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
    public abstract class MembershipBaseViewComponent : ViewComponent
    {
        protected readonly IUserRepository _userRepository;
        protected readonly IGenericPageService _genericPageService;
        protected readonly ITokenService _tokenService;
        protected readonly IUserService _userService;
        protected readonly IPageDataContextRetriever _dataRetriever;


        public MembershipBaseViewComponent(IUserRepository userRepository,
            IGenericPageService genericPageService,
            ITokenService tokenService,
            IUserService userService,
            IPageDataContextRetriever pageDataContextRetriever)
        {
            _userRepository = userRepository;
            _genericPageService = genericPageService;
            _tokenService = tokenService;
            _userService = userService;
            _dataRetriever = pageDataContextRetriever;
        }
        protected async Task<IViewComponentResult> RenderTab(PageType_Page page, string subpageAlias, Func<IOslerUserInfo, object> formPostModel)
        {
            var subPage = _genericPageService.GetSubPage(page, subpageAlias);

            if (subPage == null)
            {
                return Content("");
            }

            var model = new MembershipInfoPageTabContentViewModel(subPage)
            {
                FormPostModel = formPostModel.Invoke(_userRepository.GetByName(_userRepository.CurrentUser.UserName)),
            };

            return View("~/Views/Shared/Components/Membership/_TabContent.cshtml", model);
        }
    }


}

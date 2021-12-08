using CMS.Helpers;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Mvc.Navigation.Extensions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using OslerAlumniWebsite.ViewComponents;
using OslerAlumniWebsite.ViewComponents.Membership;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(MembershipController), Path = "/Profile-and-preferences", ActionName = nameof(MembershipController.Index))]


namespace OslerAlumni.Mvc.Controllers
{
    [Authorize]
    public class MembershipController
        : BaseController
    {
        #region "Private fields"

        private readonly IUserRepository _userRepository;
        private readonly IGenericPageService _genericPageService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        #endregion

        public MembershipController(
            IUserRepository userRepository,
            IGenericPageService genericPageService,
            IPageUrlService pageUrlService,
            ITokenService tokenService,
            IUserService userService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _userRepository = userRepository;
            _genericPageService = genericPageService;
            _tokenService = tokenService;
            _userService = userService;
        }

        #region "Actions"

        [HttpGet]
        [HideActionFromSystemUser]
        public IActionResult Index(string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            var model = new MembershipInfoPageViewModel(page);

            model.ProfileUrl = _userService.GetProfileUrl(_userRepository.CurrentUser);

            if (string.IsNullOrWhiteSpace(subpageAlias))
            {
                model.Controller = typeof(MembershipController).GetControllerName();
                model.Action = StringHelper.RemoveViewComponent(nameof(BasicInformationViewComponent));
                model.ViewComponent = StringHelper.RemoveViewComponent(nameof(BasicInformationViewComponent));
            }
            else
            {
                var subPage = _genericPageService.GetSubPage(page, subpageAlias);

                if (subPage == null)
                {
                    return NotFound();
                }

                model.Controller = subPage.DefaultController;
                model.Action = subPage.DefaultAction;
                model.ViewComponent = subPage.DefaultAction;
            }

            model.SubPageNavigation =
                GetSubNavigation(page, model.Controller, model.Action);

            if (string.Equals(model.Action, StringHelper.RemoveViewComponent(nameof(ProfileImageViewComponent)), StringComparison.OrdinalIgnoreCase))
            {
                model.ShowCta = false;
                model.IsFullWidth = true;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public IActionResult MembershipUpdateBasicInfo(
            MembershipBasicInfoFormModel model)
        {
            var currentUser = _userRepository.CurrentUser;

            //Ensure the user is only modifying his/her own profile.
            if (model.UserGuid != currentUser.UserGUID)
            {
                return this.Forbidden();
            }

            var updated = _userService.Save(
                currentUser.UserGUID,
                userInfo =>
                {
                    userInfo.FirstName = model.FirstName;
                    userInfo.LastName = model.LastName;
                    userInfo.Email = model.Email;
                    userInfo.AlumniEmail = model.AlumniEmail;
                    userInfo.Company = model.CompanyName;
                    userInfo.City = model.City;
                    userInfo.Province = model.Province;
                    userInfo.Country = model.Country;
                    userInfo.JobTitle = model.JobTitle;
                    userInfo.LinkedInUrl = model.LinkedInUrl;
                    userInfo.TwitterUrl = model.TwitterUrl;
                    userInfo.InstagramUrl = model.InstagramUrl;
                });

            //TODO: Can potentially return 424 instead of success if update fails gracefully
            //https://httpstatuses.com/424

            return JsonContent(new BaseWebResponse<object>
            {
                Status = updated
                    ? WebResponseStatus.Success
                    : WebResponseStatus.Error
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public IActionResult MembershipUpdateBoards(
            MembershipBoardsFormModel model)
        {
            var currentUser = _userRepository.CurrentUser;

            //Ensure the user is only modifying his/her own profile.
            if (model.UserGuid != currentUser.UserGUID)
            {
                return this.Forbidden();
            }

            model.NewBoard = model.NewBoard.TrimAll();

            if (!ValidateNewBoard(model.NewBoard, currentUser, ModelState))
            {
                return new ValidationErrorJsonResult(ModelState);
            }

            var newList = currentUser.BoardMembershipsList;

            newList.Add(model.NewBoard);

            var updated = _userService.Save(
                currentUser.UserGUID,
                userInfo =>
                {
                    userInfo.BoardMembershipsList = newList;
                });

            return JsonContent(new BaseWebResponse<object>
            {
                Status = updated
                    ? WebResponseStatus.Success
                    : WebResponseStatus.Error,
                RefreshOnSuccess = true
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public IActionResult MembershipUpdatePreferences(
            MembershipPreferencesFormModel model)
        {
            var currentUser = _userRepository.CurrentUser;

            //Ensure the user is only modifying his/her own profile.
            if (model.UserGuid != currentUser.UserGUID)
            {
                return this.Forbidden();
            }

            var updated = _userService.Save(
                currentUser.UserGUID,
                userInfo =>
                {
                    userInfo.IncludeEmailInDirectory = model.IncludeEmailInDirectory;
                    userInfo.DisplayImageInDirectory = model.DisplayImageInDirectory;
                    userInfo.SubscribeToEmailUpdates = model.SubscribeToEmailUpdates;
                });

            return JsonContent(new BaseWebResponse<object>
            {
                Status = updated
                    ? WebResponseStatus.Success
                    : WebResponseStatus.Error
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public IActionResult MembershipUploadProfileImage(
            MembershipProfileImageFormModel model)
        {
            bool updated = false;

            var currentUser = _userRepository.CurrentUser;

            //Ensure the user is only modifying his/her own profile.
            if (model.UserGuid != currentUser.UserGUID)
            {
                return this.Forbidden();
            }

            updated = _userService.UploadProfileImage(currentUser, model.FileUpload);

            if (updated && (model.ImageWidth > 0 || model.ImageHeight > 0 || model.ImageX > 0 || model.ImageY > 0))
            {
                //Crop Image
                updated = _userService.CropProfileImage(currentUser, model.ImageX, model.ImageY, model.ImageWidth,
                    model.ImageHeight);
            }

            var redirectUrl = Request?.Headers["Referer"].ToString();

            redirectUrl = redirectUrl.Replace("&edit=true", String.Empty);

            return JsonContent(new BaseWebResponse<object>
            {
                Status = updated
                    ? WebResponseStatus.Success
                    : WebResponseStatus.Error,
                RedirectToUrl = redirectUrl
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        public IActionResult DeleteProfileImage([Required] Guid userGuid)
        {
            bool updated = false;

            var currentUser = _userRepository.CurrentUser;

            //Ensure the user is only modifying his/her own profile.
            if (userGuid != currentUser.UserGUID)
            {
                return this.Forbidden();
            }

            updated = _userService.DeleteProfileImage(currentUser);

            var redirectUrl = Request?.Headers["Referer"].ToString();

            redirectUrl = redirectUrl.Replace("&edit=true", String.Empty);

            return JsonContent(new BaseWebResponse<object>
            {
                Status = updated
                    ? WebResponseStatus.Success
                    : WebResponseStatus.Error,
                RedirectToUrl = redirectUrl
            });
        }


        #endregion

        #region "Methods"
        private bool ValidateNewBoard(string newBoard, IOslerUserInfo currentUser, ModelStateDictionary modelState)
        {
            var boardsList = currentUser.BoardMembershipsList;

            if (boardsList.ContainsCaseInsensitive(newBoard))
            {
                modelState.AddModelError(nameof(MembershipBoardsFormModel.NewBoard),
                    ResHelper.GetString(Constants.ResourceStrings.Form.MembershipBoards.NewBoardDuplicate));

                return false;
            }

            var maxLength = currentUser
                                .GetType()
                                .GetProperty(nameof(IOslerUserInfo.BoardMemberships))
                                ?.GetCustomAttribute<MaxLengthAttribute>(true)?.Length ?? 0;

            if (maxLength > 0 && (currentUser.BoardMemberships.Length + newBoard.Length) > maxLength)
            {
                modelState.AddModelError(nameof(MembershipBoardsFormModel.NewBoard),
                    ResHelper.GetString(Constants.ResourceStrings.Form.MembershipBoards.NewBoardMaxLimitReached));

                return false;
            }

            return true;
        }



       



        private List<SubPagesNav> GetSubNavigation(
            PageType_Page page,
            string selectedController,
            string selectedAction)
        {
            var subPages =
                _genericPageService.GetSubPages(page) ?? new List<PageType_Page>();

            var subPagesNav = subPages
                .Select(subPage => new SubPagesNav
                {
                    Action = subPage.DefaultAction,
                    Controller = subPage.DefaultController,
                    NodeAlias = subPage.NodeAlias,
                    Title = subPage.Title,
                    IsSelected =
                        (selectedController == subPage.DefaultController)
                        && (selectedAction == subPage.DefaultAction)
                })
                .ToList();

            return subPagesNav;
        }


        #endregion

    }
}

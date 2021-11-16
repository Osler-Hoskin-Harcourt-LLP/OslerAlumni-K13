using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using CMS.Helpers;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Mvc.Navigation.Extensions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
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

[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(MembershipController), Path = "/Profile-and-preferences", ActionName = nameof(MembershipController.Index))]


namespace OslerAlumni.Mvc.Controllers
{
    [OslerAuthorize]
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
        public ActionResult Index(string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            var model = new MembershipInfoPageViewModel(page);

            model.ProfileUrl = _userService.GetProfileUrl(_userRepository.CurrentUser);

            if (string.IsNullOrWhiteSpace(subpageAlias))
            {
                model.Controller = typeof(MembershipController).GetControllerName();
                model.Action = nameof(BasicInformation);
            }
            else
            {
                var subPage = GetSubPage(page, subpageAlias);

                if (subPage == null)
                {
                    return HttpNotFound();
                }

                model.Controller = subPage.DefaultController;
                model.Action = subPage.DefaultAction;
            }

            model.SubPageNavigation =
                GetSubNavigation(page, model.Controller, model.Action);

            if (string.Equals(model.Action, nameof(ProfileImage), StringComparison.OrdinalIgnoreCase))
            {
                model.ShowCta = false;
                model.IsFullWidth = true;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult BasicInformation(
            string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return RenderTab(page, subpageAlias, GetBasicInfoFormModel);
        }

        [HttpGet]
        public ActionResult ProfileImage(
            string subpageAlias, string edit)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return RenderTab(
                page,
                subpageAlias,
                (currentUser) =>
                {
                    var model = GetProfileImageFormModel(currentUser);

                    if (string.Equals(edit,
                        true.ToString(),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        model.DisplayMode = Mode.Edit;
                    }

                    return model;
                }
            );
        }

        [HttpGet]
        public ActionResult Boards(
            string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return RenderTab(page, subpageAlias, GetBoardsFormModel);
        }

        [HttpGet]
        public ActionResult Preferences(
            string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return RenderTab(page, subpageAlias, GetAccountPreferencesFormModel);
        }

        [HttpGet]
        public ActionResult ChangePassword(
            string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return RenderTab(page, subpageAlias,
                (currentUser) => GetResetPasswordFormModel(currentUser).GetAwaiter().GetResult());
        }

        [HttpGet]
        public ActionResult OslerInformation(
            string subpageAlias)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return RenderTab(page, subpageAlias, GetOslerInfoFormModel);
        }

        [HttpPost]
        [ValidateHeaderAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public ActionResult MembershipUpdateBasicInfo(
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
        [ValidateHeaderAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public ActionResult MembershipUpdateBoards(
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
                HttpResponseHelper.SkipIisCustomErrors(ControllerContext.HttpContext);
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
        [ValidateHeaderAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public ActionResult MembershipUpdatePreferences(
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
        [ValidateHeaderAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        public ActionResult MembershipUploadProfileImage(
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

            var redirectUrl = Request?.UrlReferrer.ToString();

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
        [ValidateHeaderAntiForgeryToken(Order = 1)]
        public ActionResult DeleteProfileImage([Required] Guid userGuid)
        {
            bool updated =false;

            var currentUser = _userRepository.CurrentUser;

            //Ensure the user is only modifying his/her own profile.
            if (userGuid != currentUser.UserGUID)
            {
                return this.Forbidden();
            }

            updated = _userService.DeleteProfileImage(currentUser);

            var redirectUrl = Request?.UrlReferrer.ToString();

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

        private ActionResult RenderTab(PageType_Page page, string subpageAlias, Func<IOslerUserInfo, object> formPostModel)
        {
            var subPage = GetSubPage(page, subpageAlias);

            if (subPage == null)
            {
                return HttpNotFound();
            }

            var model = new MembershipInfoPageTabContentViewModel(subPage)
            {
                FormPostModel = formPostModel.Invoke(_userRepository.CurrentUser),
            };

            return PartialView("_TabContent", model);
        }

        private MembershipBasicInfoFormModel GetBasicInfoFormModel(
            IOslerUserInfo currentUser)
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

        private PageType_Page GetSubPage(
            PageType_Page page,
            string subpageAlias)
        {
            if (string.IsNullOrWhiteSpace(subpageAlias))
            {
                return null;
            }

            var subPage = _genericPageService
                .GetSubPageByAlias(
                    page,
                    subpageAlias);

            return subPage;
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

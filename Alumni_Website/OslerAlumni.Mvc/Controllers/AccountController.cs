using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using CMS.Activities.Loggers;
using CMS.Helpers;
using CMS.SiteProvider;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNet.Identity.Owin;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(AccountController), Path = "/Account/Log-In", ActionName = nameof(AccountController.LogIn))]
[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(AccountController), Path = "/Account/Request-Password-Reset", ActionName = nameof(AccountController.RequestPasswordReset))]
[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(AccountController), Path = "/Account/Reset-Password", ActionName = nameof(AccountController.ResetPassword))]
[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(AccountController), Path = "/Account/New-User-Password", ActionName = nameof(AccountController.NewUserSetupPassword))]


namespace OslerAlumni.Mvc.Controllers
{
    public class AccountController
        : BaseController
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMvcEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IIpLocatorService _ipLocatorService;
        private readonly IMembershipActivityLogger _membershipActivityLogger;
        #endregion

        public AccountController(
            IEventLogRepository eventLogRepository,
            IUserRepository userRepository,
            IAuthenticationService authenticationService,
            IMvcEmailService emailService,
            IPageUrlService pageUrlService,
            ITokenService tokenService,
            IIpLocatorService ipLocatorService,
            IMembershipActivityLogger membershipActivityLogger,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _eventLogRepository = eventLogRepository;
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _tokenService = tokenService;
            _ipLocatorService = ipLocatorService;
            _membershipActivityLogger = membershipActivityLogger;
        }

        #region "Actions"

        /// <summary>
        /// Basic action that displays the sign-in form.
        /// </summary>
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;
            if (_userRepository.CurrentUser != null)
            {
                return new RedirectResult("/");
            }

            return View(new LogInPageViewModel(page)
            {
                ShowLoginViaOslerNetwork = _ipLocatorService.IsCurrentUserInOslerNetwork(),
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// Handles authentication when the sign-in form is submitted.
        /// </summary>
        [HttpPost]
        //Reason why we don't validate for html injections is because we don't check for them on any of AJAX post forms.
        //Therefore to keep it consistent with all forms this is needed.
        //Also there was an issue where user can change there password to contain html in Change Password page, but won't be
        //able to log in after the fact.
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(
            LogInFormModel model,
            string returnUrl)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;
            var showLoginViaOslerNetwork = _ipLocatorService.IsCurrentUserInOslerNetwork();


            // Validates the received user credentials based on the view model
            if (!ModelState.IsValid)
            {
                // Displays the sign-in form if the user credentials are invalid
                return View(new LogInPageViewModel(page)
                {
                    ShowLoginViaOslerNetwork = showLoginViaOslerNetwork,
                    ReturnUrl = returnUrl
                });
            }

            var signInResult = await TrySignIn(model.UserName, model.Password);

            // If the authentication was not successful, displays the sign-in form with an "Authentication failed" message
            if (signInResult != SignInStatus.Success)
            {
                ModelState.AddModelError(nameof(LogInFormModel.UserName), ResHelper.GetString(Constants.ResourceStrings.Form.Login.UserNameOrPasswordIncorrect));
                ModelState.AddModelError(nameof(LogInFormModel.Password), " ");

                return View(new LogInPageViewModel(page)
                {
                    ShowLoginViaOslerNetwork = showLoginViaOslerNetwork,
                    ReturnUrl = returnUrl
                });
            }

            _membershipActivityLogger.LogLogin(model.UserName);

            return RedirectOnLogin(returnUrl);
        }

        private ActionResult RedirectOnLogin(string returnUrl)
        {
            // If the authentication was successful, redirects to the return URL when possible or to a different default action
            var decodedReturnUrl = Server.UrlDecode(returnUrl);

            if (!string.IsNullOrEmpty(decodedReturnUrl) && Url.IsLocalUrl(decodedReturnUrl))
            {
                return Redirect(decodedReturnUrl);
            }

            return new RedirectResult("/");
        }


        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        public async Task<ActionResult> LogInAsOslerNetwork(string returnUrl)
        {
            if (_ipLocatorService.IsCurrentUserInOslerNetwork())
            {
                var systemUser = _userRepository.GetSystemUser()?.UserInfo;

                if (systemUser != null)
                {
                    var signInResult = await _authenticationService.LogInAsUserAsync(systemUser.UserName);

                    if (signInResult == SignInStatus.Success)
                    {
                        _membershipActivityLogger.LogLogin(systemUser.UserName);

                        return RedirectOnLogin(returnUrl);
                    }
                }
            }

            return RedirectToAction(nameof(LogIn));
        }

        /// <summary>
        /// Action for signing out users.
        /// The Authorize attribute allows the action only for users who are already signed in.
        /// </summary>
        [OslerAuthorize]
        public ActionResult LogOut()
        {
            // Signs out the current user
            _authenticationService.LogOut();
            
            // Redirects to a different action after the sign-out
            return new RedirectResult("/");
        }

        [HttpGet]
        public ActionResult RequestPasswordReset()
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            return View(new RequestPasswordResetPageViewModel(page));
        }

        [HttpPost]
        [ValidateHeaderAntiForgeryToken]
        [ValidateModel]
        public async Task<ActionResult> RequestPasswordReset(
            RequestPasswordResetPostModel model)
        {
            var user = _userRepository.GetByName(model.UserNameOrEmail) ??
                       _userRepository.GetByEmail(model.UserNameOrEmail);
            
            if (user != null && user.Enabled)
            {
                var token = 
                    await _tokenService.GeneratePasswordResetTokenAsync(user.UserGUID);

                _emailService.SendPasswordResetEmail(user.UserGUID, token);
            }

            // Note: We always return success message due to security reasons
            return JsonContent(new BaseWebResponse<object>
            {
                Status = WebResponseStatus.Success
            });
        }

        [HttpGet]
        public ActionResult ResetPassword(
            Guid? userGuid,
            string token)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            if (!userGuid.HasValue || string.IsNullOrEmpty(token))
            {
                return HttpNotFound();
            }

            if (!_tokenService.VerifyPasswordResetToken(userGuid.Value, token))
            {
                return View("ResetPasswordInvalidToken");
            }

            var resetPasswordPageViewModel = new ResetPasswordPageViewModel(page)
            {
                FormModel = new ResetPasswordFormModel
                {
                    UserGuid = userGuid.Value,
                    Token = token
                }
            };

            return View(resetPasswordPageViewModel);
        }

        private async Task<SignInStatus> TrySignIn(string username, string password)
        {
            // Attempts to authenticate the user against the Kentico database
            var signInResult = SignInStatus.Failure;

            try
            {
                signInResult = await _authenticationService.LogInAsync(username, password);
            }
            catch (Exception ex)
            {
                // Logs an error into the Kentico event log if the authentication fails
                _eventLogRepository.LogError(GetType(), nameof(TrySignIn), ex);
            }

            return signInResult;
        }

        // POST: Account/ResetPassword
        [HttpPost]
        [ValidateHeaderAntiForgeryToken]
        [ValidateModel]
        public async Task<ActionResult> ResetPassword(
            ResetPasswordFormModel model)
        {
            var identityResult = _authenticationService.ResetUserPassword(
                model.UserGuid, 
                model.Token, 
                model.Password);

            if (identityResult.Succeeded)
            {
                switch (model.SetPasswordMode)
                {
                    case SetPasswordMode.ResetPassword:

                        _emailService.SendPasswordResetConfirmationEmail(model.UserGuid);

                        return JsonContent(
                            new BaseWebResponse<object>
                            {
                                Status = WebResponseStatus.Success,
                            }
                        );

                    case SetPasswordMode.NewPassword:

                        var user = _userRepository.GetByGuid(model.UserGuid);

                        // Attempts to authenticate the user against the Kentico database
                        var signInResult = await TrySignIn(user.UserName, model.Password);

                        if (signInResult == SignInStatus.Success)
                        {
                            _membershipActivityLogger.LogLogin(user.UserName);

                            return JsonContent(
                                new BaseWebResponse<object>
                                {
                                    Status = WebResponseStatus.Success,
                                    RefreshOnSuccess = true
                                }
                            );
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _eventLogRepository.LogError(
                GetType(),
                nameof(ResetPassword),
                $"Could not reset user password: {string.Join(",", identityResult.Errors)}");

            return JsonContent(new BaseWebResponse<object>
            {
                Status = WebResponseStatus.Error,
            });
        }


        [HttpGet]
        public ActionResult NewUserSetupPassword(
            Guid? userGuid,
            string token)
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            //If Already logged in just go to the home page.
            if (_userRepository.CurrentUser != null)
            {
                return RedirectToPage(StandalonePageType.Home);
            }

            if (!userGuid.HasValue || string.IsNullOrEmpty(token))
            {
                return HttpNotFound();
            }

            if (!_tokenService.VerifyPasswordResetToken(userGuid.Value, token))
            {
                return View("NewUserSetupPasswordInvalidToken");
            }

            var resetPasswordPageViewModel = new ResetPasswordPageViewModel(page)
            {
                FormModel = new ResetPasswordFormModel
                {
                    UserGuid = userGuid.Value,
                    Token = token,
                    SetPasswordMode = SetPasswordMode.NewPassword
                }
            };

            return View(resetPasswordPageViewModel);
        }

        #endregion
    }
}

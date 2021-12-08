using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Services;
using System;
using System.Threading.Tasks;

namespace OslerAlumni.Mvc.Controllers
{
    [BasicAuthorize]
    public class InternalApiController
        : BaseController
    {
        #region "Private fields"

        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        #endregion

        public InternalApiController(
            IUserRepository userRepository,
            IPageUrlService pageUrlService,
            ITokenService tokenService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Do not modify this method or its name. It is serving as an API for the Admin site.
        /// 
        /// Given a user guid, this API method returns a password reset token for that user.
        /// This is done as an API since the Admin project doesn't have access to OWIN project.
        /// 
        /// See AdminUserRepository
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPasswordResetToken(
            Guid userGuid)
        {
            var user = _userRepository.GetByGuid(userGuid);

            var token = string.Empty;

            if (user != null && user.Enabled)
            {
                token =
                    await _tokenService.GeneratePasswordResetTokenAsync(user.UserGUID);
            }

            return JsonContent(new BaseWebResponse<string>()
            {
                Status = string.IsNullOrWhiteSpace(token) ? WebResponseStatus.Error : WebResponseStatus.Success,
                Result = token
            });
        }
    }
}

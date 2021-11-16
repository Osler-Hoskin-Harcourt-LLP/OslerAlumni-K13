using System.Linq;
using System.Web.Mvc;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /// <summary>
    /// Useful for hidding certain actions for the System User.
    /// This user is used for People who sign in via Osler Network.
    /// </summary>
    public class HideActionFromSystemUserAttribute
        : ActionFilterAttribute
    {
        #region "Properties"

        public IUserRepository UserRepository { get; set; }

        #endregion


        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            var user = UserRepository.CurrentUser;

            if (UserRepository.IsSystemUser(user.UserName))
            {
                filterContext.Result = new HttpNotFoundResult();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}

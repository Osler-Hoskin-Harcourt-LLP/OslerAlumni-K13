using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class RequestPasswordResetPostModel
    {
        [Required(ErrorMessage = Constants.ResourceStrings.Form.RequestPasswordReset.UserNameOrEmailRequired)]
        [Display(Name = Constants.ResourceStrings.Form.RequestPasswordReset.UserNameOrEmail)]
        public string UserNameOrEmail { get; set; }
    }
}

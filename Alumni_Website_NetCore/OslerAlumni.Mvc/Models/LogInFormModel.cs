using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class LogInFormModel
    {
        
        [Display(Name = Constants.ResourceStrings.Form.Login.UserName)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.Login.UserNameRequired)]
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = Constants.ResourceStrings.Form.Login.Password)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.Login.PasswordRequired)]

        public string Password { get; set; }
    }
}

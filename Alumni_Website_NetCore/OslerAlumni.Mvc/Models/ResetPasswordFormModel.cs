using System;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class ResetPasswordFormModel
    {
        [Required]
        public Guid UserGuid { get; set; }

        [Required]
        public string Token { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = Constants.ResourceStrings.Form.ResetPassword.Password)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ResetPassword.PasswordRequired)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name= Constants.ResourceStrings.Form.ResetPassword.PasswordConfirmation)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ResetPassword.PasswordConfirmationRequired)]
        [Compare(nameof(Password), ErrorMessage = Constants.ResourceStrings.Form.ResetPassword.PasswordConfirmationError)]
        public string PasswordConfirmation { get; set; }

        public SetPasswordMode SetPasswordMode { get; set; } = SetPasswordMode.ResetPassword;
    }
}

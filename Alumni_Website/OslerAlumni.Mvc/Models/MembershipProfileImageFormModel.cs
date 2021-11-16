using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class MembershipProfileImageFormModel
    {
        public Guid? UserGuid { get; set; }
        public string ProfileImageUrl { get; set; }

        [Required(ErrorMessage = Constants.ResourceStrings.Form.MembershipProfileImage.FileUploadRequired)]
        [Display(Name = Constants.ResourceStrings.Form.MembershipProfileImage.FileUpload)]
        [Description(Constants.ResourceStrings.Form.MembershipProfileImage.FileUploadExplanation)]
        [FileTypeValidation("png,jpg", ErrorMessage = Constants.ResourceStrings.Form.MembershipProfileImage.FileUploadError)]
        [MaxFileSizeValidation(ErrorMessage = Constants.ResourceStrings.Form.MembershipProfileImage.FileUploadError)]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUpload { get; set; }

        public int ImageX { get; set; }
        public int ImageY { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public Mode DisplayMode { get; set; }
    }

    public enum Mode
    {
        View,
        Edit
    }
}

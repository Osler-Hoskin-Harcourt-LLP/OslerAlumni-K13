using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class ResetPasswordPageViewModel
        : PageViewModel
    {
        public ResetPasswordFormModel FormModel { get; set; }

        public ResetPasswordPageViewModel(PageType_Page page)
            : base(page)
        {
        }
    }
}

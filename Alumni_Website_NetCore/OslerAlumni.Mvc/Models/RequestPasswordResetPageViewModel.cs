using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class RequestPasswordResetPageViewModel
        : PageViewModel
    {
        public RequestPasswordResetPostModel FormPostModel { get; set; }

        public RequestPasswordResetPageViewModel(PageType_Page page) : base(page)
        {
            FormPostModel = new RequestPasswordResetPostModel();
        }
    }
}

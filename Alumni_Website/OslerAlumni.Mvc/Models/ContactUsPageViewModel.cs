using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class ContactUsPageViewModel
        : PageViewModel
    {
        public bool IsAuthenticated { get; set; }
        public ContactUsPostModel FormPostModel { get; set; }

        public ContactUsPageViewModel(PageType_Page page) 
            : base(page)
        {

        }
    }
}

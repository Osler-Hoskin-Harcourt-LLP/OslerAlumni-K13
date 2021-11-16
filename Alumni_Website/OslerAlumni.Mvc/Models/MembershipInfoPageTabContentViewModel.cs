using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class MembershipInfoPageTabContentViewModel 
        : PageViewModel
    {
        public object FormPostModel { get; set; }

        public MembershipInfoPageTabContentViewModel(PageType_Page page)
            : base(page)
        {

        }
    }
}

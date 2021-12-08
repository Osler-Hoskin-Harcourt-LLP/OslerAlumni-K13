using System.Collections.Generic;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class MembershipInfoPageViewModel
        : PageViewModel
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public string ViewComponent { get;set; }

        public List<SubPagesNav> SubPageNavigation { get; set; }

        public string ProfileUrl { get; set; }

        public bool ShowCta { get; set; } = true;

        public bool IsFullWidth { get; set; }

        public MembershipInfoPageViewModel(PageType_Page page) 
            : base(page)
        { }
    }
}

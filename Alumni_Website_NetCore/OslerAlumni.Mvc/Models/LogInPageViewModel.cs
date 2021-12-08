using System.Collections.Generic;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class LogInPageViewModel : BasePageViewModel
    {
        public LogInFormModel FormPostModel { get; set; }

        public bool ShowLoginViaOslerNetwork { get; set; }

        public string ReturnUrl { get; set; }

        public LogInPageViewModel(PageType_Page page) : base(page)
        {
            FormPostModel = new LogInFormModel();
        }
    }
}

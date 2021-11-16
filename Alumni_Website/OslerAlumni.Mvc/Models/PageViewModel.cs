using CMS.DocumentEngine;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class PageViewModel
        : BasePageViewModel
    {
        #region "Properties"

        public bool HasSuccessState { get; set; }

        public string SuccessHeader { get; set; }

        public string SuccessDescription { get; set; }


        #endregion

        public PageViewModel(PageType_Page page)
            : base(page)
        {
            HasSuccessState = page.HasSuccessState;
            SuccessHeader = page.SuccessHeader;
            SuccessDescription = page.SuccessDescription;
            Page = page;
        }
    }
}

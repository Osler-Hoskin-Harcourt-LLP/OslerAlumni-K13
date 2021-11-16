using CMS.DocumentEngine;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class BasePageViewModel
        : IBasePageType
    {
        public string PageName { get; set; }

        public string Title { get; set; }
        public string DocumentPageTitle { get; set; }
        public string DocumentPageDescription { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string DefaultAction { get; set; }

        public string DefaultController { get; set; }

        public TreeNode Page { get; set; }

        protected BasePageViewModel(IBasePageType page)
        {
            PopulateBasePageFields(page);
        }

        /// <summary>
        /// Useful for auto populating the base page view model properties from a CMS Node
        /// </summary>
        /// <param name="page"></param>
        private void PopulateBasePageFields(
            IBasePageType page)
        {
            Title = page.Title;
            Description = page.Description;
            ShortDescription = page.ShortDescription;
            DocumentPageTitle = page.DocumentPageTitle;
            DocumentPageDescription = page.DocumentPageDescription;
        }
    }
}

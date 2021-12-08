using System.Collections.Generic;
using CMS.DocumentEngine;

namespace OslerAlumni.Core.Kentico.Models
{
    public interface IBasePageType
    {
        string DefaultAction { get; set; }

        string DefaultController { get; set; }

        string Description { get; set; }
        
        string PageName { get; set; }

        string ShortDescription { get; set; }

        string Title { get; set; }

        #region MetadataFields

        string DocumentPageTitle { get; }
        string DocumentPageDescription { get; }

        #endregion
    }
}

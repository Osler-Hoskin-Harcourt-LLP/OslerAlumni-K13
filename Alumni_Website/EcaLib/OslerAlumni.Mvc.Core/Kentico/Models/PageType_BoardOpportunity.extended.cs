using CMS;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Core.Kentico.Models;

[assembly: RegisterDocumentType(PageType_BoardOpportunity.CLASS_NAME, typeof(PageType_BoardOpportunity))]

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    /// <summary>
    /// Represents a content item of type PageType_BoardOpportunity.
    /// </summary>
    public partial class PageType_BoardOpportunity : IBasePageType

    {
    }
}

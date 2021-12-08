
using System;
using CMS;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Core.Kentico.Models;

[assembly: RegisterDocumentType(PageType_DevelopmentResource.CLASS_NAME, typeof(PageType_DevelopmentResource))]

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    /// <summary>
    /// Represents a content item of type PageType_DevelopmentResource.
    /// </summary>
    public partial class PageType_DevelopmentResource : IBasePageType

    {
        public string[] DevelopmentResourceTypeArray => DevelopmentResourceTypes.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
    }
}

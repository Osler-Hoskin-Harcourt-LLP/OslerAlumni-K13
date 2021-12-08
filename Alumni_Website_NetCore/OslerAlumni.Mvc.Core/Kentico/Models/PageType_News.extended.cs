using System;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    /// <summary>
	/// Represents a content item of type PageType_Page.
	/// </summary>
	public partial class PageType_News
        : IBasePageType
    {
        public NewsPageType NewsPageType =>
            (NewsPageType)Enum.Parse(typeof(NewsPageType), Type);
    }
}

using System;

namespace OslerAlumni.Mvc.Api.Models
{
    public interface ISearchable
    {
        string PageType { get; set; }

        Guid NodeGuid { get; set; }

        string Culture { get; set; }

        string Title { get; set; }

        string ShortDescription { get; set; }

        string PageUrl { get; set; }
    }
}

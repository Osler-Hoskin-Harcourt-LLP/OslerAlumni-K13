using System.Collections.Generic;
using CMS.DocumentEngine;

namespace OslerAlumni.Core.Kentico.Models
{
    public interface ICompetitorProtected
    {
        bool HideFromCompetitors { get; set; }
    }
}

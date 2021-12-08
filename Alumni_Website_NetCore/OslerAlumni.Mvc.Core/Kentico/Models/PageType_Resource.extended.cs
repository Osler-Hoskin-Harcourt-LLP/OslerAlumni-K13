using System;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    public partial class PageType_Resource
        : IBasePageType, ICompetitorProtected
    {
        public string[] TypeArray => Types.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
    }
}

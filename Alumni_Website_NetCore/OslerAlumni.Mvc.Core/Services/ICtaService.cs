using System.Collections.Generic;
using CMS.DocumentEngine;
using ECA.Core.Services;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface ICtaService
        : IService
    {
        IEnumerable<CtaViewModel> GetRelatedContentCtas(
            TreeNode page);

        IEnumerable<CtaViewModel> GetTopWidgetZoneCtasAsync(
            TreeNode page);
    }
}

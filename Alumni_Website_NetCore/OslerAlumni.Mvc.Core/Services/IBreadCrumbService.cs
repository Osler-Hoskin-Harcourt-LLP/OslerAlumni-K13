using System;
using System.Collections.Generic;
using ECA.Core.Services;
using ECA.Mvc.Navigation.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IBreadCrumbService : IService
    {
        IEnumerable<NavigationItem> GetBreadCrumbs(Guid pageGuid);
        IEnumerable<NavigationItem> GetBreadCrumbs(string nodeAliasPath);

    }
}

using System.Collections.Generic;
using ECA.Core.Repositories;
using ECA.Mvc.Navigation.Kentico.Models;

namespace ECA.Mvc.Navigation.Repositories
{
    public interface INavigationItemRepository
        : IRepository
    {
        IEnumerable<PageType_NavigationItem> GetNavigationItems(
            string path,
            bool includeProtected = false,
            string cultureName = null,
            string siteName = null,
            bool showProfilesAndPreferences = true);
    }
}

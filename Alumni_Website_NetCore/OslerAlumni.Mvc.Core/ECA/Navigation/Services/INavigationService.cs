using System.Collections.Generic;
using ECA.Core.Services;
using ECA.Mvc.Navigation.Definitions;
using ECA.Mvc.Navigation.Models;

namespace ECA.Mvc.Navigation.Services
{
    public interface INavigationService
        : IService
    {
        /// <summary>
        /// Gets the list of navigation items that should appear
        /// under a specific navigation section (<see cref="NavigationType"/>).
        /// </summary>
        /// <param name="navigationType">Navigation section (Primary, Secondary, etc.).</param>
        /// <param name="includeProtected">
        /// Indicates if protected navigation items should be returned.
        /// Normally, protected navigation items should only be displayed for authorized users.
        /// </param>
        /// <param name="pageTypeName">
        /// Optional parameter: Name of the Page Type that navigation items link to.
        /// If navigation items can link to pages of more than one Page Type, leave this parameter empty.
        /// </param>
        /// <returns>List of navigation items with the name of the item and its link URL.</returns>
        IEnumerable<NavigationItem> GetNavigation(
            NavigationType navigationType,
            bool includeProtected = false,
            string pageTypeName = null);
    }
}

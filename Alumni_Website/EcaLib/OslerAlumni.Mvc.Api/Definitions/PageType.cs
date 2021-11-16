
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Definitions
{
    public enum PageTypeFilter
    {
        [Display(Name = Constants.ResourceStrings.Search.PageTypeFilters.News)]
        [SearchPageType(PageType_News.CLASS_NAME)]
        News,
        [Display(Name = Constants.ResourceStrings.Search.PageTypeFilters.JobOpportunities)]
        [SearchPageType(PageType_Job.CLASS_NAME)]
        Jobs,
        [Display(Name = Constants.ResourceStrings.Search.PageTypeFilters.BoardOpportunities)]
        [SearchPageType(PageType_BoardOpportunity.CLASS_NAME)]
        BoardOpportunities,
        [Display(Name = Constants.ResourceStrings.Search.PageTypeFilters.Events)]
        [SearchPageType(PageType_Event.CLASS_NAME)]
        Events,
        [Display(Name = Constants.ResourceStrings.Search.PageTypeFilters.Profiles)]
        [SearchPageType(PageType_Profile.CLASS_NAME)]
        Profiles,
        [Display(Name = Constants.ResourceStrings.Search.PageTypeFilters.Resources)]
        [SearchPageType(PageType_Resource.CLASS_NAME)]
        Resources,
        [Display(Name = Constants.ResourceStrings.Search.PageTypeFilters.DevelopmentResources)]
        [SearchPageType(PageType_DevelopmentResource.CLASS_NAME)]
        DevelopmentResources
    }

}

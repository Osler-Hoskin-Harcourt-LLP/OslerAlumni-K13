using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace OslerAlumniWebsite.ViewComponents
{
    public abstract class BaseViewComponent : ViewComponent
    {
        protected readonly IPageUrlService _pageUrlService;

        protected readonly ContextConfig _context;

        protected readonly IPageDataContextRetriever _dataRetriever;


        protected BaseViewComponent(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
        {
            _pageUrlService = pageUrlService;
            _context = context;
            _dataRetriever = dataRetriever;
        }
    }
}

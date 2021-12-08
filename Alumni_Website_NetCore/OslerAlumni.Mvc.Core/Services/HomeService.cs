using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Services;
using ECA.PageURL.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class HomeService
        : ServiceBase, IHomeService
    {
        #region "Private fields"

        private readonly IBoardOpportunityService _boardOpportunityService;
        private readonly IEventsService _eventsService;
        private readonly IJobsService _jobsService;
        private readonly INewsService _newsService;
        private readonly IPageUrlService _pageUrlService;
        private readonly IResourceService _resourceService;
        private readonly ContextConfig _context;

        #endregion

        #region "Properties"


        #endregion

        public HomeService(
            IBoardOpportunityService boardOpportunityService,
            IEventsService eventsService,
            IJobsService jobsService,
            INewsService newsService,
            IPageUrlService pageUrlService,
            IResourceService resourceService,
            ContextConfig context)
        {
            _boardOpportunityService = boardOpportunityService;
            _eventsService = eventsService;
            _jobsService = jobsService;
            _newsService = newsService;
            _pageUrlService = pageUrlService;
            _resourceService = resourceService;
            _context = context;
        }

        #region "Methods"

        public List<HomePageFeaturedEventItem> GetFeaturedEventItems(int top, bool filterForCompetitor)
        {
            var featuredItems =
                GetFeaturedItems<PageType_Event, HomePageFeaturedEventItem>(
                    _eventsService.GetLatestEvents(top, filterForCompetitor),
                    (eventPage, item) =>
                    {
                        item.Description = eventPage.DisplayDate;
                        item.HostedByOsler = eventPage.HostedByOsler;
                        item.Url = "/"; //Events don't link to anything so this is needed so that they are not removed from the return.
                    });

            return featuredItems;
        }
        public List<HomePageFeaturedItem> GetFeaturedResourceItems(int top, bool filterForCompetitor)
        {
            var featuredItems =
            GetFeaturedItems<PageType_Resource, HomePageFeaturedItem>(
                _resourceService.GetLatestResources(top,filterForCompetitor),
                (resource, item) =>
                {
                    if (!string.IsNullOrWhiteSpace(resource.ExternalUrl))
                    {
                        item.Url = URLHelper.ResolveUrl(resource.ExternalUrl);
                        item.IsExternal = true;
                        item.IsFile = resource.IsFile;
                    }
                });

            return featuredItems;
        }

        public List<HomePageFeaturedItem> GetFeaturedBoardOpportunityItems(int top)
        {
            var featuredItems =

                GetFeaturedItems<PageType_BoardOpportunity, HomePageFeaturedItem>(
                    _boardOpportunityService.GetLatestBoardOpportunities(top),
                    (boardOpportunity, item) =>
                    {
                        if (!string.IsNullOrWhiteSpace(boardOpportunity.ExternalUrl))
                        {
                            item.Url = URLHelper.ResolveUrl(boardOpportunity.ExternalUrl);
                            item.IsExternal = true;
                        }

                        item.Description = ResHelper.GetStringFormat(Definitions.Constants.ResourceStrings.Jobs.Location,
                            boardOpportunity.BoardOpportunityLocation);
                    }
                );
            return featuredItems;
        }

        public List<HomePageFeaturedItem> GetFeaturedNewsItems(int top)
        {
            var featuredItems = GetFeaturedItems<PageType_News, HomePageFeaturedItem>(
                _newsService.GetLatestNews(top),
                (news, item) =>
                {
                    item.Description =
                        news.DatePublished.ToString(StringHelper.GetDateTimeFormat(_context.CultureName));
                }
            );
            return featuredItems;
        }

        public List<HomePageFeaturedItem> GetFeaturedJobItems(int top)
        {
            var featuredItems = GetFeaturedItems<PageType_Job, HomePageFeaturedItem>(
                _jobsService.GetLatestJobs(top),
                (job, item) =>
                {
                    if (!string.IsNullOrWhiteSpace(job.ExternalUrl))
                    {
                        item.Url = URLHelper.ResolveUrl(job.ExternalUrl);
                        item.IsExternal = true;
                    }

                    item.Description = ResHelper.GetStringFormat(Definitions.Constants.ResourceStrings.Jobs.Location,
                        job.JobLocation);
                }
            );


            return featuredItems;
        }

        private List<U> GetFeaturedItems<T, U>(List<T> pageItems, Action<T, U> configureFeaturedItem)
            where T : TreeNode
            where U : HomePageFeaturedItem, new()
        {
            var result =
                pageItems?.Select(item =>
                {
                    string url;

                    var featuredItem = new U()
                    {
                        Title = (item as IBasePageType)?.Title,
                    };

                    if (_pageUrlService.TryGetPageMainUrl(item, out url))
                    {
                        featuredItem.Url = url;
                    }

                    configureFeaturedItem(item, featuredItem);

                    if (string.IsNullOrWhiteSpace(featuredItem.Url))
                    {
                        //Remove non-linked items
                        return null;
                    }

                    return featuredItem;

                }).WhereNotNull()?.ToList();

            return result;
        }


        #endregion
    }
}

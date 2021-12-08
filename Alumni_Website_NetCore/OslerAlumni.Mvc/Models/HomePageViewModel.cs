using System;
using System.Collections.Generic;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class HomePageViewModel : BasePageViewModel
    {
        public bool IsAuthenticated { get; set; }

        #region Banner
        public HomeBannerLoggedInViewModel HomeBannerLoggedInViewModel { get; set; }

        #endregion

        #region LoggedOutBanner
        public HomeBannerLoggedOutViewModel HomeBannerLoggedOutViewModel { get; set; }
        
        #endregion

        #region Spotlight
        public string SpotlightHeader { get; set; }
        public string SpotlightDescription { get; set; }
        public string SpotlightImage { get; set; }
        public string SpotlightImageAltText { get; set; }
        public Guid? SpotlightLinkedPageGuid { get; set; }
        public string SpotlightLink { get; set; }
        public string SpotlightLinkText { get; set; }

        #endregion

        #region Alumni Moves

        public string AlumniMovesHeader { get; set; }
        public string AlumniMovesDescription { get; set; }
        public string AlumniMovesLink { get; set; }
        public string AlumniMovesLinkText { get; set; }
        public Guid? AlumniMovesLinkedPageGuid { get; set; }

        #endregion

        #region Community News

        public FeaturedCommunityNewsSectionViewModel FeaturedCommunityNewsSectionViewModel { get; set; }

        #endregion

        #region Job Opportunities

        public FeaturedJobOpportunitiesViewModel FeaturedJobOpportunitiesViewModel { get; set; }

        #endregion

        #region Board Opportunities

        public FeaturedBoardOpportunitiesViewModel FeaturedBoardOpportunitiesViewModel { get; set; }

        #endregion

        #region Events

        public FeaturedEventsViewModel FeaturedEventsViewModel { get; set; }

        #endregion

        #region Resources

        public FeaturedResourcesViewModel FeaturedResourcesViewModel { get; set; }

        #endregion

        #region Alumni in the media
        public string AlumniInTheMediaHeader { get; set; }

        public string AlumniInTheMediaDescription { get; set; }
        #endregion

        #region Twitter

        public TwitterViewModel TwitterViewModel { get; set; }


        #endregion

        public HomePageViewModel(PageType_Home page) : base(page)
        {
            #region Banner
            HomeBannerLoggedInViewModel = new HomeBannerLoggedInViewModel()
            {
                BannerImage1 = page.BannerImage1,
                BannerImage2 = page.BannerImage2,
                BannerImage3 = page.BannerImage3,
                BannerImageAltText = page.BannerImageAltText,
                BannerHeader = page.BannerHeader,
                BannerTitle = page.BannerTitle,
                BannerDescription = page.BannerDescription,
                BannerLinkText = page.BannerLinkText,
            };
            #endregion

            #region Logged Out Banner
            HomeBannerLoggedOutViewModel = new HomeBannerLoggedOutViewModel()
            {
                LoggedOutBannerHeader = page.LoggedOutBannerHeader,
                LoggedOutBannerDescription = page.LoggedOutBannerDescription,
                LoggedOutBannerImage1 = page.LoggedOutBannerImage1,
                LoggedOutBannerImage2 = page.LoggedOutBannerImage2,
                LoggedOutBannerImage3 = page.LoggedOutBannerImage3,
                LoggedOutBannerTitle = page.LoggedOutBannerTitle,
                LoggedOutBannerImageAltText = page.LoggedOutBannerImageAltText,
                LoggedOutBannerLinkText = page.LoggedOutBannerLinkText,
            };
            #endregion

            #region Spotlight

            SpotlightHeader = page.SpotlightHeader;
            SpotlightDescription = page.SpotlightDescription;
            SpotlightImage = page.SpotlightImage;
            SpotlightImageAltText = page.SpotlightImageAltText;
            SpotlightLinkedPageGuid = page.SpotlightLinkedPageGuid;
            SpotlightLinkText = page.SpotlightLinkText;

            #endregion

            #region Alumni Moves

            AlumniMovesHeader = page.AlumniMovesHeader;
            AlumniMovesDescription = page.AlumniMovesDescription;
            AlumniMovesLinkText = page.AlumniMovesLinkText;
            AlumniMovesLinkedPageGuid = page.AlumniMovesLinkedPageGuid;

            #endregion

            #region Twitter

            TwitterViewModel = new TwitterViewModel()
            {
                TwitterNumberOfItemsToDisplay = page.TwitterNumberOfItemsToDisplay,
                TwitterHandle = page.TwitterHandle
            };


            #endregion

            #region Alumni in the media

            AlumniInTheMediaHeader = page.AlumniInTheMediaHeader;
            AlumniInTheMediaDescription = page.AlumniInTheMediaDescription;

            #endregion

        }

    }
}

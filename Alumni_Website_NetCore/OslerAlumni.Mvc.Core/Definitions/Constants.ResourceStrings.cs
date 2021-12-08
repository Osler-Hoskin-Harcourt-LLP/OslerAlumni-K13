namespace OslerAlumni.Mvc.Core.Definitions
{
    public static partial class Constants
    {
        public static class ResourceStrings
        {
            public const string DateTimeFormat = "OslerAlumni.DateTimeFormat";

            public const string SkipLinkText = "OslerAlumni.SkipLinkText";
            public const string LanguageToggleText = "OslerAlumni.LanguageToggleText";
            public const string Send = "OslerAlumni.Send";
            public const string Submit = "OslerAlumni.Submit";
            public const string SaveEdits = "OslerAlumni.SaveEdits";
            public const string LogoLinkText = "OslerAlumni.LogoLinkText";
            public const string ListServerError = "OslerAlumni.List.GlobalServerError";
            public const string ListNoResultsMessage = "OslerAlumni.List.GlobalNoResultsMessage";
            public const string ListTotalResultsCountDisplay = "OslerAlumni.List.GlobalTotalResultsCountDisplay";
            public const string Loading = "OslerAlumni.Loading";
            public const string Copyright = "OslerAlumni.Copyright";
            public const string Lang = "OslerAlumni.LanguageAttribute";
            public const string Results = "OslerAlumni.Results";

            public const string RelatedContentHeader = "OslerAlumni.RelatedContentHeader";

            public static class ScreenReader
            {
                public const string ShareThisPage = "OslerAlumni.ScreenReader.ShareThisPage";
            }

            public static class Navigation
            {
                public const string OpenNavigation = "OslerAlumni.Navigation.OpenNavigation";
                public const string CloseNavigation = "OslerAlumni.Navigation.CloseNavigation";

                public const string Login = "OslerAlumni.Navigation.Login";
                public const string Logout = "OslerAlumni.Navigation.Logout";

                public const string NetworkLogin = "OslerAlumni.Navigation.NetworkLogin";
                public const string AltLoginText = "OslerAlumni.Navigation.NetworkLogin.AltLoginText";

                public const string Greeting = "OslerAlumni.Navigation.Greeting";
                public const string NetworkGreeting = "OslerAlumni.Navigation.NetworkGreeting";
                public const string IdentityConfirmation = "OslerAlumni.Navigation.IdentityConfirmation";
            }

            public static class EmailTemplates
            {
                public static class ContactUsNotificationInternalEmail
                {
                    public const string Subject = "OslerAlumni.EmailTemplates.ContactUsNotificationInternalEmail.Subject";
                }
            }
            public static class Home
            {
                public const string TwitterHeader = "OslerAlumni.Home.TwitterHeader";
                public const string TweetsBy = "OslerAlumni.Home.TweetsBy";
            }
                public static class Video
            {
                public const string TranscriptOpen = "OslerAlumni.Video.Transcript.Open";
                public const string TranscriptClose = "OslerAlumni.Video.Transcript.Close";
                public const string TranscriptLabel = "OslerAlumni.Video.Transcript.Label";
            }
            public static class Social
            {
                public const string LinkedinLink = "OslerAlumni.Social.Link.Linkedin";
                public const string YoutubeLink = "OslerAlumni.Social.Link.Youtube";
                public const string TwitterLink = "OslerAlumni.Social.Link.Twitter";
                public const string LinkedinLinkText = "OslerAlumni.Social.LinkText.Linkedin";
                public const string YoutubeLinkText = "OslerAlumni.Social.LinkText.Youtube";
                public const string TwitterLinkText = "OslerAlumni.Social.LinkText.Twitter";
            }

            #region "Site sections"

            public static class ProfileAndPreferences
            {
                public const string CTATitle = "OslerAlumni.ProfileAndPreferences.CTATitle";
                public const string CTA = "OslerAlumni.ProfileAndPreferences.CTA";
            }

            public static class News
            {
                public const string FeaturedNews = "OslerAlumni.News.Featured.Title.News";
                public const string FeaturedSpotlight = "OslerAlumni.News.Featured.Title.Spotlight";
                public const string FeaturedLinkText = "OslerAlumni.News.Featured.LinkText";
                public const string SpotlightVitalsHeader = "OslerAlumni.News.SpotlightVitalsHeader";
                public const string SpotlightVitalsDefaultHeader = "OslerAlumni.News.SpotlightVitalsHeader.Default";
                public const string SpotlightStoryHighlightsHeader = "OslerAlumni.News.SpotlightStoryHighlightsHeader";
                public const string MostRecentHeader = "OslerAlumni.News.MostRecent.Header";
                public const string NoListingsMessage = "OslerAlumni.News.NoListingsMessage";
            }

            public static class Events
            {
                public const string HostedByOsler = "OslerAlumni.Events.HostedByOsler";
                public const string NoListingsMessage = "OslerAlumni.Events.NoListingsMessage";

                public static class Attendees
                {
                    public const string CloseEventAttendeesList = "OslerAlumni.Events.Attendees.CloseEventAttendeesList";
                    public const string SeeWhosComing = "OslerAlumni.Events.Attendees.SeeWhosComing";
                    public const string PeopleComing = "OslerAlumni.Events.Attendees.PeopleComing";
                    public const string SearchPlaceholderText = "OslerAlumni.Events.Attendees.SearchPlaceholderText";
                }
            }

            public static class Search
            {
                public static class PageTypeFilters
                {
                    public const string Title = "OslerAlumni.Search.PageTypeFilters.Title";
                    public const string JobOpportunities = "OslerAlumni.Search.PageTypeFilters.JobOpportunities";
                    public const string BoardOpportunities = "OslerAlumni.Search.PageTypeFilters.BoardOpportunities";
                    public const string News = "OslerAlumni.Search.PageTypeFilters.News";
                    public const string Events = "OslerAlumni.Search.PageTypeFilters.Events";
                    public const string Profiles = "OslerAlumni.Search.PageTypeFilters.Profiles";
                    public const string Resources = "OslerAlumni.Search.PageTypeFilters.Resources";
                    public const string DevelopmentResources = "OslerAlumni.Search.PageTypeFilters.DevelopmentResources";
                }
            }
            public static class SearchResults
            {
                public const string KeywordSearch = "OslerAlumni.SearchResults.KeywordSearch";
                public const string ClearKeywords = "OslerAlumni.SearchResults.ClearKeywords";

            }

            public static class Jobs
            {
                public static class JobClassification
                {
                    public const string Osler = "OslerAlumni.Jobs.JobClassification.Osler";
                    public const string Marketplace = "OslerAlumni.Jobs.JobClassification.Marketplace";
                }

                public const string Company = "OslerAlumni.Jobs.Company";
                public const string Location = "OslerAlumni.Jobs.Location";
                public const string DatePosted = "OslerAlumni.Jobs.DatePosted";
                public const string Category = "OslerAlumni.Jobs.Category";
                public const string DatePostedAndCategory = "OslerAlumni.Jobs.DatePostedAndCategory";
                public const string JobsListHeader = "OslerAlumni.Jobs.JobsListHeader";
                public const string JobLocationPlaceholder = "OslerAlumni.Jobs.JobLocationPlaceholder";
                public const string SearchJobsHeader = "OslerAlumni.Jobs.SearchJobs.Header";
                public const string SearchDirections = "OslerAlumni.Jobs.SearchJobs.SearchDirections";
                public const string NoListingsMessage = "OslerAlumni.Jobs.NoListingsMessage";


                public static class SearchFilters
                {
                    public const string JobClassificationTitle = "OslerAlumni.Jobs.SearchFilters.Filters.JobClassificationTitle";

                    public const string JobLocationTitle = "OslerAlumni.Jobs.SearchFilters.Filters.JobLocationTitle";

                    public const string JobCategoryTitle = "OslerAlumni.Jobs.SearchFilters.Filters.JobCategoryTitle";
                }

            }

            public static class BoardOpportunity
            {
                public const string BoardOpportunityType = "OslerAlumni.BoardOpportunity.BoardOpportunityType";
                public const string Company = "OslerAlumni.BoardOpportunity.Company";
                public const string Location = "OslerAlumni.BoardOpportunity.Location";
                public const string DatePosted = "OslerAlumni.BoardOpportunity.DatePosted";
                public const string Source = "OslerAlumni.BoardOpportunity.Source";
                public const string DatePostedAndCategory = "OslerAlumni.BoardOpportunity.DatePostedAndCategory";
                public const string BoardOpportunityListHeader = "OslerAlumni.BoardOpportunity.BoardOpportunityListHeader";
                public const string SearchBoardOpportunityHeader = "OslerAlumni.BoardOpportunity.SearchBoardOpportunity.Header";
                public const string SearchDirections = "OslerAlumni.BoardOpportunity.SearchBoardOpportunity.SearchDirections";
                public const string NoListingsMessage = "OslerAlumni.BoardOpportunity.NoListingsMessage";

                public static class SearchFilters
                {
                    public const string LocationTitle = "OslerAlumni.BoardOpportunity.Filters.LocationTitle";

                    public const string LocationPlaceHolder = "OslerAlumni.BoardOpportunity.Filters.LocationPlaceHolder";

                    public const string BoardOpportunityTypeTitle = "OslerAlumni.BoardOpportunity.Filters.BoardOpportunityTypeTitle";

                    public const string IndustryTitle = "OslerAlumni.BoardOpportunity.Filters.IndustryTitle";
                }
            }

            public static class Resource
            {
                public const string FeaturedResource = "OslerAlumni.Resource.Featured.Title.Resource";
                public const string FeaturedLinkText = "OslerAlumni.Resource.Featured.LinkText";
                public const string SearchResourcesHeader = "OslerAlumni.Resource.SearchResources.Header";
                public const string SearchBoxPlaceholder = "OslerAlumni.Resource.SearchResources.SearchBoxPlaceholder";
                public const string SearchButtonText = "OslerAlumni.Resource.SearchResources.SearchButtonText";
                public const string SearchDirectionsText = "OslerAlumni.Resource.SearchResources.SearchDirectionsText";
                public const string FiltersHeader = "OslerAlumni.Resource.SearchResources.FiltersHeader";
                public const string ClearFiltersText = "OslerAlumni.Resource.SearchResources.ClearFiltersText";
                public const string ClearAllFiltersText = "OslerAlumni.Resource.SearchResources.ClearAllFiltersText";
                public const string ApplyFiltersText = "OslerAlumni.Resource.SearchResources.ApplyFiltersText";
                public const string ResourceListHeader = "OslerAlumni.Resource.ResourceListHeader";
                public const string RelatedContentHeader = "OslerAlumni.Resource.RelatedContentHeader";
                public const string CategoriesFilterTitle = "OslerAlumni.Resource.SearchResources.Filters.Categories";
                public const string ResourceDate = "OslerAlumni.Resource.Detail.Date";
                public const string ResourceAuthor = "OslerAlumni.Resource.Detail.Author";
                public const string ResourceCategory = "OslerAlumni.Resource.Detail.Category";
                public const string NoListingsMessage = "OslerAlumni.Resource.NoListingsMessage";
            }
            public static class DevelopmentResource
            {

                public const string FeaturedDevelopmentResource = "OslerAlumni.DevelopmentResource.Featured.Title.Resource";
                public const string SearchDevelopmentResourcesHeader = "OslerAlumni.DevelopmentResource.SearchResources.Header";
                public const string SearchDevelopmentResourcesDirectionsText = "OslerAlumni.DevelopmentResource.SearchResources.SearchDirectionsText";
                public const string DevelopmentResourceListHeader = "OslerAlumni.DevelopmentResource.ResourceListHeader";
                public const string NoListingsMessage = "OslerAlumni.DevelopmentResource.NoListingsMessage";

                public const string CategoriesFilterTitle = "OslerAlumni.DevelopmentResource.SearchResources.Filters.Categories";
            }

            public static class Profile
            {
                public const string BasicInformation = "OslerAlumni.Profile.BasicInformation.Header";
                public const string CityLabel = "OslerAlumni.Profile.Label.City";
                public const string ProvinceLabel = "OslerAlumni.Profile.Label.Province";
                public const string CountryLabel = "OslerAlumni.Profile.Label.Country";
                public const string JobTitleLabel = "OslerAlumni.Profile.Label.JobTitle";
                public const string CompanyLabel = "OslerAlumni.Profile.Label.Company";
                public const string YearofCallLabel = "OslerAlumni.Profile.Label.BarAdmission";
                public const string EducationHistoryLabel = "OslerAlumni.Profile.Label.EducationHistoryLabel";
                public const string IndustriesLabel = "OslerAlumni.Profile.Label.Industries";
                public const string WhileAtOsler = "OslerAlumni.Profile.WhileAtOsler.Header";
                public const string LocationsLabel = "OslerAlumni.Profile.Label.Locations";
                public const string YearsAtOslerLabel = "OslerAlumni.Profile.Label.YearsAtOslerLabel";
                public const string PracticeAreasLabel = "OslerAlumni.Profile.Label.PracticeAreas";
                public const string BoardMembershipsHeader = "OslerAlumni.Profile.BoardMemberships.Header";
                public const string BoardMembershipsLabel = "OslerAlumni.Profile.BoardMemberships.Label";
                public const string SearchProfilesHeader = "OslerAlumni.Profile.SearchProfiles.Header";
                public const string SearchDirectory = "OslerAlumni.Profile.SearchProfiles.Directory";
                public const string SearchBoxPlaceholder = "OslerAlumni.Profile.SearchProfiles.SearchBoxPlaceholder";
                public const string SearchButtonText = "OslerAlumni.Profile.SearchProfiles.SearchButtonText";
                public const string SearchDirections = "OslerAlumni.Profile.SearchProfiles.SearchDirections";
                public const string FiltersHeader = "OslerAlumni.Profile.SearchProfiles.FiltersHeader";
                public const string ClearFiltersText = "OslerAlumni.Profile.SearchProfiles.ClearFiltersText";
                public const string LocationFilter = "OslerAlumni.Profile.SearchProfiles.LocationFilter";
                public const string LocationFilterPlaceholder = "OslerAlumni.Profile.SearchProfiles.LocationFilterPlaceholder";
                public const string YearOfCallFilter = "OslerAlumni.Profile.SearchProfiles.BarAdmissionFilter";
                public const string YearOfCallFilterPlaceholder = "OslerAlumni.Profile.SearchProfiles.BarAdmissionFilterPlaceholder";
                public const string JurisdictionFilter = "OslerAlumni.Profile.SearchProfiles.JurisdictionFilter";
                public const string IndustryFilter = "OslerAlumni.Profile.SearchProfiles.IndustryFilter";
                public const string PracticeAreaFilter = "OslerAlumni.Profile.SearchProfiles.PracticeAreaFilter";
                public const string OfficeLocationFilter = "OslerAlumni.Profile.SearchProfiles.OfficeLocationFilter";
                public const string FilterButtonText = "OslerAlumni.Profile.SearchProfiles.FilterButtonText";
                public const string ViewProfile = "OslerAlumni.Profile.ViewProfileLink";
                public const string NoListingsMessage = "OslerAlumni.Profile.NoListingsMessage";
            }

            #endregion

            #region "Standalone pages"

            public static class ResetPassword
            {
                public const string InvalidTokenHeader = "OslerAlumni.ResetPassword.InvalidToken.Header";
                public const string InvalidTokenDescription = "OslerAlumni.ResetPassword.InvalidToken.Description";
                public const string InvalidTokenLinkText = "OslerAlumni.ResetPassword.InvalidToken.LinkText";
            }

            public static class NewUserLoginPage
            {
                public const string InvalidTokenHeader = "OslerAlumni.NewUserLoginPage.InvalidToken.Header";
                public const string InvalidTokenDescription = "OslerAlumni.NewUserLoginPage.InvalidToken.Description";
                public const string InvalidTokenLinkText = "OslerAlumni.NewUserLoginPage.InvalidToken.LinkText";
            }

            public static class ContactUs
            {
                public const string ContactUsNotLoggedInMessage = "OslerAlumni.ContactUs.ContactUsNotLoggedInMessage";
            }

            #endregion

            #region "Forms"

            public static class Form
            {
                public const string GlobalServerError = "OslerAlumni.Form.GlobalServerError";
                public const string GlobalError = "OslerAlumni.Form.GlobalError";
                public const string GlobalErrorTitle = "OslerAlumni.Form.GlobalError.Title";
                public const string GlobalMaxLengthError = "OslerAlumni.Form.GlobalMaxLengthError";
                public const string GlobalCaptchaRequired = "OslerAlumni.Form.GlobalCaptchaRequired";
                public const string GlobalCaptchaError = "OslerAlumni.Form.GlobalCaptchaError";
                public const string GlobalSearchPlaceholderText = "OslerAlumni.Form.GlobalSearchPlaceholderText";
                public const string GlobalSearchButtonText = "OslerAlumni.Form.GlobalSearchButtonText";

                public const string GlobalFileUploadButtonText = "OslerAlumni.Form.GlobalFileUploadButtonText";

                public static class Login
                {
                    public const string UserName = "OslerAlumni.Form.Login.UserName";
                    public const string UserNameRequired = "OslerAlumni.Form.Login.UserName.Required";

                    public const string Password = "OslerAlumni.Form.Login.Password";
                    public const string PasswordRequired = "OslerAlumni.Form.Login.Password.Required";

                    public const string UserNameOrPasswordIncorrect = "OslerAlumni.Form.Login.UserNameOrPasswordIncorrect";

                    public const string ForgotPassword = "OslerAlumni.Form.Login.ForgotPassword";
                }

                public static class RequestPasswordReset
                {
                    public const string UserNameOrEmail = "OslerAlumni.Form.RequestPasswordReset.UserNameOrEmail";
                    public const string UserNameOrEmailRequired = "OslerAlumni.Form.RequestPasswordReset.UserNameOrEmail.Required";
                }

                public static class ResetPassword
                {
                    public const string Password = "OslerAlumni.Form.ResetPassword.Password";
                    public const string PasswordRequired = "OslerAlumni.Form.ResetPassword.Password.Required";
                    public const string PasswordError = "OslerAlumni.Form.ResetPassword.Password.Error";

                    public const string PasswordConfirmation = "OslerAlumni.Form.ResetPassword.PasswordConfirmation";
                    public const string PasswordConfirmationRequired = "OslerAlumni.Form.ResetPassword.PasswordConfirmation.Required";
                    public const string PasswordConfirmationError = "OslerAlumni.Form.ResetPassword.PasswordConfirmation.Error";
                }

                public static class ContactUs
                {
                    public const string OpportunityType = "OslerAlumni.Form.ContactUs.OpportunityType";
                    public const string OpportunityTypeRequired = "OslerAlumni.Form.ContactUs.OpportunityType.Required";

                    public const string OpportunityTypeDefaultOption = "OslerAlumni.Form.ContactUs.OpportunityType.DefaultOption";
                    public const string OpportunityTypeJob = "OslerAlumni.Form.ContactUs.OpportunityType.Job";
                    public const string OpportunityTypeBoard = "OslerAlumni.Form.ContactUs.OpportunityType.Board";


                    public const string ReasonForContactingUs = "OslerAlumni.Form.ContactUs.ReasonForContactingUs";
                    public const string ReasonForContactingUsRequired = "OslerAlumni.Form.ContactUs.ReasonForContactingUs.Required";


                    public const string ReasonForContactingUsGeneralInquiry = "OslerAlumni.Form.ContactUs.ReasonForContactingUs.GeneralInquiry";
                    public const string ReasonForContactingUsSubmitNews = "OslerAlumni.Form.ContactUs.ReasonForContactingUs.SubmitNews";
                    public const string ReasonForContactingUsPostAnOpportunity = "OslerAlumni.Form.ContactUs.ReasonForContactingUs.PostAnOpportunity";


                    public const string FirstName = "OslerAlumni.Form.ContactUs.FirstName";
                    public const string FirstNameRequired = "OslerAlumni.Form.ContactUs.FirstName.Required";

                    public const string LastName = "OslerAlumni.Form.ContactUs.LastName";
                    public const string LastNameRequired = "OslerAlumni.Form.ContactUs.LastName.Required";

                    public const string CompanyName = "OslerAlumni.Form.ContactUs.CompanyName";
                    public const string CompanyNameRequired = "OslerAlumni.Form.ContactUs.CompanyName.Required";

                    public const string PhoneNumber = "OslerAlumni.Form.ContactUs.PhoneNumber";
                    public const string PhoneNumberError = "OslerAlumni.Form.ContactUs.PhoneNumber.Error";
                    public const string PhoneNumberRequired = "OslerAlumni.Form.ContactUs.PhoneNumber.Required";


                    public const string Email = "OslerAlumni.Form.ContactUs.Email";
                    public const string EmailRequired = "OslerAlumni.Form.ContactUs.Email.Required";
                    public const string EmailError = "OslerAlumni.Form.ContactUs.Email.Error";

                    public const string Subject = "OslerAlumni.Form.ContactUs.Subject";
                    public const string SubjectError = "OslerAlumni.Form.ContactUs.Subject.Error";

                    public const string Message = "OslerAlumni.Form.ContactUs.Message";
                    public const string MessageRequired = "OslerAlumni.Form.ContactUs.Message.Required";
                    public const string MessageError = "OslerAlumni.Form.ContactUs.Message.Error";


                    public const string FileUpload = "OslerAlumni.Form.ContactUs.FileUpload";
                    public const string FileUploadError = "OslerAlumni.Form.ContactUs.FileUpload.Error";
                    public const string FileUploadExplanation = "OslerAlumni.Form.ContactUs.FileUpload.Explanation";


                    public const string ConfirmationCTAMessage = "OslerAlumni.Form.ContactUs.ConfirmationCTAMessage";
                    public const string ConfirmationCTALink = "OslerAlumni.Form.ContactUs.ConfirmationCTALink";
                }

                public static class MembershipBasicInfo
                {
                    public const string FirstName = "OslerAlumni.Form.MembershipBasicInfo.FirstName";
                    public const string FirstNameRequired = "OslerAlumni.Form.MembershipBasicInfo.FirstName.Required";

                    public const string LastName = "OslerAlumni.Form.MembershipBasicInfo.LastName";
                    public const string LastNameRequired = "OslerAlumni.Form.MembershipBasicInfo.LastName.Required";

                    public const string CompanyName = "OslerAlumni.Form.MembershipBasicInfo.CompanyName";
                    public const string CompanyNameRequired = "OslerAlumni.Form.MembershipBasicInfo.CompanyName.Required";


                    public const string JobTitle = "OslerAlumni.Form.MembershipBasicInfo.JobTitle";
                    public const string JobTitleRequired = "OslerAlumni.Form.MembershipBasicInfo.JobTitle.Required";

                    public const string City = "OslerAlumni.Form.MembershipBasicInfo.City";
                    public const string CityRequired = "OslerAlumni.Form.MembershipBasicInfo.City.Required";

                    public const string UserName = "OslerAlumni.Form.MembershipBasicInfo.UserName";

                    public const string Province = "OslerAlumni.Form.MembershipBasicInfo.Province";

                    public const string Country = "OslerAlumni.Form.MembershipBasicInfo.Country";


                    public const string Email = "OslerAlumni.Form.MembershipBasicInfo.Email";
                    public const string EmailRequired = "OslerAlumni.Form.MembershipBasicInfo.Email.Required";


                    public const string EmailError = "OslerAlumni.Form.MembershipBasicInfo.Email.Error";


                    public const string AlumniEmail = "OslerAlumni.Form.MembershipBasicInfo.AlumniEmail";


                    public const string BarAdmissionAndJurisdiction = "OslerAlumni.Form.MembershipBasicInfo.BarAdmissionAndJurisdiction";
                    public const string BarAdmissionAndJurisdictionEmpty = "OslerAlumni.Form.MembershipBasicInfo.BarAdmissionAndJurisdiction.Empty";
                    public const string EducationHistory = "OslerAlumni.Form.MembershipBasicInfo.EducationHistory";
                    public const string EducationHistoryEmpty = "OslerAlumni.Form.MembershipBasicInfo.EducationHistory.Empty";
                    public const string CurrentIndustry = "OslerAlumni.Form.MembershipBasicInfo.CurrentIndustry";
                    public const string CurrentIndustryEmpty = "OslerAlumni.Form.MembershipBasicInfo.CurrentIndustry.Empty";

                    public const string YearOfCallAndJurisdictionFormat = "OslerAlumni.Form.MembershipBasicInfo.BarAdmissionAndJurisdictionFormat";

                    public const string LinkedInUrl = "OslerAlumni.Form.MembershipBasicInfo.LinkedInUrl";
                    public const string LinkedInUrlExplanation = "OslerAlumni.Form.MembershipBasicInfo.LinkedInUrl.Explanation";
                    public const string LinkedInUrlError = "OslerAlumni.Form.MembershipBasicInfo.LinkedInUrl.Error";

                    public const string TwitterUrl = "OslerAlumni.Form.MembershipBasicInfo.TwitterUrl";
                    public const string TwitterUrlExplanation = "OslerAlumni.Form.MembershipBasicInfo.TwitterUrl.Explanation";
                    public const string TwitterUrlError = "OslerAlumni.Form.MembershipBasicInfo.TwitterUrl.Error";

                    public const string InstagramUrl = "OslerAlumni.Form.MembershipBasicInfo.InstagramUrl";
                    public const string InstagramUrlExplanation = "OslerAlumni.Form.MembershipBasicInfo.InstagramUrl.Explanation";
                    public const string InstagramUrlError = "OslerAlumni.Form.MembershipBasicInfo.InstagramUrl.Error";
                }

                public static class MembershipProfileImage
                {
                    public const string FileUploadTitle = "OslerAlumni.Form.MembershipProfileImage.FileUploadTitle";
                    public const string FileUpload = "OslerAlumni.Form.MembershipProfileImage.FileUpload";
                    public const string FileUploadExplanation = "OslerAlumni.Form.MembershipProfileImage.FileUploadExplanation";
                    public const string FileUploadError = "OslerAlumni.Form.MembershipProfileImage.FileUpload.Error";
                    public const string FileUploadRequired = "OslerAlumni.Form.MembershipProfileImage.FileUpload.Required";
                    public const string EditImageText = "OslerAlumni.Form.MembershipProfileImage.EditImageText";
                    public const string DeleteImageText = "OslerAlumni.Form.MembershipProfileImage.DeleteImageText";
                    public const string MobileText = "OslerAlumni.Form.MembershipProfileImage.MobileText";
                }
                public static class MembershipPreferences
                {
                    public const string IncludeEmailInDirectory = "OslerAlumni.Form.MembershipPreferences.IncludeEmailInDirectory";
                    public const string DisplayImageInDirectory = "OslerAlumni.Form.MembershipPreferences.DisplayImageInDirectory";
                    public const string SubscribeToEmailUpdates = "OslerAlumni.Form.MembershipPreferences.SubscribeToEmailUpdates";
                }

                public static class MembershipBoards
                {
                    public const string BoardMemberships = "OslerAlumni.Form.MembershipBoards.BoardMemberships";

                    public const string NoBoards = "OslerAlumni.Form.MembershipBoards.NoBoards";

                    public const string NewBoard = "OslerAlumni.Form.MembershipBoards.NewBoard";
                    public const string NewBoardRequired = "OslerAlumni.Form.MembershipBoards.NewBoard.Required";

                    public const string NewBoardIllegalCharacter = "OslerAlumni.Form.MembershipBoards.NewBoard.NewBoardIllegalCharacter";

                    public const string NewBoardDuplicate = "OslerAlumni.Form.MembershipBoards.NewBoard.Duplicate";
                    public const string NewBoardMaxLengthError = "OslerAlumni.Form.MembershipBoards.NewBoard.MaxLengthError";
                    public const string NewBoardMaxLimitReached = "OslerAlumni.Form.MembershipBoards.NewBoard.NewBoardMaxLimitReached";

                    public const string AddCompanyName = "OslerAlumni.Form.MembershipBoards.AddCompanyName";
                }

                public static class OslerInformation
                {
                    public const string YearsAtOsler = "OslerAlumni.Form.OslerInformation.YearsAtOsler";
                    public const string YearsAtOslerEmpty = "OslerAlumni.Form.OslerInformation.YearsAtOsler.Empty";
                    public const string OslerLocations = "OslerAlumni.Form.OslerInformation.OslerLocations";
                    public const string OslerPracticeAreas = "OslerAlumni.Form.OslerInformation.OslerPracticeAreas";

                    public const string OslerLocationsEmpty = "OslerAlumni.Form.OslerInformation.OslerLocations.Empty";
                    public const string OslerPracticeAreasEmpty = "OslerAlumni.Form.OslerInformation.OslerPracticeAreas.Empty";
                }
            }

            #endregion

        }
    }
}

using ECA.Core.Definitions;

namespace OslerAlumni.Core.Definitions
{
    public static partial class GlobalConstants
    {
        public static class Caching
        {
            public const string Prefix = "OslerAlumni.Custom.";
            public const string Separator = ECAGlobalConstants.Caching.Separator;
            public const string All = ECAGlobalConstants.Caching.All;
            
            public static class Configuration
            {
                public const string ConfigByType = Prefix + "Configuration|Configs|bytype|{0}";
                public const string WebConfigSettingBySection = Prefix + "Configuration|WebConfigSettings|bysection|{0}";
                public const string WebConfigSettingBySectionAndName = Prefix + "Configuration|WebConfigSettings|bysection|{0}|byname|{1}";
            }

            public static class Emails
            {
                public const string EmailTemplateByName = Prefix + "Emails|EmailTemplates|byname|{0}";
            }

            public static class Events
            {
                public const string LocationItemsAll = Prefix + "Events|LocationItems|" + All;
            }

            public static class Home
            {
                public const string HomeTilesByPage = Prefix + "Home|HomeTiles|bypage|{0}";
            }

            public static class Jobs
            {
                public const string JobCategoryItemsAll = Prefix + "Jobs|JobCategoryItems|" + All;
            }

            public static class BoardOpportunities
            {
                public const string BoardOpportunityItemsAll = Prefix + "BoardOpportunities|BoardOpportunityItems|" + All;
                public const string BoardOpportunitySourceItemsAll = Prefix + "BoardOpportunities|BoardOpportunitySourceItems|" + All;
            }

            public static class News
            {
                public const string FeaturedItemByLandingPage = Prefix + "News|FeaturedItem|bylandingpage|{0}";
            }

            /// <summary>
            /// Cache keys to be used with pages of PageType_Page page type (not just any page).
            /// </summary>
            public static class GenericPages
            {
                public const string SubPagesByPage = Prefix + "Pages|SubPages|bypage|{0}";
            }

            public static class Resources
            {
                public const string FeaturedItemByLandingPage = Prefix + "Resources|FeaturedItem|bylandingpage|{0}";
                public const string ResourceTypeItemsAll = Prefix + "Resources|ResourceTypeItems|" + All;
            }

            public static class CTA
            {
                public const string CTAByPageAndFieldName = Prefix + "CTA|bypage|{0}|byfieldname|{1}|filterForCompetitors{2}";
            }


            public static class DevelopmentResources
            {
                public const string FeaturedItemByLandingPage = Prefix + "DevelopmentResources|FeaturedItem|bylandingpage|{0}";
                public const string DevelopmentResourceTypeItemsAll = Prefix + "DevelopmentResources|DevelopmentResourceTypeItems|" + All;
            }

            public static class Search
            {
                public const string SearchIndexByName = Prefix + "Search|SearchIndexes|byname|{0}";
                public const string SearchIndexesByType = Prefix + "Search|SearchIndexes|bytype|{0}";
                public const string SearchResultsBySearchRequest =
                    Prefix + "Search|SearchResults|bypagetypes|{0}|byindex|{1}|pagesize|{2}|skip|{3}|orderby|{4}|{5}|excludednodeguids|{6}|filterForCompetitors|{7}";
                public const string SearchResultsBySearchRequestWithFitlers =
                    Prefix + "Search|SearchResults|bypagetypes|{0}|byindex|{1}|pagesize|{2}|skip|{3}|orderby|{4}|{5}|excludednodeguids|{6}|filterForCompetitors|{7}|includefilters|{8}";

                public const string SearchResultsByOnePlaceType = Prefix + "Search|SearchResults|byoneplacetypes|{0}";
                public const string SearchResultsByOnePlaceTypeAndId = Prefix + "Search|SearchResults|byoneplacetypes|{0}|id|{1}";
            }

            public static class ResourceStrings
            {
                public const string ResourceStringById = Prefix + "ResourceString|byId|{0}";
                public const string ResourceStringByName = Prefix + "ResourceString|byName|{0}";
                public const string ResourceStringByPrefix = Prefix + "ResourceString|byPrefix|{0}";
                public const string ResourceStringByIds = Prefix + "ResourceString|byIds|{0}";
            }

            public static class Pages
            {
                public const string PagesByType = "nodes|{0}|{1}|all"; //{0 : sitename} {1: pagetype}
            }
        }
    }
}

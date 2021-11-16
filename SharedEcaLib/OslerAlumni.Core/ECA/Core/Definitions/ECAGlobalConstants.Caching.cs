namespace ECA.Core.Definitions
{
    public static partial class ECAGlobalConstants
    {
        public static class Caching
        {
            public const string Prefix = "ECA.Custom.";
            public const string Separator = "|";
            public const string All = "all";

            public static class Classes
            {
                public const string ClassesByNamesAndColumns = Prefix + "Classes|bynames|{0}|columns|{1}";
                public const string IsChildClass = Prefix + "Classes|IsChildClass|byname|{0}|parent|{1}";
                public const string RelationshipByClassAndField = Prefix + "Classes|Relationships|byclass|{0}|byfield|{1}";
            }
            
            public static class Navigation
            {
                public const string NavigationByTypeAndPageType = Prefix + "Navigation|bytype|{0}|includeprotected|{1}|pagetype|{2}|loggedInViaOlserNetwork|{3}";
                public const string NavigationItemsByPath = Prefix + "Navigation|NavigationItems|bypath|{0}|includeprotected|{1}|loggedInViaOlserNetwork|{2}";
            }

            /// <summary>
            /// Cache keys to be used with any type of pages.
            /// </summary>
            public static class Pages
            {
                public const string PageByNodeGuidAndColumns = Prefix + "Pages|Page|bynodeguid|{0}|columns|{1}|includeallcoupledcolumns|{2}";
                public const string PageByNodeAliasPath = Prefix + "Pages|Page|bynodealiaspath|{0}";
            }

            public static class PageUrls
            {
                public const string UrlItemByPath = Prefix + "PageUrls|UrlItems|bypath|{0}";
                public const string MainUrlItemByNodeGuid = Prefix + "PageUrls|UrlItems|MainUrlItem|bynodeguid|{0}";
            }

            public static class Relationships
            {
                public const string RelationshipsByNameAndLeftNodeId = Prefix + "Relationships|byname|{0}|byleftnodeid|{1}";
            }
        }
    }
}

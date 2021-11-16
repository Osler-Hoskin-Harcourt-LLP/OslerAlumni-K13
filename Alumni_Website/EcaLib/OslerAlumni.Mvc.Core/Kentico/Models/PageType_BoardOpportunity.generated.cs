//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at http://docs.kentico.com.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using OslerAlumni.Mvc.Core.Kentico.Models;

[assembly: RegisterDocumentType(PageType_BoardOpportunity.CLASS_NAME, typeof(PageType_BoardOpportunity))]

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    /// <summary>
    /// Represents a content item of type PageType_BoardOpportunity.
    /// </summary>
    public partial class PageType_BoardOpportunity : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "OslerAlumni.PageType_BoardOpportunity";


        /// <summary>
        /// The instance of the class that provides extended API for working with PageType_BoardOpportunity fields.
        /// </summary>
        private readonly PageType_BoardOpportunityFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// PageType_BoardOpportunityID.
        /// </summary>
        [DatabaseIDField]
        public int PageType_BoardOpportunityID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PageType_BoardOpportunityID"), 0);
            }
            set
            {
                SetValue("PageType_BoardOpportunityID", value);
            }
        }


        /// <summary>
        /// Name of the item in the content tree.
        /// </summary>
        [DatabaseField]
        public string PageName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PageName"), @"");
            }
            set
            {
                SetValue("PageName", value);
            }
        }


        /// <summary>
        /// Title.
        /// </summary>
        [DatabaseField]
        public string Title
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Title"), @"");
            }
            set
            {
                SetValue("Title", value);
            }
        }


        /// <summary>
        /// Short Description.
        /// </summary>
        [DatabaseField]
        public string ShortDescription
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ShortDescription"), @"");
            }
            set
            {
                SetValue("ShortDescription", value);
            }
        }


        /// <summary>
        /// Source.
        /// </summary>
        [DatabaseField]
        public string SourceCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SourceCodeName"), @"");
            }
            set
            {
                SetValue("SourceCodeName", value);
            }
        }


        /// <summary>
        /// Company.
        /// </summary>
        [DatabaseField]
        public string Company
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Company"), @"");
            }
            set
            {
                SetValue("Company", value);
            }
        }


        /// <summary>
        /// Board Opportunity Location.
        /// </summary>
        [DatabaseField]
        public string BoardOpportunityLocation
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BoardOpportunityLocation"), @"");
            }
            set
            {
                SetValue("BoardOpportunityLocation", value);
            }
        }


        /// <summary>
        /// Category (Industry).
        /// </summary>
        [DatabaseField]
        public string JobCategoryCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("JobCategoryCodeName"), @"");
            }
            set
            {
                SetValue("JobCategoryCodeName", value);
            }
        }


        /// <summary>
        /// Board Opportunity Type.
        /// </summary>
        [DatabaseField]
        public string BoardOpportunityTypeCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BoardOpportunityTypeCodeName"), @"");
            }
            set
            {
                SetValue("BoardOpportunityTypeCodeName", value);
            }
        }


        /// <summary>
        /// Posted Date.
        /// </summary>
        [DatabaseField]
        public DateTime PostedDate
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PostedDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PostedDate", value);
            }
        }


        /// <summary>
        /// External Url.
        /// </summary>
        [DatabaseField]
        public string ExternalUrl
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ExternalUrl"), @"");
            }
            set
            {
                SetValue("ExternalUrl", value);
            }
        }


        /// <summary>
        /// Description.
        /// </summary>
        [DatabaseField]
        public string Description
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Description"), @"");
            }
            set
            {
                SetValue("Description", value);
            }
        }


        /// <summary>
        /// When enabled, prevent site visitors from navigating to the page directly.
        /// If the page has any URLs associated with it, they will be removed.
        /// </summary>
        [DatabaseField]
        public bool DocumentMenuItemHideInNavigation1
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("DocumentMenuItemHideInNavigation"), false);
            }
            set
            {
                SetValue("DocumentMenuItemHideInNavigation", value);
            }
        }


        /// <summary>
        /// Name of the MVC Controller code class that should be responsible for rendering this page on the front-end.
        /// </summary>
        [DatabaseField]
        public string DefaultController
        {
            get
            {
                return ValidationHelper.GetString(GetValue("DefaultController"), @"");
            }
            set
            {
                SetValue("DefaultController", value);
            }
        }


        /// <summary>
        /// Name of the action method of the MVC Controller code class that should be responsible for rendering this page on the front-end.
        /// </summary>
        [DatabaseField]
        public string DefaultAction
        {
            get
            {
                return ValidationHelper.GetString(GetValue("DefaultAction"), @"");
            }
            set
            {
                SetValue("DefaultAction", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with PageType_BoardOpportunity fields.
        /// </summary>
        [RegisterProperty]
        public PageType_BoardOpportunityFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with PageType_BoardOpportunity fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class PageType_BoardOpportunityFields : AbstractHierarchicalObject<PageType_BoardOpportunityFields>
        {
            /// <summary>
            /// The content item of type PageType_BoardOpportunity that is a target of the extended API.
            /// </summary>
            private readonly PageType_BoardOpportunity mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="PageType_BoardOpportunityFields" /> class with the specified content item of type PageType_BoardOpportunity.
            /// </summary>
            /// <param name="instance">The content item of type PageType_BoardOpportunity that is a target of the extended API.</param>
            public PageType_BoardOpportunityFields(PageType_BoardOpportunity instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// PageType_BoardOpportunityID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.PageType_BoardOpportunityID;
                }
                set
                {
                    mInstance.PageType_BoardOpportunityID = value;
                }
            }


            /// <summary>
            /// Name of the item in the content tree.
            /// </summary>
            public string PageName
            {
                get
                {
                    return mInstance.PageName;
                }
                set
                {
                    mInstance.PageName = value;
                }
            }


            /// <summary>
            /// Title.
            /// </summary>
            public string Title
            {
                get
                {
                    return mInstance.Title;
                }
                set
                {
                    mInstance.Title = value;
                }
            }


            /// <summary>
            /// Short Description.
            /// </summary>
            public string ShortDescription
            {
                get
                {
                    return mInstance.ShortDescription;
                }
                set
                {
                    mInstance.ShortDescription = value;
                }
            }


            /// <summary>
            /// Source.
            /// </summary>
            public string SourceCodeName
            {
                get
                {
                    return mInstance.SourceCodeName;
                }
                set
                {
                    mInstance.SourceCodeName = value;
                }
            }


            /// <summary>
            /// Company.
            /// </summary>
            public string Company
            {
                get
                {
                    return mInstance.Company;
                }
                set
                {
                    mInstance.Company = value;
                }
            }


            /// <summary>
            /// Board Opportunity Location.
            /// </summary>
            public string BoardOpportunityLocation
            {
                get
                {
                    return mInstance.BoardOpportunityLocation;
                }
                set
                {
                    mInstance.BoardOpportunityLocation = value;
                }
            }


            /// <summary>
            /// Category (Industry).
            /// </summary>
            public string JobCategoryCodeName
            {
                get
                {
                    return mInstance.JobCategoryCodeName;
                }
                set
                {
                    mInstance.JobCategoryCodeName = value;
                }
            }


            /// <summary>
            /// Board Opportunity Type.
            /// </summary>
            public string BoardOpportunityTypeCodeName
            {
                get
                {
                    return mInstance.BoardOpportunityTypeCodeName;
                }
                set
                {
                    mInstance.BoardOpportunityTypeCodeName = value;
                }
            }


            /// <summary>
            /// Posted Date.
            /// </summary>
            public DateTime PostedDate
            {
                get
                {
                    return mInstance.PostedDate;
                }
                set
                {
                    mInstance.PostedDate = value;
                }
            }


            /// <summary>
            /// External Url.
            /// </summary>
            public string ExternalUrl
            {
                get
                {
                    return mInstance.ExternalUrl;
                }
                set
                {
                    mInstance.ExternalUrl = value;
                }
            }


            /// <summary>
            /// Description.
            /// </summary>
            public string Description
            {
                get
                {
                    return mInstance.Description;
                }
                set
                {
                    mInstance.Description = value;
                }
            }


            /// <summary>
            /// CTAs that are to appear in the top widget zone, if the template of the page supports it.
            /// </summary>
            public IEnumerable<TreeNode> TopWidgetZone
            {
                get
                {
                    return mInstance.GetRelatedDocuments("TopWidgetZone");
                }
            }


            /// <summary>
            /// Related Content Widget Zone.
            /// </summary>
            public IEnumerable<TreeNode> RelatedContentWidgetZone
            {
                get
                {
                    return mInstance.GetRelatedDocuments("RelatedContentWidgetZone");
                }
            }


            /// <summary>
            /// When enabled, prevent site visitors from navigating to the page directly.
            /// If the page has any URLs associated with it, they will be removed.
            /// </summary>
            public bool DocumentMenuItemHideInNavigation
            {
                get
                {
                    return mInstance.DocumentMenuItemHideInNavigation1;
                }
                set
                {
                    mInstance.DocumentMenuItemHideInNavigation1 = value;
                }
            }


            /// <summary>
            /// Name of the MVC Controller code class that should be responsible for rendering this page on the front-end.
            /// </summary>
            public string DefaultController
            {
                get
                {
                    return mInstance.DefaultController;
                }
                set
                {
                    mInstance.DefaultController = value;
                }
            }


            /// <summary>
            /// Name of the action method of the MVC Controller code class that should be responsible for rendering this page on the front-end.
            /// </summary>
            public string DefaultAction
            {
                get
                {
                    return mInstance.DefaultAction;
                }
                set
                {
                    mInstance.DefaultAction = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="PageType_BoardOpportunity" /> class.
        /// </summary>
        public PageType_BoardOpportunity() : base(CLASS_NAME)
        {
            mFields = new PageType_BoardOpportunityFields(this);
        }

        #endregion
    }
}

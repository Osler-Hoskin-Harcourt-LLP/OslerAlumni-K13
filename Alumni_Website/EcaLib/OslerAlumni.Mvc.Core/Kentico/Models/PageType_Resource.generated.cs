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

[assembly: RegisterDocumentType(PageType_Resource.CLASS_NAME, typeof(PageType_Resource))]

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    /// <summary>
    /// Represents a content item of type PageType_Resource.
    /// </summary>
    public partial class PageType_Resource : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "OslerAlumni.PageType_Resource";


        /// <summary>
        /// The instance of the class that provides extended API for working with PageType_Resource fields.
        /// </summary>
        private readonly PageType_ResourceFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// PageType_ResourceID.
        /// </summary>
        [DatabaseIDField]
        public int PageType_ResourceID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PageType_ResourceID"), 0);
            }
            set
            {
                SetValue("PageType_ResourceID", value);
            }
        }


        /// <summary>
        /// Hide from competitors.
        /// </summary>
        [DatabaseField]
        public bool HideFromCompetitors
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("HideFromCompetitors"), false);
            }
            set
            {
                SetValue("HideFromCompetitors", value);
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
        /// Resource Type(s).
        /// </summary>
        [DatabaseField]
        public string Types
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Types"), @"");
            }
            set
            {
                SetValue("Types", value);
            }
        }


        /// <summary>
        /// Authors.
        /// </summary>
        [DatabaseField]
        public string Authors
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Authors"), @"");
            }
            set
            {
                SetValue("Authors", value);
            }
        }


        /// <summary>
        /// Date Published.
        /// </summary>
        [DatabaseField]
        public DateTime DatePublished
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("DatePublished"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("DatePublished", value);
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
        /// External URL.
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
        /// Is File.
        /// </summary>
        [DatabaseField]
        public bool IsFile
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("IsFile"), false);
            }
            set
            {
                SetValue("IsFile", value);
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
        /// Gets an object that provides extended API for working with PageType_Resource fields.
        /// </summary>
        [RegisterProperty]
        public PageType_ResourceFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with PageType_Resource fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class PageType_ResourceFields : AbstractHierarchicalObject<PageType_ResourceFields>
        {
            /// <summary>
            /// The content item of type PageType_Resource that is a target of the extended API.
            /// </summary>
            private readonly PageType_Resource mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="PageType_ResourceFields" /> class with the specified content item of type PageType_Resource.
            /// </summary>
            /// <param name="instance">The content item of type PageType_Resource that is a target of the extended API.</param>
            public PageType_ResourceFields(PageType_Resource instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// PageType_ResourceID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.PageType_ResourceID;
                }
                set
                {
                    mInstance.PageType_ResourceID = value;
                }
            }


            /// <summary>
            /// Hide from competitors.
            /// </summary>
            public bool HideFromCompetitors
            {
                get
                {
                    return mInstance.HideFromCompetitors;
                }
                set
                {
                    mInstance.HideFromCompetitors = value;
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
            /// Resource Type(s).
            /// </summary>
            public string Types
            {
                get
                {
                    return mInstance.Types;
                }
                set
                {
                    mInstance.Types = value;
                }
            }


            /// <summary>
            /// Authors.
            /// </summary>
            public string Authors
            {
                get
                {
                    return mInstance.Authors;
                }
                set
                {
                    mInstance.Authors = value;
                }
            }


            /// <summary>
            /// Date Published.
            /// </summary>
            public DateTime DatePublished
            {
                get
                {
                    return mInstance.DatePublished;
                }
                set
                {
                    mInstance.DatePublished = value;
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
            /// External URL.
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
            /// Is File.
            /// </summary>
            public bool IsFile
            {
                get
                {
                    return mInstance.IsFile;
                }
                set
                {
                    mInstance.IsFile = value;
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
        /// Initializes a new instance of the <see cref="PageType_Resource" /> class.
        /// </summary>
        public PageType_Resource() : base(CLASS_NAME)
        {
            mFields = new PageType_ResourceFields(this);
        }

        #endregion
    }
}

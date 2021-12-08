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
using OslerAlumni.Core.Kentico.Models;

[assembly: RegisterDocumentType(PageType_BasePageType.CLASS_NAME, typeof(PageType_BasePageType))]

namespace OslerAlumni.Core.Kentico.Models
{

    /// <summary>
	/// Represents a content item of type PageType_BasePageType.
	/// </summary>
	public partial class PageType_BasePageType : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "OslerAlumni.PageType_BasePageType";


        /// <summary>
        /// The instance of the class that provides extended API for working with PageType_BasePageType fields.
        /// </summary>
        private readonly PageType_BasePageTypeFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// PageType_BasePageTypeID.
        /// </summary>
        [DatabaseIDField]
        public int PageType_BasePageTypeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PageType_BasePageTypeID"), 0);
            }
            set
            {
                SetValue("PageType_BasePageTypeID", value);
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
        /// Gets an object that provides extended API for working with PageType_BasePageType fields.
        /// </summary>
        [RegisterProperty]
        public PageType_BasePageTypeFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with PageType_BasePageType fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class PageType_BasePageTypeFields : AbstractHierarchicalObject<PageType_BasePageTypeFields>
        {
            /// <summary>
            /// The content item of type PageType_BasePageType that is a target of the extended API.
            /// </summary>
            private readonly PageType_BasePageType mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="PageType_BasePageTypeFields" /> class with the specified content item of type PageType_BasePageType.
            /// </summary>
            /// <param name="instance">The content item of type PageType_BasePageType that is a target of the extended API.</param>
            public PageType_BasePageTypeFields(PageType_BasePageType instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// PageType_BasePageTypeID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.PageType_BasePageTypeID;
                }
                set
                {
                    mInstance.PageType_BasePageTypeID = value;
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
        /// Initializes a new instance of the <see cref="PageType_BasePageType" /> class.
        /// </summary>
        public PageType_BasePageType() : base(CLASS_NAME)
        {
            mFields = new PageType_BasePageTypeFields(this);
        }

        #endregion
    }
}
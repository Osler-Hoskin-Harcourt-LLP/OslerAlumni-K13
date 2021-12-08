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


[assembly: RegisterDocumentType(PageType_News.CLASS_NAME, typeof(PageType_News))]

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    /// <summary>
	/// Represents a content item of type PageType_News.
	/// </summary>
	public partial class PageType_News : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "OslerAlumni.PageType_News";


        /// <summary>
        /// The instance of the class that provides extended API for working with PageType_News fields.
        /// </summary>
        private readonly PageType_NewsFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// PageType_NewsID.
        /// </summary>
        [DatabaseIDField]
        public int PageType_NewsID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PageType_NewsID"), 0);
            }
            set
            {
                SetValue("PageType_NewsID", value);
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
        /// Type.
        /// </summary>
        [DatabaseField]
        public string Type
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Type"), @"");
            }
            set
            {
                SetValue("Type", value);
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
        /// Image.
        /// </summary>
        [DatabaseField]
        public string Image
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Image"), @"");
            }
            set
            {
                SetValue("Image", value);
            }
        }


        /// <summary>
        /// Image Alt Text.
        /// </summary>
        [DatabaseField]
        public string ImageAltText
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ImageAltText"), @"");
            }
            set
            {
                SetValue("ImageAltText", value);
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
        /// Populates the placeholder within <em>"OslerAlumni.News.SpotlightVitalsHeader"</em> resource string.
        /// </summary>
        [DatabaseField]
        public string VitalsHeader
        {
            get
            {
                return ValidationHelper.GetString(GetValue("VitalsHeader"), @"");
            }
            set
            {
                SetValue("VitalsHeader", value);
            }
        }


        /// <summary>
        /// Vitals.
        /// </summary>
        [DatabaseField]
        public string Vitals
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Vitals"), @"");
            }
            set
            {
                SetValue("Vitals", value);
            }
        }


        /// <summary>
        /// Story Highlights.
        /// </summary>
        [DatabaseField]
        public string StoryHighlights
        {
            get
            {
                return ValidationHelper.GetString(GetValue("StoryHighlights"), @"");
            }
            set
            {
                SetValue("StoryHighlights", value);
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
        /// Gets an object that provides extended API for working with PageType_News fields.
        /// </summary>
        [RegisterProperty]
        public PageType_NewsFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with PageType_News fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class PageType_NewsFields : AbstractHierarchicalObject<PageType_NewsFields>
        {
            /// <summary>
            /// The content item of type PageType_News that is a target of the extended API.
            /// </summary>
            private readonly PageType_News mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="PageType_NewsFields" /> class with the specified content item of type PageType_News.
            /// </summary>
            /// <param name="instance">The content item of type PageType_News that is a target of the extended API.</param>
            public PageType_NewsFields(PageType_News instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// PageType_NewsID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.PageType_NewsID;
                }
                set
                {
                    mInstance.PageType_NewsID = value;
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
            /// Type.
            /// </summary>
            public string Type
            {
                get
                {
                    return mInstance.Type;
                }
                set
                {
                    mInstance.Type = value;
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
            /// Image.
            /// </summary>
            public string Image
            {
                get
                {
                    return mInstance.Image;
                }
                set
                {
                    mInstance.Image = value;
                }
            }


            /// <summary>
            /// Image Alt Text.
            /// </summary>
            public string ImageAltText
            {
                get
                {
                    return mInstance.ImageAltText;
                }
                set
                {
                    mInstance.ImageAltText = value;
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
            /// Populates the placeholder within <em>"OslerAlumni.News.SpotlightVitalsHeader"</em> resource string.
            /// </summary>
            public string VitalsHeader
            {
                get
                {
                    return mInstance.VitalsHeader;
                }
                set
                {
                    mInstance.VitalsHeader = value;
                }
            }


            /// <summary>
            /// Vitals.
            /// </summary>
            public string Vitals
            {
                get
                {
                    return mInstance.Vitals;
                }
                set
                {
                    mInstance.Vitals = value;
                }
            }


            /// <summary>
            /// Story Highlights.
            /// </summary>
            public string StoryHighlights
            {
                get
                {
                    return mInstance.StoryHighlights;
                }
                set
                {
                    mInstance.StoryHighlights = value;
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
            /// CTAs and other widgets that are to appear in the left widget zone, if the template of the page supports it.
            /// </summary>
            public IEnumerable<TreeNode> LeftWidgetZone
            {
                get
                {
                    return mInstance.GetRelatedDocuments("LeftWidgetZone");
                }
            }


            /// <summary>
            /// CTAs and other widgets that are to appear in the right widget zone, if the template of the page supports it.
            /// </summary>
            public IEnumerable<TreeNode> RightWidgetZone
            {
                get
                {
                    return mInstance.GetRelatedDocuments("RightWidgetZone");
                }
            }


            /// <summary>
            /// CTAs and other widgets that are to appear in the bottom widget zone, if the template of the page supports it.
            /// </summary>
            public IEnumerable<TreeNode> BottomWidgetZone
            {
                get
                {
                    return mInstance.GetRelatedDocuments("BottomWidgetZone");
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
        /// Initializes a new instance of the <see cref="PageType_News" /> class.
        /// </summary>
        public PageType_News() : base(CLASS_NAME)
        {
            mFields = new PageType_NewsFields(this);
        }

        #endregion
    }
}
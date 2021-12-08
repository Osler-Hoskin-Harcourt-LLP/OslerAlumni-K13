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

[assembly: RegisterDocumentType(PageType_CTA.CLASS_NAME, typeof(PageType_CTA))]

namespace OslerAlumni.Mvc.Core.Kentico.Models
{
    /// <summary>
    /// Represents a content item of type PageType_CTA.
    /// </summary>
    public partial class PageType_CTA : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "OslerAlumni.PageType_CTA";


        /// <summary>
        /// The instance of the class that provides extended API for working with PageType_CTA fields.
        /// </summary>
        private readonly PageType_CTAFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// PageType_CTAID.
        /// </summary>
        [DatabaseIDField]
        public int PageType_CTAID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PageType_CTAID"), 0);
            }
            set
            {
                SetValue("PageType_CTAID", value);
            }
        }


        /// <summary>
        /// Page Name.
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
        /// Linked Page.
        /// </summary>
        [DatabaseField]
        public Guid PageGUID
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PageGUID"), Guid.Empty);
            }
            set
            {
                SetValue("PageGUID", value);
            }
        }


        /// <summary>
        /// External URL.
        /// </summary>
        [DatabaseField]
        public string ExternalURL
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ExternalURL"), @"");
            }
            set
            {
                SetValue("ExternalURL", value);
            }
        }


        /// <summary>
        /// Link Text.
        /// </summary>
        [DatabaseField]
        public string LinkText
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LinkText"), @"");
            }
            set
            {
                SetValue("LinkText", value);
            }
        }


        /// <summary>
        /// Image (Desktop).
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
        /// Content.
        /// </summary>
        [DatabaseField]
        public string Content
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Content"), @"");
            }
            set
            {
                SetValue("Content", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with PageType_CTA fields.
        /// </summary>
        [RegisterProperty]
        public PageType_CTAFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with PageType_CTA fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class PageType_CTAFields : AbstractHierarchicalObject<PageType_CTAFields>
        {
            /// <summary>
            /// The content item of type PageType_CTA that is a target of the extended API.
            /// </summary>
            private readonly PageType_CTA mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="PageType_CTAFields" /> class with the specified content item of type PageType_CTA.
            /// </summary>
            /// <param name="instance">The content item of type PageType_CTA that is a target of the extended API.</param>
            public PageType_CTAFields(PageType_CTA instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// PageType_CTAID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.PageType_CTAID;
                }
                set
                {
                    mInstance.PageType_CTAID = value;
                }
            }


            /// <summary>
            /// Page Name.
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
            /// Linked Page.
            /// </summary>
            public Guid PageGUID
            {
                get
                {
                    return mInstance.PageGUID;
                }
                set
                {
                    mInstance.PageGUID = value;
                }
            }


            /// <summary>
            /// External URL.
            /// </summary>
            public string ExternalURL
            {
                get
                {
                    return mInstance.ExternalURL;
                }
                set
                {
                    mInstance.ExternalURL = value;
                }
            }


            /// <summary>
            /// Link Text.
            /// </summary>
            public string LinkText
            {
                get
                {
                    return mInstance.LinkText;
                }
                set
                {
                    mInstance.LinkText = value;
                }
            }


            /// <summary>
            /// Image (Desktop).
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
            /// Content.
            /// </summary>
            public string Content
            {
                get
                {
                    return mInstance.Content;
                }
                set
                {
                    mInstance.Content = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="PageType_CTA" /> class.
        /// </summary>
        public PageType_CTA() : base(CLASS_NAME)
        {
            mFields = new PageType_CTAFields(this);
        }

        #endregion
    }
}
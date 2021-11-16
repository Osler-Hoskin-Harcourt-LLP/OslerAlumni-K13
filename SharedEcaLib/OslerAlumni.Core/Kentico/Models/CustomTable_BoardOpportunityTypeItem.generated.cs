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
using CMS.CustomTables;
using OslerAlumni.Core.Kentico.Models;


[assembly: RegisterCustomTable(CustomTable_BoardOpportunityTypeItem.CLASS_NAME, typeof(CustomTable_BoardOpportunityTypeItem))]

namespace OslerAlumni.Core.Kentico.Models
{
    /// <summary>
    /// Represents a content item of type CustomTable_BoardOpportunityTypeItem.
    /// </summary>
    public partial class CustomTable_BoardOpportunityTypeItem : CustomTableItem
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "OslerAlumni.CustomTable_BoardOpportunityType";


        /// <summary>
        /// The instance of the class that provides extended API for working with CustomTable_BoardOpportunityTypeItem fields.
        /// </summary>
        private readonly CustomTable_BoardOpportunityTypeItemFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// Job Category.
        /// </summary>
        [DatabaseField]
        public string DisplayName
        {
            get { return ValidationHelper.GetString(GetValue("DisplayName"), @""); }
            set { SetValue("DisplayName", value); }
        }


        /// <summary>
        /// Code Name.
        /// </summary>
        [DatabaseField]
        public string CodeName
        {
            get { return ValidationHelper.GetString(GetValue("CodeName"), @""); }
            set { SetValue("CodeName", value); }
        }


        /// <summary>
        /// Enabled.
        /// </summary>
        [DatabaseField]
        public bool Enabled
        {
            get { return ValidationHelper.GetBoolean(GetValue("Enabled"), true); }
            set { SetValue("Enabled", value); }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with CustomTable_BoardOpportunityTypeItem fields.
        /// </summary>
        [RegisterProperty]
        public CustomTable_BoardOpportunityTypeItemFields Fields
        {
            get { return mFields; }
        }


        /// <summary>
        /// Provides extended API for working with CustomTable_BoardOpportunityTypeItem fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class
            CustomTable_BoardOpportunityTypeItemFields : AbstractHierarchicalObject<
                CustomTable_BoardOpportunityTypeItemFields>
        {
            /// <summary>
            /// The content item of type CustomTable_BoardOpportunityTypeItem that is a target of the extended API.
            /// </summary>
            private readonly CustomTable_BoardOpportunityTypeItem mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="CustomTable_BoardOpportunityTypeItemFields" /> class with the specified content item of type CustomTable_BoardOpportunityTypeItem.
            /// </summary>
            /// <param name="instance">The content item of type CustomTable_BoardOpportunityTypeItem that is a target of the extended API.</param>
            public CustomTable_BoardOpportunityTypeItemFields(CustomTable_BoardOpportunityTypeItem instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// Job Category.
            /// </summary>
            public string DisplayName
            {
                get { return mInstance.DisplayName; }
                set { mInstance.DisplayName = value; }
            }


            /// <summary>
            /// Code Name.
            /// </summary>
            public string CodeName
            {
                get { return mInstance.CodeName; }
                set { mInstance.CodeName = value; }
            }


            /// <summary>
            /// Enabled.
            /// </summary>
            public bool Enabled
            {
                get { return mInstance.Enabled; }
                set { mInstance.Enabled = value; }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTable_BoardOpportunityTypeItem" /> class.
        /// </summary>
        public CustomTable_BoardOpportunityTypeItem() : base(CLASS_NAME)
        {
            mFields = new CustomTable_BoardOpportunityTypeItemFields(this);
        }

        #endregion
    }
}

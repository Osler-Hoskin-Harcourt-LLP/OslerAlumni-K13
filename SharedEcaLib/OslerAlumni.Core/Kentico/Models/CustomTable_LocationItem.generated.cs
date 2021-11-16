//--------------------------------------------------------------------------------------------------
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


[assembly: RegisterCustomTable(LocationItem.CLASS_NAME, typeof(LocationItem))]

namespace OslerAlumni.Core.Kentico.Models
{
    /// <summary>
    /// Represents a content item of type LocationItem.
    /// </summary>
    public partial class LocationItem : CustomTableItem
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "Osler.Location";


        /// <summary>
        /// The instance of the class that provides extended API for working with LocationItem fields.
        /// </summary>
        private readonly LocationItemFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// Location.
        /// </summary>
        [DatabaseField]
        public string Location
        {
            get { return ValidationHelper.GetString(GetValue("Location"), @""); }
            set { SetValue("Location", value); }
        }


        /// <summary>
        /// Active.
        /// </summary>
        [DatabaseField]
        public bool Active
        {
            get { return ValidationHelper.GetBoolean(GetValue("Active"), true); }
            set { SetValue("Active", value); }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with LocationItem fields.
        /// </summary>
        [RegisterProperty]
        public LocationItemFields Fields
        {
            get { return mFields; }
        }


        /// <summary>
        /// Provides extended API for working with LocationItem fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class LocationItemFields : AbstractHierarchicalObject<LocationItemFields>
        {
            /// <summary>
            /// The content item of type LocationItem that is a target of the extended API.
            /// </summary>
            private readonly LocationItem mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="LocationItemFields" /> class with the specified content item of type LocationItem.
            /// </summary>
            /// <param name="instance">The content item of type LocationItem that is a target of the extended API.</param>
            public LocationItemFields(LocationItem instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// Location.
            /// </summary>
            public string Location
            {
                get { return mInstance.Location; }
                set { mInstance.Location = value; }
            }


            /// <summary>
            /// Active.
            /// </summary>
            public bool Active
            {
                get { return mInstance.Active; }
                set { mInstance.Active = value; }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationItem" /> class.
        /// </summary>
        public LocationItem() : base(CLASS_NAME)
        {
            mFields = new LocationItemFields(this);
        }

        #endregion
    }
}

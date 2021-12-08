using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Core.Extensions;
using Newtonsoft.Json;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Api.Attributes.Validation;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    public class FunctionAttendeeSearchRequest
    {

        #region "Properties"

        /// <summary>
        /// Standard culture code, in which to search for the pages, e.g. "fr-CA".
        /// NOTE: Search is case-sensitive and the culture code is
        /// expected to follow correct casing.
        /// </summary>
        [Required]
        [AllowedCulture(ErrorMessage = "Incorrect culture")]
        [JsonProperty("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// OnePlaceFunctionId: Id of the function in OnePlace
        /// </summary>
        [Required]
        [JsonProperty("onePlaceFunctionId")]
        public string OnePlaceFunctionId { get; set; }

        #endregion

        #region "Methods"



        #endregion
    }
}

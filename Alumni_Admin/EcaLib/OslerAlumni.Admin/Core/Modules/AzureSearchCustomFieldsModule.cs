using System;
using System.Linq;
using CMS;
using CMS.Search.Azure;
using ECA.Admin.Core.Modules;
using Microsoft.Azure.Search.Models;
using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;

[assembly: RegisterModule(typeof(AzureSearchCustomFieldsModule))]
namespace OslerAlumni.Admin.Core.Modules
{
    public class AzureSearchCustomFieldsModule : BaseModule
    {
        private static readonly string LastNameField = nameof(PageType_Profile.LastName).ToLower();
        private static readonly string LastNameNormalizedField = nameof(PageType_Profile.LastNameNormalized).ToLower();

        public AzureSearchCustomFieldsModule()
            : base($"{GlobalConstants.ModulePrefix}.{nameof(AzureSearchCustomFieldsModule)}")
        {

        }

        protected override void OnInit()
        {
            base.OnInit();

            //SearchServiceManager.CreatingOrUpdatingIndex.Execute += AddCustomFields;
            DocumentCreator.Instance.AddingDocumentValue.Execute += SetCustomFieldValues;
        }

        //private void AddCustomFields(object sender, CreateOrUpdateIndexEventArgs e)
        //{
        //    if (!e.Index.Fields
        //        .Any(field=>
        //            string.Equals( field.Name,
        //                LastNameNormalizedField,
        //                StringComparison.OrdinalIgnoreCase)))
        //    {
        //        e.Index.Fields.Add(new Field(LastNameNormalizedField, DataType.String));
        //    }
        //}

        private void SetCustomFieldValues(object sender, AddDocumentValueEventArgs e)
        {
            if (e.AzureName
                .Equals(
                LastNameNormalizedField,
                StringComparison.OrdinalIgnoreCase))
            {
                e.Value = e.Searchable.GetValue(LastNameField)?.ToString().ToLower();
            }
        }
    }
}

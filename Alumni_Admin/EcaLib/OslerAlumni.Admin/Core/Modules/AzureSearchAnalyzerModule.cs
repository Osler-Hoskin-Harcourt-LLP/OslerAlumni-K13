using System.Collections.Generic;
using System.Linq;
using CMS;
using CMS.Search.Azure;
using ECA.Admin.Core.Modules;
using Microsoft.Azure.Search.Models;
using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;

[assembly: RegisterModule(typeof(AzureSearchAnalyzerModule))]

namespace OslerAlumni.Admin.Core.Modules
{
    /// <summary>
    /// Adds custom analyzers to search fields to allow for more robust searching. E.g) partial searches.
    /// </summary>
    public class AzureSearchAnalyzerModule : BaseModule
    {
        #region "Constants"

        private const string CustomAnalyzerName = "customSearchAnalyzer";
        private const string SearchFieldFlag = "AzureSearchable";

        protected static readonly string[] PartialSearchExcludeFieldNames =
        {
            nameof(PageType_Profile.YearsAndJurisdictions),
            nameof(PageType_Profile.CurrentIndustry),
            nameof(PageType_Profile.PracticeAreas),
            nameof(PageType_Profile.OfficeLocations)
        };

        #endregion

        public AzureSearchAnalyzerModule()
            : base($"{GlobalConstants.ModulePrefix}.{nameof(AzureSearchAnalyzerModule)}")
        {

        }

        protected override void OnInit()
        {
            base.OnInit();

            DocumentFieldCreator.Instance.CreatingField.After += UseCustomSearchAnalyzer;
            SearchServiceManager.CreatingOrUpdatingIndex.Execute += AddCustomAnalyzer;
        }

        private void UseCustomSearchAnalyzer(object sender, CreateFieldEventArgs e)
        {
            if (e.SearchField.GetFlag(SearchFieldFlag) && !PartialSearchExcludeFieldNames.Contains(e.SearchField.FieldName))
            {
                e.Field.SearchAnalyzer = CustomAnalyzerName;
                e.Field.IndexAnalyzer = CustomAnalyzerName;
            }
        }

        private void AddCustomAnalyzer(object sender, CreateOrUpdateIndexEventArgs e)
        {
            var index = e.Index;

            if (index.Analyzers == null)
            {
                index.Analyzers = new List<Analyzer>();
            }

            if (index.TokenFilters == null)
            {
                index.TokenFilters = new List<TokenFilter>();
            }

            var analyzer = new CustomAnalyzer(CustomAnalyzerName, TokenizerName.Standard,
                new List<TokenFilterName>
                {
                    TokenFilterName.Lowercase,
                    TokenFilterName.AsciiFolding,
                    TokenFilterName.Snowball
                },
                new List<CharFilterName>
                {
                    CharFilterName.HtmlStrip
                });

            if (index.Analyzers.FirstOrDefault(a => a.Name == CustomAnalyzerName) == null)
                index.Analyzers.Add(analyzer);
        }
    }
}
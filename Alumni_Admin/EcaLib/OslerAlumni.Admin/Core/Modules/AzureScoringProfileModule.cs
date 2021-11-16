using System.Collections.Generic;
using CMS;
using CMS.Search.Azure;
using ECA.Admin.Core.Modules;
using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Services;


[assembly: RegisterModule(typeof(AzureScoringProfileModule))]

namespace OslerAlumni.Admin.Core.Modules
{
    /// <summary>
    /// This module adds a scoring profile to the Azure Search Index.
    /// </summary>
    public class AzureScoringProfileModule : BaseModule
    {
        #region Properties

        public IConfigurationService ConfigurationService { get; set; }

        public IAzureScoringProfileService AzureScoringProfileService { get; set; }


        #endregion

        public AzureScoringProfileModule()
            : base($"{GlobalConstants.ModulePrefix}.{nameof(AzureScoringProfileModule)}")
        {

        }

        protected override void OnInit()
        {
            base.OnInit();

            SearchServiceManager.CreatingOrUpdatingIndex.Execute += AddScoringProfile;
        }

        private void AddScoringProfile(object sender, CreateOrUpdateIndexEventArgs e)
        {

            var scoringProfile = ConfigurationService.GetWebConfigSection("AzureScoringProfileConfig");

            var scoringProfileValues = new Dictionary<string, double>();

            foreach (string key in scoringProfile)
            {
                scoringProfileValues.Add(key, double.Parse(scoringProfile[key]));
            }

            AzureScoringProfileService.AddScoringProfileToAzureIndex(e.Index,
                GlobalConstants.AzureSearch.ScoringProfile.BoostTitle,
                scoringProfileValues);
        }
    }
}

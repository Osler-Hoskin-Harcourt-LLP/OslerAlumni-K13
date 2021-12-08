using System;
using System.Collections.Generic;
using System.Linq;
using ECA.Core.Services;
using Microsoft.Azure.Search.Models;

namespace OslerAlumni.Core.Services
{
    public class AzureScoringProfileService
        : ServiceBase, IAzureScoringProfileService
    {

        #region "Methods"

        public void AddScoringProfileToAzureIndex(Microsoft.Azure.Search.Models.Index index, string profileName, Dictionary<string, double> scoringWeights, ScoringFunctionAggregation type = ScoringFunctionAggregation.Sum)
        {
            // Creates a dictionary containing the index's fields
            Dictionary<string, Field> indexFields = index.Fields.ToDictionary(f => f.Name, StringComparer.InvariantCultureIgnoreCase);


            // Initializes the index's list of scoring profiles (if it does not exist)
            if (index.ScoringProfiles == null)
            {
                index.ScoringProfiles = new List<ScoringProfile>();
            }

            // Prepares a scoring profile object for further configuration
            ScoringProfile scoringProfile;

            ScoringProfile existingProfile = index.ScoringProfiles.FirstOrDefault(sp => sp.Name == profileName);

            if (existingProfile != null)
            {
                scoringProfile = existingProfile;
            }
            else
            {
                scoringProfile = new ScoringProfile
                {
                    Name = profileName,
                    FunctionAggregation = type,
                    TextWeights = new TextWeights(new Dictionary<string, double>())
                };

                index.ScoringProfiles.Add(scoringProfile);
            }

            if (scoringWeights != null)
            {
                foreach (var keyValuePair in scoringWeights)
                {

                    if (indexFields.ContainsKey(keyValuePair.Key) && (!scoringProfile.TextWeights.Weights.ContainsKey(keyValuePair.Key) || scoringProfile.TextWeights.Weights[keyValuePair.Key] != keyValuePair.Value))
                    {
                        scoringProfile.TextWeights.Weights.Remove(keyValuePair.Key);
                        scoringProfile.TextWeights.Weights.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
        }


        #endregion
    }
}

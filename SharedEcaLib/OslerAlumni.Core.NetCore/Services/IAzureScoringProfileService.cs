using System.Collections.Generic;
using ECA.Core.Services;
using Microsoft.Azure.Search.Models;

namespace OslerAlumni.Core.Services
{
    public interface IAzureScoringProfileService: IService
    {
        void AddScoringProfileToAzureIndex(Index index, string profileName,
            Dictionary<string, double> scoringWeights,
            ScoringFunctionAggregation type = ScoringFunctionAggregation.Sum);
    }
}

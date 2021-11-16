using System;
using System.Threading.Tasks;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Models;
using Salesforce.Force;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceConnectionService
        : IService
    {
        IForceClient Connect(
            OnePlaceConfig onePlaceConfig);

        Task<IForceClient> ConnectAsync(
            OnePlaceConfig onePlaceConfig);

        Task<T> RetryOnTimeoutAsync<T>(
            IForceClient client,
            OnePlaceConfig onePlaceConfig,
            Func<IForceClient, Task<T>> executeFunc,
            bool propagateExceptions = false);
    }
}

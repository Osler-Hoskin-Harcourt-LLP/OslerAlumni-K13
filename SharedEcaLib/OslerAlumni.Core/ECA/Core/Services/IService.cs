using System;

namespace ECA.Core.Services
{
    /// <summary>
    /// The default interface that all services should implement,
    /// in order to be automatically handled by the DI framework.
    /// </summary>
    public interface IService
        : IDisposable
    {
    }
}

using System;
using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceFunctionService
        : IService
    {
        List<OnePlaceFunction> GetFunctions();

        bool TryGetOnePlaceFunctions(
            out IList<OnePlaceFunction> functions,
            out string errorMessage);
    }
}

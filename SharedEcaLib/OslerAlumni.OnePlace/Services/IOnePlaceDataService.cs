using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Delegates;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceDataService
        : IService
    {
        bool TryCreate(
            string objectName,
            object obj,
            out string objectId,
            out string errorMessage);

        bool TryDelete(
            string objectName,
            string objectId,
            out string errorMessage);

        bool TryGetByMainReference<T>(
            string id,
            ListFunctionDelegate<T> listFunc,
            IList<string> columnNames,
            out T result,
            out string errorMessage)
            where T : class, new();

        bool TryGetList<T>(
            IOnePlaceQuery query,
            out IList<T> resultList,
            out string errorMessage,
            IList<string> columnNames = null,
            IOnePlaceWhereCondition where = null,
            string orderBy = null,
            int? topN = null);

        bool TryUpdate(
            string objectName,
            string objectId,
            object obj,
            out string errorMessage);
    }
}

using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceRelationshipTypeService
        : IService
    {
        bool TryGetRelationshipType(
            string codeName,
            IList<string> columnNames,
            out RelationshipType relationshipType,
            out string errorMessage);

        bool TryGetRelationshipTypes(
            IList<string> codeNames,
            IList<string> columnNames,
            out IList<RelationshipType> relationshipTypes,
            out string errorMessage,
            int? topN = null);
    }
}

using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceRelationshipService
        : IService
    {
        bool TryGetRelationship(
            string fromContactId,
            string toAccountId,
            string relationshipType,
            IList<string> columnNames,
            out Relationship relationship,
            out string errorMessage);
    }
}

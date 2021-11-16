using System.Collections.Generic;
using System.Linq;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Services;
using Nito.AsyncEx;
using OslerAlumni.OnePlace.Kentico.Models;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class OnePlaceRelationshipService
        : ServiceBase, IOnePlaceRelationshipService
    {
        #region "Private fields"

        private readonly IOnePlaceDataService _onePlaceDataService;

        #endregion

        public OnePlaceRelationshipService(
            IOnePlaceDataService onePlaceDataService)
        {
            _onePlaceDataService = onePlaceDataService;
        }

        #region "Methods"

        /// <inheritdoc />
        public bool TryGetRelationship(
            string fromContactId,
            string toAccountId,
            string relationshipType,
            IList<string> columnNames,
            out Relationship relationship,
            out string errorMessage)
        {
            relationship = null;

            if (string.IsNullOrWhiteSpace(fromContactId))
            {
                errorMessage = "Cannot look up a relationship for an empty contact ID";

                return false;
            }
            
            if (string.IsNullOrWhiteSpace(toAccountId))
            {
                errorMessage = "Cannot look up a relationship for an empty account ID";

                return false;
            }

            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetRelationships,
                PageType_OnePlaceQueries.CLASS_NAME);

            var fromContactIdPropertyName =
                typeof(Relationship).GetPropertyName(
                    nameof(Relationship.FromContactId),
                    NameSource.Json);

            var toAccountIdPropertyName =
                typeof(Relationship).GetPropertyName(
                    nameof(Relationship.ToAccountId),
                    NameSource.Json);

            var relationshipTypePropertyName =
                typeof(Relationship).GetPropertyName(
                    nameof(Relationship.RelationshipType),
                    NameSource.Json);

            var relationshipTypeCodeNamePropertyName =
                typeof(RelationshipType).GetPropertyName(
                    nameof(RelationshipType.CodeName),
                    NameSource.Json);

            var where = new OnePlaceWhereCondition()
                .WhereEquals(
                    fromContactIdPropertyName,
                    fromContactId)
                .WhereEquals(
                    toAccountIdPropertyName,
                    toAccountId)
                .WhereEquals(
                    $"{relationshipTypePropertyName}.{relationshipTypeCodeNamePropertyName}",
                    relationshipType);

            IList<Relationship> relationships = null;
            string message = null;

            var isSuccess =
                AsyncContext.Run(() =>
                    _onePlaceDataService.TryGetList(
                        query,
                        out relationships,
                        out message,
                        columnNames,
                        where,
                        topN: 1));

            relationship = relationships?.FirstOrDefault();
            errorMessage = message;

            return isSuccess;
        }

        #endregion
    }
}

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
    public class OnePlaceRelationshipTypeService
        : ServiceBase, IOnePlaceRelationshipTypeService
    {
        #region "Private fields"

        private readonly IOnePlaceDataService _onePlaceDataService;

        #endregion

        public OnePlaceRelationshipTypeService(
            IOnePlaceDataService onePlaceDataService)
        {
            _onePlaceDataService = onePlaceDataService;
        }

        #region "Methods"

        /// <inheritdoc />
        public bool TryGetRelationshipType(
            string codeName,
            IList<string> columnNames,
            out RelationshipType relationshipType,
            out string errorMessage)
        {
            return _onePlaceDataService.TryGetByMainReference(
                codeName,
                TryGetRelationshipTypes,
                columnNames,
                out relationshipType,
                out errorMessage);
        }

        /// <inheritdoc />
        public bool TryGetRelationshipTypes(
            IList<string> codeNames,
            IList<string> columnNames,
            out IList<RelationshipType> relationshipTypes,
            out string errorMessage,
            int? topN = null)
        {
            relationshipTypes = null;

            if ((codeNames == null) || (codeNames.Count < 1))
            {
                errorMessage = "List of code names for relationship type lookup was empty";

                return false;
            }
            
            var query = new OnePlaceQuery(
                PageType_OnePlaceQueries.QueryNames.GetRelationshipTypes,
                PageType_OnePlaceQueries.CLASS_NAME);

            var codeNamePropertyName =
                typeof(RelationshipType).GetPropertyName(
                    nameof(RelationshipType.CodeName),
                    NameSource.Json);

            var where = new OnePlaceWhereCondition()
                .WhereIn(
                    codeNamePropertyName,
                    codeNames);

            IList<RelationshipType> relationshipTypeList = null;
            string message = null;

            var orderBy =
                $"{codeNamePropertyName} ASC";

            var isSuccess =
                AsyncContext.Run(() =>
                    _onePlaceDataService.TryGetList(
                        query,
                        out relationshipTypeList,
                        out message,
                        columnNames,
                        where,
                        orderBy,
                        topN));

            relationshipTypes = relationshipTypeList;
            errorMessage = message;

            return isSuccess;
        }

        #endregion
    }
}

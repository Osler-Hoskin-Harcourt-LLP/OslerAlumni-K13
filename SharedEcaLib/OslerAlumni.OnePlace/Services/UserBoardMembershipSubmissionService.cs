using System;
using System.Collections.Generic;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class UserBoardMembershipSubmissionService
        : BaseDataSubmissionService, IDataSubmissionService
    {
        #region "Private fields"

        private readonly IOnePlaceAccountService _onePlaceAccountService;
        private readonly IOnePlaceContactService _onePlaceContactService;
        private readonly IOnePlaceRelationshipService _onePlaceRelationshipService;
        private readonly IOnePlaceRelationshipTypeService _onePlaceRelationshipTypeService;

        #endregion

        public UserBoardMembershipSubmissionService(
            IOnePlaceAccountService onePlaceAccountService,
            IOnePlaceContactService onePlaceContactService,
            IOnePlaceDataService onePlaceDataService,
            IOnePlaceRelationshipService onePlaceRelationshipService,
            IOnePlaceRelationshipTypeService onePlaceRelationshipTypeService)
            : base(onePlaceDataService)
        {
            _onePlaceAccountService = onePlaceAccountService;
            _onePlaceContactService = onePlaceContactService;
            _onePlaceRelationshipService = onePlaceRelationshipService;
            _onePlaceRelationshipTypeService = onePlaceRelationshipTypeService;
        }

        #region "Methods"

        /// <inheritdoc />
        public override bool AppliesTo(
            Type contextType,
            Type type)
        {
            return contextType.IsAssignableFrom(typeof(IOslerUserInfo))
                   && type.IsAssignableFrom(typeof(BoardMembership));
        }

        #endregion

        #region "Helper methods"

        protected override ProcessingResult TryProcessUpsert(
            Type payloadType,
            string externalId,
            object payload)
        {
            var boardMembership = payload as BoardMembership;

            var result = new ProcessingResult
            {
                Success = false,
                Method = DataSubmissionMethod.None
            };

            if (boardMembership == null)
            {
                result.Message = $"Missing payload object of the type '{nameof(BoardMembership)}'";

                return result;
            }

            if (string.IsNullOrWhiteSpace(boardMembership.FromContactId))
            {
                result.Message =
                    $"Missing '{nameof(BoardMembership)}.{nameof(BoardMembership.FromContactId)}' field value.";

                return result;
            }

            if (string.IsNullOrWhiteSpace(boardMembership.ToAccountId))
            {
                result.Message =
                    $"Missing '{nameof(BoardMembership)}.{nameof(BoardMembership.ToAccountId)}' field value.";

                return result;
            }

            Relationship opRelationship;
            string message;

            var relationshipType = typeof(Relationship); 

            // Try to look up a board member relationship object by contact and account IDs
            if (!_onePlaceRelationshipService.TryGetRelationship(
                    boardMembership.FromContactId,
                    boardMembership.ToAccountId,
                    OnePlaceRelationshipType.BoardMember,
                    new List<string>
                    {
                        relationshipType
                            .GetPropertyName(
                                nameof(Relationship.Id),
                                NameSource.Json),
                        relationshipType
                            .GetPropertyName(
                                nameof(Relationship.FromContactId),
                                NameSource.Json),
                        relationshipType
                            .GetPropertyName(
                                nameof(Relationship.ToAccountId),
                                NameSource.Json)
                    }, 
                    out opRelationship,
                    out message))
            {
                result.Message = message;

                return result;
            }

            // If board member relationship doesn't exist, need to create it
            if (opRelationship == null)
            {
                // Check that the contact still exists in OnePlace
                Contact opContact;

                if (!_onePlaceContactService.TryGetContact(
                        boardMembership.FromContactId,
                        new List<string>
                        {
                            typeof(Contact)
                                .GetPropertyName(
                                    nameof(Contact.Id),
                                    NameSource.Json)
                        },
                        out opContact,
                        out message))
                {
                    result.Message = message;

                    return result;
                }

                if (opContact == null)
                {
                    result.Message = $"Contact '{boardMembership.FromContactId}' no longer exists in OnePlace";

                    return result;
                }

                // Check that the account still exists in OnePlace
                Account opAccount;
                
                if (!_onePlaceAccountService.TryGetAccount(
                        boardMembership.ToAccountId,
                        new List<string>
                        {
                            typeof(Account)
                                .GetPropertyName(
                                    nameof(Account.Id),
                                    NameSource.Json)
                        },
                        out opAccount,
                        out message))
                {
                    result.Message = message;

                    return result;
                }

                if (opAccount == null)
                {
                    result.Message = $"Account '{boardMembership.ToAccountId}' no longer exists in OnePlace";

                    return result;
                }

                // Get ID of the the Board Member relationship type from OnePlace
                RelationshipType opRelationshipType;

                if (!_onePlaceRelationshipTypeService.TryGetRelationshipType(
                    OnePlaceRelationshipType.BoardMember,
                    new List<string>
                    {
                        typeof(RelationshipType)
                            .GetPropertyName(
                                nameof(RelationshipType.Id),
                                NameSource.Json)
                    },
                    out opRelationshipType,
                    out message))
                {
                    result.Message = message;

                    return result;
                }

                if (opRelationshipType == null)
                {
                    result.Message = $"Relationship type '{OnePlaceRelationshipType.BoardMember}' no longer exists in OnePlace";

                    return result;
                }

                boardMembership.RelationshipTypeId = opRelationshipType.Id;

                result.Method = DataSubmissionMethod.Post; 
                
                // Need to clear ID value, so that OnePlace doesn't complain that we are trying to update it
                boardMembership.Id = null;
            }
            else
            {
                // If board member relationship does exist, we do NOT update it,
                // just use the existing object's ID
                result.Id = opRelationship.Id;
                result.Message = $"{nameof(Relationship)} object of type '{OnePlaceRelationshipType.BoardMember}' for contact '{boardMembership.FromContactId}' and account '{boardMembership.ToAccountId}' already exists";
            }

            result.Success = true;
            result.Payload = boardMembership;

            return result;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class UserContactSubmissionService
        : BaseDataSubmissionService, IDataSubmissionService
    {
        #region "Private fields"

        private readonly IUserRepository _userRepository;
        private readonly IOnePlaceAccountService _onePlaceAccountService;
        private readonly IOnePlaceContactService _onePlaceContactService;

        #endregion

        public UserContactSubmissionService(
            IUserRepository userRepository,
            IOnePlaceAccountService onePlaceAccountService,
            IOnePlaceDataService onePlaceDataService,
            IOnePlaceContactService onePlaceContactService)
            : base(onePlaceDataService)
        {
            _userRepository = userRepository;
            _onePlaceAccountService = onePlaceAccountService;
            _onePlaceContactService = onePlaceContactService;
        }

        #region "Methods"

        /// <inheritdoc />
        public override bool AppliesTo(
            Type contextType,
            Type type)
        {
            return contextType.IsAssignableFrom(typeof(IOslerUserInfo))
                   && type.IsAssignableFrom(typeof(Contact));
        }

        /// <inheritdoc />
        public override bool TryUpdateContextObjectExternalId(
            int contextObjectId,
            string externalId,
            out string message)
        {
            message = null;

            if (contextObjectId < 1)
            {
                message = "Missing user context object ID";

                return false;
            }

            var user = _userRepository.GetById(
                contextObjectId);

            if (user == null)
            {
                message = $"User matching ID '{contextObjectId}' doesn't exist";

                return false;
            }

            var currentExternalId = user.OnePlaceReference;

            // Don't overwrite the OnePlace reference if:
            // - the new value is the same as the current value;
            // - the new value is empty, while the current one is not;
            // - the new value is the placeholder macro, while the current one is not empty
            if (string.Equals(
                    currentExternalId,
                    externalId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            if (!string.IsNullOrWhiteSpace(currentExternalId)
                && string.IsNullOrWhiteSpace(externalId))
            {
                message = "Trying to overwrite an existing OnePlace reference with an empty value";

                return false;
            }

            user.OnePlaceReference = externalId;

            user.UpdateOnePlace = false;

            if (!_userRepository.Save(user))
            {
                message = "Failed to update the user object";

                return false;
            }

            return true;
        }


        #endregion

        #region "Helper methods"

        protected override ProcessingResult TryProcessUpdate(
            Type payloadType,
            string externalId,
            object payload)
        {
            var contact = payload as Contact;

            var result = new ProcessingResult
            {
                Success = false,
                Method = DataSubmissionMethod.None
            };

            if (contact == null)
            {
                result.Message = $"Missing payload object of the type '{nameof(Contact)}'";

                return result;
            }

            if (string.IsNullOrWhiteSpace(externalId))
            {
                result.Message =
                    $"Missing '{nameof(externalId)}' value for an update.";

                return result;
            }

            Contact opContact;
            string message;

            // Try to look up contact by its ID
            if (!_onePlaceContactService.TryGetContact(
                    externalId,
                    new List<string>
                    {
                        typeof(Contact).GetPropertyName(
                            nameof(Contact.Id),
                            NameSource.Json)
                    },
                    out opContact,
                    out message))
            {
                result.Message = message;

                return result;
            }

            // If contact wasn't found, check if it was merged into another contact
            if (opContact == null)
            {
                if (!_onePlaceContactService.TryGetContactMergeParent(
                        externalId,
                        new List<string>
                        {
                            typeof(Contact).GetPropertyName(
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
                    result.Message =
                        $"Contact matching the ID '{contact.Id}' no longer exists in OnePlace, and no other contact into which it might have been merged has been detected.";

                    return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(contact.AccountId))
            {
                // Check that the account still exists in OnePlace
                Account opAccount;

                if (!_onePlaceAccountService.TryGetAccount(
                    contact.AccountId,
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
                    result.Message = $"Account '{contact.AccountId}' no longer exists in OnePlace";

                    return result;
                }
            }

            // TODO: [VI] Do we need to update email preferences in OP?
            //MergeSubscriptionPreferences(opContact);

            result.Method = DataSubmissionMethod.Patch;
            result.Id = opContact.Id;

            // Need to clear ID value, so that OnePlace doesn't complain that we are trying to update it
            contact.Id = null;

            result.Success = true;
            result.Payload = contact;

            return result;
        }

        #endregion
    }
}

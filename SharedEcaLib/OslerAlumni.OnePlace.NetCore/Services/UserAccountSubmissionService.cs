using System;
using System.Collections.Generic;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.OnePlace.Definitions;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public class UserAccountSubmissionService
        : BaseDataSubmissionService, IDataSubmissionService
    {
        #region "Private fields"

        private readonly IOnePlaceAccountService _onePlaceAccountService;

        #endregion

        public UserAccountSubmissionService(
            IOnePlaceAccountService onePlaceAccountService,
            IOnePlaceDataService onePlaceDataService)
            : base(onePlaceDataService)
        {
            _onePlaceAccountService = onePlaceAccountService;
        }

        #region "Methods"

        /// <inheritdoc />
        public override bool AppliesTo(
            Type contextType,
            Type type)
        {
            return contextType.IsAssignableFrom(typeof(IOslerUserInfo))
                   && type.IsAssignableFrom(typeof(Account));
        }

        #endregion

        #region "Helper methods"

        protected override ProcessingResult TryProcessUpsert(
            Type payloadType,
            string externalId,
            object payload)
        {
            var account = payload as Account;

            var result = new ProcessingResult
            {
                Success = false,
                Method = DataSubmissionMethod.None
            };

            if (account == null)
            {
                result.Message = $"Missing payload object of the type '{nameof(Account)}'";

                return result;
            }

            if (string.IsNullOrWhiteSpace(account.Name))
            {
                result.Message =
                    $"Missing '{nameof(Account)}.{nameof(Account.Name)}' field value.";

                return result;
            }

            Account opAccount;
            string message;

            // Try to look up account by its name - note that we are ignoring external ID here
            if (!_onePlaceAccountService.TryGetAccountByName(
                    account.Name,
                    new List<string>
                    {
                        typeof(Account).GetPropertyName(
                            nameof(Account.Id),
                            NameSource.Json)
                    },
                    out opAccount,
                    out message))
            {
                result.Message = message;

                return result;
            }

            // If account doesn't exist, need to create it
            if (opAccount == null)
            {
                result.Method = DataSubmissionMethod.Post;
                
                // Need to clear ID value, so that OnePlace doesn't complain that we are trying to update it
                account.Id = null;
            }
            else
            {
                // If account does exist, we do NOT update it, just use the existing object's ID
                result.Id = opAccount.Id;
                result.Message = $"{nameof(Account)} object with the name '{account.Name}' already exists";
            }

            result.Success = true;
            result.Payload = account;

            return result;
        }

        #endregion
    }
}

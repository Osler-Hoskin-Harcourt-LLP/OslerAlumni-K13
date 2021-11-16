using System;
using System.Threading.Tasks;
using ECA.Admin.Tests;
using ECA.Core.Definitions;
using Moq;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Services;
using Salesforce.Common;
using Salesforce.Common.Models.Json;
using Salesforce.Force;
using Xunit;

namespace OslerAlumni.Admin.Tests.OnePlace.ServiceTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.OnePlace)]
    public class OnePlaceConnectionServiceTests
        : AdminTestsBase
    {
        #region "Mock components"

        protected class MockOPConnectionService
            : OnePlaceConnectionService, IOnePlaceConnectionService
        {
            public MockOPConnectionService()
                : base(null, null)
            {
            }

            public override Task<IForceClient> ConnectAsync(
                OnePlaceConfig onePlaceConfig)
            {
                return Task.FromResult(GetMockForceClient());
            }
        }

        protected static IForceClient GetMockForceClient()
        {
            return new Mock<IForceClient>().Object;
        }

        #endregion

        #region "Facts"

        [Fact]
        public async void RetriesQueryExecutionOnSessionTimeout()
        {
            var connectionService = new MockOPConnectionService();

            // Test for session timeout error code
            var executionCount = 0;

            await connectionService.RetryOnTimeoutAsync<object>(
                GetMockForceClient(),
                null,
                client =>
                {
                    executionCount++;

                    throw new ForceException(Error.InvalidSessionId, "Mock session timeout");
                });

            Assert.Equal(2, executionCount);

            // Test for some other error code
            executionCount = 0;

            await connectionService.RetryOnTimeoutAsync<object>(
                GetMockForceClient(),
                null,
                client =>
                {
                    executionCount++;

                    throw new ForceException(Error.AuthenticationFailure, "Mock authentication failure");
                });

            Assert.Equal(1, executionCount);

            // Test for a different exception
            executionCount = 0;

            await connectionService.RetryOnTimeoutAsync<object>(
                GetMockForceClient(),
                null,
                client =>
                {
                    executionCount++;

                    throw new Exception("Mock exception");
                });

            Assert.Equal(1, executionCount);
        }
        
        #endregion
    }
}

using ECA.Core.Definitions;
using ECA.Mvc.Tests;
using Microsoft.AspNet.Identity;
using Moq;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using OslerAlumni.Mvc.Core.Services;
using Xunit;

namespace OslerAlumni.Mvc.Tests.AttributeTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.Attributes)]

    public class PasswordPolicyEnforceTests
        : MvcTestsBase
    {
        private PasswordValidator GetPasswordValidator()
        {
            return new PasswordValidator()
            {
                RequiredLength = 10,
                RequireUppercase = true,
                RequireLowercase = true,
                RequireNonLetterOrDigit = true,
                RequireDigit = true
            };
        }

        private IPasswordPolicyService GetMockPasswordPolicyService(
            PasswordValidator passwordValidator)
        {
            var mock = new Mock<IPasswordPolicyService>();

            mock
                .Setup(obj => obj.PasswordValidator)
                .Returns(passwordValidator);

            return mock.Object;
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("incorrectPasword", false)]
        [InlineData("1234567890", false)]
        [InlineData("1catdogPuppy0", false)]
        [InlineData("1catdogPuppy5", false)]
        [InlineData("1c@tdogPuppy5", true)]
        [InlineData("1c#Tdogpuppy", true)]
        public void ShouldValidatePasswordCorrectly(string password, bool isValid)
        {
            //Arrange
            var attrib = 
                new PasswordPolicyEnforceAttribute(
                    GetMockPasswordPolicyService(
                        GetPasswordValidator()));

            //Act
            var result = attrib.IsValid(password);

            //Assert
            Assert.Equal(isValid, result);
        }
    }
}

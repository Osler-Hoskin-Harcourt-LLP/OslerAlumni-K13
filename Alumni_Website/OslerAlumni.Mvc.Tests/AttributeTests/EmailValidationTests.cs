using ECA.Core.Definitions;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using Xunit;

namespace OslerAlumni.Mvc.Tests.AttributeTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.Attributes)]
    public class EmailValidationTests
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("incorrectEmail", false)]
        [InlineData("abc.example.com", false)]
        [InlineData("a@b@c@example.com", false)]
        [InlineData("john.doe@example..com", false)]
        [InlineData("a\"b(c)d,e:f;g<h>i[j\\k]l@example.com", false)]
        [InlineData("simple@example.com", true)]
        [InlineData("veryVeryLongEmailwithManyCharcters123@veryverylongdomain.com", true)]
        [InlineData("very.common@example.com", true)]
        [InlineData("user.name+tag+sorting@example.com", true)]
        [InlineData("test@test.com", true)]
        [InlineData("test_user_772@hotmail.com", true)]
        [InlineData("sp'e.c%al!characters@hotmail.com", true)]
        [InlineData("specialcharacters@h'otmail.com", false)]
        public void ShouldValidateEmailCorrectly(string email, bool isValid)
        {
            //Arrange
            var attrib = new EmailValidationAttribute();

            //Act
            var result = attrib.IsValid(email);

            //Assert
            Assert.Equal(isValid, result);
        }
    }
}

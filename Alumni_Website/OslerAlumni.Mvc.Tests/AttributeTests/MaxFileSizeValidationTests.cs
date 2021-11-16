using System.Web;
using ECA.Core.Definitions;
using Moq;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using Xunit;

namespace OslerAlumni.Mvc.Tests.AttributeTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.Attributes)]
    public class MaxFileSizeValidationTests
    {
        [Theory]
        [InlineData(4, 0, true)]
        [InlineData(4, 3, true)]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, false)]
        [InlineData(5, 5, true)]
        [InlineData(0, 0, true)]
        [InlineData(0, 1, false)]
        public void ShouldValidateFileUploadSize(int allowableFileSizeMb, int fileSizeMb, bool isValid)
        {
            var mockFile = new Mock<HttpPostedFileBase>();
            mockFile.Setup(a => a.ContentLength).Returns(fileSizeMb * 1024 * 1024);

            //Arrange
            var attrib = new MaxFileSizeValidationAttribute(allowableFileSizeMb);

            //Act
            var result = attrib.IsValid(mockFile.Object);

            //Assert
            Assert.Equal(isValid, result);
        }
    }
}

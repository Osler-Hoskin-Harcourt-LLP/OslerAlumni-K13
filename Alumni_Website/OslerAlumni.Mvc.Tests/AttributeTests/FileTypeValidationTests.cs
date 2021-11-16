using System.Web;
using ECA.Core.Definitions;
using Moq;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using Xunit;

namespace OslerAlumni.Mvc.Tests.AttributeTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.Attributes)]
    public class FileTypeValidationTests
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData("", false)]
        [InlineData("test.doc", true)]
        [InlineData("test.pdf", true)]
        [InlineData("test.rtf", true)]
        [InlineData("test.png", false)]
        [InlineData("test.jpeg", false)]
        [InlineData("test.exe", false)]
        [InlineData("test.DOC", true)]
        [InlineData("test.DoC", true)]
        [InlineData("test.docx", true)]
        [InlineData("test .doc", true)]
        [InlineData(".doc", true)]
        [InlineData(".d oc", false)]
        [InlineData(".doc ", false)]
        [InlineData("test.png.exe.doc", true)]
        [InlineData("test.png.doc.exe", false)]
        [InlineData("noextension", false)]
        public void ShouldValidateFileUploadExentions(string fileName, bool isValid)
        {
            var mockFile = new Mock<HttpPostedFileBase>();
            mockFile.Setup(a => a.FileName).Returns(fileName);

            //Arrange
            var attrib = new FileTypeValidationAttribute(".doc,.docx,.pdf,.rtf");

            //Act
            var result = attrib.IsValid(fileName == null ? null : mockFile.Object);

            //Assert
            Assert.Equal(isValid, result);
        }
    }
}

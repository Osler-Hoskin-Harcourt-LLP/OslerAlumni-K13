using ECA.Mvc.Tests;
using Xunit;

namespace OslerAlumni.Mvc.Tests
{
    public class ControllerTests
        : MvcTestsBase
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, 2 + 2);
        }
    }
}

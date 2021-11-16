using System;
using ECA.Admin.Core.Macros;
using ECA.Admin.Tests;
using ECA.Core.Definitions;
using Xunit;

namespace ECA.Admin.Core.Tests.MacroTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.Macros)]
    public class ResourceStringMacroTests
        : AdminTestsBase
    {
        [Fact]
        public void ThrowsNotSupportedExceptionIfParametersAreNotProvider()
        {
            Assert.Throws<NotSupportedException>(
                () => ResourceStringMacros.GetLocalizedString(null));
        }
    }
}

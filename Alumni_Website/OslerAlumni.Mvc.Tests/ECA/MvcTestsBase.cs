using System;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Tests;
using ECA.Mvc.Tests.Helpers;
using Moq;

namespace ECA.Mvc.Tests
{
    public abstract class MvcTestsBase
        : TestsBase
    {
        #region "Properties"

        public override string CurrentCulture
        {
            get
            {
                return CultureHelper.GetCurrentCulture();
            }
            set
            {
                CultureHelper.SetCurrentCulture(value);
            }
        }

        #endregion

        protected MvcTestsBase()
            : this(false)
        { }

        protected MvcTestsBase(
            bool initializeKentico)
            : base(initializeKentico)
        { }

        #region "Shared mock components"

        protected ICacheService GetMockCacheService<T>()
        {
            var cacheService = new Mock<ICacheService>();

            cacheService
                .Setup(
                    cs => cs.Get(
                        It.IsAny<Func<CacheParameters, T>>(),
                        It.IsAny<CacheParameters>()))
                .Returns<Func<CacheParameters, T>, CacheParameters>(
                    GetMockCachedValue);

            return cacheService.Object;
        }

        protected T GetMockCachedValue<T>(
            Func<CacheParameters, T> func,
            CacheParameters parameters)
        {
            return func(parameters);
        }

        #endregion
    }
}

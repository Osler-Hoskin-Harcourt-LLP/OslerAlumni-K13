using CMS.Localization;
using ECA.Core.Tests;

namespace ECA.Admin.Tests
{
    public abstract class AdminTestsBase
        : TestsBase
    {
        #region "Properties"

        public override string CurrentCulture
        {
            get
            {
                return LocalizationContext.PreferredCultureCode;
            }
            set
            {
                LocalizationContext.PreferredCultureCode = value;
            }
        }

        #endregion

        protected AdminTestsBase()
            : this(false)
        { }

        protected AdminTestsBase(
            bool initializeKentico)
            : base(initializeKentico)
        { }
    }
}

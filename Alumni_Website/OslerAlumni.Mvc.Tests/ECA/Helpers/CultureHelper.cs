using System.Globalization;
using System.Threading;

namespace ECA.Mvc.Tests.Helpers
{
    public static class CultureHelper
    {
        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public static void SetCurrentCulture(string cultureName)
        {
            Thread.CurrentThread.CurrentUICulture =
                Thread.CurrentThread.CurrentCulture =
                    new CultureInfo(cultureName);
        }
    }
}

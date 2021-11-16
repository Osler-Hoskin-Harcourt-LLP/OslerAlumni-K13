using System;
using CMS;
using CMS.Helpers;
using CMS.MacroEngine;
using ECA.Admin.Core.Macros;

[assembly: RegisterExtension(typeof(ResourceStringMacros), typeof(string))]
[assembly: RegisterExtension(typeof(ResourceStringMacros), typeof(StringNamespace))]

namespace ECA.Admin.Core.Macros
{
    public class ResourceStringMacros 
        : MacroMethodContainer
    {
        [MacroMethod(typeof(string), "Returns a localized variant of a resource string from a specific culture.", 2)]
        [MacroMethodParam(0, "resourceStringCodename", typeof(string), "Resource String Codename.")]
        [MacroMethodParam(1, "culture", typeof(string), "Culture e.g. en-Us.")]
        public static object GetLocalizedString(
            EvaluationContext context, 
            params object[] parameters)
        {
            // Branches according to the number of the method's parameters
            switch (parameters.Length)
            {
                case 2:
                    // Overload with two parameters
                    return ResHelper.GetString(
                        ValidationHelper.GetString(parameters[0], string.Empty),
                        ValidationHelper.GetString(parameters[1], null));

                default:
                    // No other overloads are supported
                    throw new NotSupportedException();
            }
        }
    }
}

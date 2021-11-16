using System.Linq;
using System.Reflection;
using CMS.MacroEngine;

namespace ECA.Core.Extensions
{
    public static class MacroMethodExtensions
    {
        public static void LoadAttribute(
            this MacroMethod macroMethod,
            MacroMethodAttribute methodAttribute)
        {
            if ((macroMethod == null) || (methodAttribute == null))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(methodAttribute.Name))
            {
                macroMethod.Name = methodAttribute.Name;
            }

            macroMethod.Comment = methodAttribute.Comment;
            macroMethod.IsHidden = methodAttribute.IsHidden;
            macroMethod.MinimumParameters = methodAttribute.MinimumParameters;
            macroMethod.Snippet = methodAttribute.Snippet;
            macroMethod.Type = methodAttribute.Type;
            macroMethod.SpecialParameters = methodAttribute.SpecialParameters;
        }

        public static void LoadParameters(
            this MacroMethod macroMethod,
            MethodInfo method)
        {
            if ((macroMethod == null) || (method == null))
            {
                return;
            }

            var macroParameters = method
                .GetCustomAttributes(
                    typeof(MacroMethodParamAttribute),
                    true)
                .Cast<MacroMethodParamAttribute>()
                .OrderBy(p => p.Index);

            foreach (var macroParameter in macroParameters)
            {
                macroMethod.AddParameter(
                    macroParameter.GetMacroParam());
            }
        }
    }
}

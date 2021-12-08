using System;
using System.Reflection;
using CMS.MacroEngine;
using ECA.Core.Extensions;

namespace ECA.Core.Macros
{
    public abstract class MacroMethodsBase 
        : MacroMethodContainer
    {
        #region "Methods"

        protected override void RegisterMethods()
        {
            // Kentico doesn't read instance-specific macro methods,
            // which is why we are registering them here.
            // A lot of macro methods use instance-specific properties (e.g. services),
            // which is why they can't be declared static.
            var instanceMethods = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public);

            foreach (var method in instanceMethods)
            {
                var macroMethod = 
                    GetMacroMethod(method);

                if (macroMethod == null)
                {
                    continue;
                }

                // Parameter declarations for the method are always going to be the same
                macroMethod.LoadParameters(method);

                var methodAttributes =
                    method.GetCustomAttributes(
                        typeof(MacroMethodAttribute),
                        true);

                // The same method could have multiple macro declarations, i.e. names
                foreach (MacroMethodAttribute methodAttribute in methodAttributes)
                {
                    macroMethod.LoadAttribute(methodAttribute);

                    RegisterMethod(macroMethod);
                }
            }
        }

        #endregion

        #region "Helper methods"

        protected MacroMethod GetMacroMethod(
            MethodInfo method)
        {
            if (method == null)
            {
                return null;
            }

            var parameters = method.GetParameters();

            if ((parameters.Length < 1) || (parameters.Length > 2))
            {
                return null;
            }

            if (parameters.Length == 1)
            {
                if (parameters[0].ParameterType != typeof(object[]))
                {
                    return null;
                }

                var methodDelegate =
                    (Func<object[], object>)
                    Delegate.CreateDelegate(
                        typeof(Func<object[], object>),
                        this,
                        method,
                        false);

                if (methodDelegate == null)
                {
                    return null;
                }

                return new MacroMethod(
                    method.Name,
                    methodDelegate);
            }

            if (parameters[1].ParameterType != typeof(object[]))
            {
                return null;
            }

            if (parameters[0].ParameterType == typeof(EvaluationContext))
            {
                var methodDelegate =
                    (Func<EvaluationContext, object[], object>)
                    Delegate.CreateDelegate(
                        typeof(Func<EvaluationContext, object[], object>),
                        this,
                        method,
                        false);

                if (methodDelegate == null)
                {
                    return null;
                }

                return new MacroMethod(
                    method.Name,
                    methodDelegate);
            }

            if (parameters[0].ParameterType == typeof(MacroResolver))
            {
                var methodDelegate =
                    (Func<MacroResolver, object[], object>)
                    Delegate.CreateDelegate(
                        typeof(Func<MacroResolver, object[], object>),
                        this,
                        method,
                        false);

                if (methodDelegate == null)
                {
                    return null;
                }

                return new MacroMethod(
                    method.Name,
                    methodDelegate);
            }

            return null;
        }

        #endregion
    }
}

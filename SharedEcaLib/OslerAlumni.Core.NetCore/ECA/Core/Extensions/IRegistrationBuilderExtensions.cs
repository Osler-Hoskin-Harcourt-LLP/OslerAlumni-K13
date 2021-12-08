using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace ECA.Core.Extensions
{
    public static class IRegistrationBuilderExtensions
    {
        /// <summary>
        /// Specifies that a type is registered as providing all of its implemented interfaces.
        /// Customized version of the extension with the same name defined in <see cref="Autofac.RegistrationExtensions"/>.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <param name="registration">Registration to set service mapping on.</param>
        /// <param name="excludedInterfaces">List of implemented interfaces that should be excluded from the registration.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> AsImplementedInterfaces<TLimit>(
            this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration,
            params Type[] excludedInterfaces)
            where TLimit : class 
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            var excludedList = excludedInterfaces?.ToList() ?? new List<Type>();

            excludedList.Add(typeof(IDisposable));

            return registration.As(t =>
                t
                    .GetInterfaces()
                    .Where(i => !excludedList.Contains(i))
                    .ToArray());
        }
    }
}

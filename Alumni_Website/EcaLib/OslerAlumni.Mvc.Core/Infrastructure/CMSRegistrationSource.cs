using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Builder;
using Autofac.Core;

namespace OslerAlumni.Mvc.Core.Infrastructure
{
    /// <summary>
    /// Allows registrations to be made on-the-fly when unregistered services are requested.
    /// Provides all services from the Xperience API assemblies (included in the Kentico.Xperience.Libraries NuGet package).
    /// </summary>
    public class CMSRegistrationSource : IRegistrationSource
    {
        /// <summary>
        /// Indicates whether registrations provided by this source are 1:1 adapters on top of other components (like Meta, Func or Owned).
        /// </summary>
        public bool IsAdapterForIndividualComponents => false;

        /// <summary>
        /// Retrieves registrations for an unregistered service, to be used by the container.
        /// </summary>
        /// <param name="service">The service that was requested.</param>
        /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            // Checks whether the container already contains an existing registration for the requested service
            if (registrationAccessor(service).Any())
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            // Checks that the requested service carries valid type information
            var swt = service as IServiceWithType;
            if (swt == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            // Gets an instance of the requested service using the CMS.Core API
            // Returns null if the service instance cannot be resolved
            object instance = CMS.Core.Service.ResolveOptional(swt.ServiceType);

            if (instance == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            // Registers the service instance in the container
            return new[] {RegistrationBuilder.ForDelegate(swt.ServiceType, (c, p) => instance).CreateRegistration()};
        }
    }
}

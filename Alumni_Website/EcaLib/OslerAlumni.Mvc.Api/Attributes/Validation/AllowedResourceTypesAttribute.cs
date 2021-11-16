using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Api.Attributes.Validation
{
    public class AllowedResourceTypesAttribute : AllowedValuesAttribute
    {
        public AllowedResourceTypesAttribute() : base(null)
        {
        }

        public override IList<string> GetAllowableValues()
        {
            var diResolver = GlobalConfiguration.Configuration.DependencyResolver;

            //Since we are only looking at codenames, we can use the default culture.
            var resourceTypeItemRepository = (IResourceTypeItemRepository)diResolver.GetService(typeof(IResourceTypeItemRepository));
            var allowedValues = resourceTypeItemRepository
                                    .GetAllResourceTypeItems(GlobalConstants.Cultures.Default)?
                                    .Select(rt => rt.CodeName)?.ToList() ?? new List<string>();

            return allowedValues;
        }
    }
}

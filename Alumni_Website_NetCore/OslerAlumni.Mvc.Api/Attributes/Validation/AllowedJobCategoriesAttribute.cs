using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Api.Attributes.Validation
{
    public class AllowedJobCategoriesAttribute : AllowedValuesAttribute
    {
        public AllowedJobCategoriesAttribute() : base(null)
        {
            
        }

        public override IList<string> GetAllowableValues()
        {
           

            //Since we are only looking at codenames, we can use the default culture.
            var jobCategoryItemRepository = CMS.Core.Service.Resolve<IJobCategoryItemRepository>();
            var allowedValues = jobCategoryItemRepository
                                    .GetAllJobCategoryItems(GlobalConstants.Cultures.Default)?
                                    .Select(rt => rt.CodeName)?.ToList() ?? new List<string>();
            return allowedValues;
        }
    }
}

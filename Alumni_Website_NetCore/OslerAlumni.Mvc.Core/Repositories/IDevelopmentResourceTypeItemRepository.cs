using System.Collections.Generic;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface IDevelopmentResourceTypeItemRepository 
        : IRepository
    {
        List<CustomTable_DevelopmentResourceTypeItem> GetAllDevelopmentResourceTypeItems(
            string cultureName = null);

        CustomTable_DevelopmentResourceTypeItem GetByCodeName(
            string codeName,
            string cultureName = null);

        List<CustomTable_DevelopmentResourceTypeItem> GetByCodeNames(
            IEnumerable<string> codeNames,
            string cultureName = null
        );
    }
}

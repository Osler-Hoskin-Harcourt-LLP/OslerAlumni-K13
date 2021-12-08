using System.Collections.Generic;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface IResourceTypeItemRepository 
        : IRepository
    {
        List<CustomTable_ResourceTypeItem> GetAllResourceTypeItems(
            string cultureName = null);

        CustomTable_ResourceTypeItem GetByCodeName(
            string codeName,
            string cultureName = null);

        List<CustomTable_ResourceTypeItem> GetByCodeNames(
            IEnumerable<string> codeNames,
            string cultureName = null
        );
    }
}

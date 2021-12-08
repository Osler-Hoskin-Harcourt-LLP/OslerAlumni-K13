using System;
using System.Collections.Generic;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface IJobCategoryItemRepository 
        : IRepository
    {
        List<CustomTable_JobCategoryItem> GetAllJobCategoryItems(
            string cultureName = null);

        CustomTable_JobCategoryItem GetByGuid(
            Guid guid,
            string cultureName = null);

        CustomTable_JobCategoryItem GetByCodeName(
            string codeName,
            string cultureName = null);
    }
}

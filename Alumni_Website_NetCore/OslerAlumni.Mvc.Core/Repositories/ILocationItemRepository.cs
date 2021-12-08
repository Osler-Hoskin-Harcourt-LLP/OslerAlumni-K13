using System;
using System.Collections.Generic;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface ILocationItemRepository 
        : IRepository
    {
        List<LocationItem> GetAllLocationItems(
            string cultureName = null);

        LocationItem GetByGuid(
            Guid guid,
            string cultureName = null);
    }
}

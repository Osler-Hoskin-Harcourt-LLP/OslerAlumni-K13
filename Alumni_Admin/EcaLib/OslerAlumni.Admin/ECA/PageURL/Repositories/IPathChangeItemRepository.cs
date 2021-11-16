using System.Collections.Generic;
using ECA.Core.Repositories;
using ECA.PageURL.Kentico.Models;

namespace ECA.Admin.PageURL.Repositories
{
    public interface IPathChangeItemRepository
        : IRepository
    {
        bool Delete(
            IList<CustomTable_PathChangeItem> items);

        IList<CustomTable_PathChangeItem> GetPathChangeItems(
            int count,
            string siteName = null);

        void Save(
            CustomTable_PathChangeItem pathChangeItem);
    }
}

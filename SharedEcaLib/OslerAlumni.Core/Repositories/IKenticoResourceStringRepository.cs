using System.Collections.Generic;
using CMS.Localization;

using ECA.Core.Repositories;

namespace OslerAlumni.Core.Repositories
{
    public interface IKenticoResourceStringRepository
        : IRepository
    {
        ResourceStringInfo GetById(
            int id);

        ResourceStringInfo GetByName(
            string name);

        IEnumerable<ResourceStringInfo> GetByPrefix(
            string prefix);

        IEnumerable<ResourceStringInfo> GetByIds(
            List<int> resourceStringIds);
    }
}
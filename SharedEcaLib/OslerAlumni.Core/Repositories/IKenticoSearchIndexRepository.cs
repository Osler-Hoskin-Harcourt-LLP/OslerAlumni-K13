using System.Collections.Generic;
using CMS.Search;
using ECA.Core.Repositories;

namespace OslerAlumni.Core.Repositories
{
    public interface IKenticoSearchIndexRepository
        : IRepository
    {
        SearchIndexInfo GetByName(
            string name);

        IList<SearchIndexInfo> GetAzureSearchIndexes();
    }
}

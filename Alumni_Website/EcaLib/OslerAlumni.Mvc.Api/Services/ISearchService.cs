using CMS.Search;
using ECA.Core.Services;
using OslerAlumni.Mvc.Api.Models;

namespace OslerAlumni.Mvc.Api.Services
{
    public interface ISearchService
        : IService
    {
        SearchResponse<T> Search<T>(
            SearchRequest<T> searchRequest,
            out SearchIndexInfo searchIndex)
            where T : class, ISearchable, new();
    }
}

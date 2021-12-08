using ECA.Core.Services;
using OslerAlumni.Mvc.Api.Models;

namespace OslerAlumni.Mvc.Api.Services
{
    public interface ISearchService<in TRequest, TResult>
        : IService
        where TRequest : SearchRequest<TResult>, new()
        where TResult : class, ISearchable, new()
    {
        SearchResponse<TResult> Search(
            TRequest searchRequest);
    }
}

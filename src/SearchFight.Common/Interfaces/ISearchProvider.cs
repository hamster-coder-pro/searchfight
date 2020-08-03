using System.Threading.Tasks;
using SearchFight.Common.Models;

namespace SearchFight.Common.Interfaces
{
    public interface ISearchProvider<in TRequest, TResponse>
        where TRequest : ISearchRequestModel
        where TResponse : ISearchResultModel
    {
        Task<TResponse> SearchAsync(TRequest request);
    }
}
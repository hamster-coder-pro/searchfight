using System.Collections.Generic;
using System.Threading.Tasks;
using SearchFight.Common.Models;

namespace SearchFight.Common.Interfaces
{
    public interface ISearchReportBuilder<TSearchResponse, TReportModel>
        where TSearchResponse : ISearchResultModel
        where TReportModel : ISearchReportModel
    {
        Task<TReportModel> ExecuteAsync(ICollection<TSearchResponse> searchResult);
    }
}
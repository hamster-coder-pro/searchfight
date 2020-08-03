using System.Collections.Generic;
using SearchFight.Common.Models;

namespace SearchFight.Common.Interfaces
{
    public interface ISearchServiceFactory
    {
        ISearchReportBuilder<TSearchResult, TReport> CreateReportBuilder<TSearchResult, TReport>()
            where TSearchResult : ISearchResultModel
            where TReport : ISearchReportModel;

        ICollection<ISearchProvider<TSearchRequest, TSearchResult>> CreateSearchProvider<TSearchRequest, TSearchResult>()
            where TSearchRequest : ISearchRequestModel
            where TSearchResult : ISearchResultModel;

        ICollection<ISearchReportProvider<TReport>> CreateReportProvider<TReport>()
            where TReport : ISearchReportModel;
    }
}
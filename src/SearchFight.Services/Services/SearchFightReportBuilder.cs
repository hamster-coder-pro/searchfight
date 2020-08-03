using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SearchFight.Common.Interfaces;
using SearchFight.Services.Models;

namespace SearchFight.Services.Services
{
    internal class SearchFightReportBuilder : ISearchReportBuilder<SearchFightSearchResultModel, SearchFightReportModel>
    {
        public Task<SearchFightReportModel> ExecuteAsync(ICollection<SearchFightSearchResultModel> searchResult)
        {
            var result = new SearchFightReportModel();
            result.KeywordPerSearchEngineWinnerCollection = searchResult.GroupBy(x => x.SearchEngine, x => x)
                .Select(
                    group =>
                    {
                        var searchEngine = group.Key;
                        var keyword = group.OrderBy(x => x.ResultCount).LastOrDefault()?.Request.Keyword ?? "-";
                        return new SearchFightReportModel.WinnerPerSearchEngineModel(searchEngine, keyword);
                    }
                )
                .ToArray();

            result.KeywordCollection = searchResult.GroupBy(x => x.Request.Keyword, x => x)
                .Select(
                    group => new SearchFightReportModel.SearchResultsPerKeywordModel(
                        group.Key
                        , group.Select(
                            x => new SearchFightReportModel.SearchResultsPerKeywordModel.SearchResultModel()
                            {
                                SearchEngine = x.SearchEngine
                                , ResultCount = x.IsSucceed ? x.ResultCount : -1
                            }
                        )
                    )
                )
                .ToArray();

            result.TotalWinnerKeyword = searchResult.OrderBy(x => x.ResultCount).LastOrDefault().Request.Keyword;

            return Task.FromResult(result);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using SearchFight.Common.Models;

namespace SearchFight.Services.Models
{
    internal class SearchFightReportModel : ISearchReportModel
    {
        public SearchFightReportModel()
        {
            KeywordCollection = new List<SearchResultsPerKeywordModel>();
            KeywordPerSearchEngineWinnerCollection = new List<WinnerPerSearchEngineModel>();
            TotalWinnerKeyword = "";
        }

        public ICollection<SearchResultsPerKeywordModel> KeywordCollection { get; set; }

        public ICollection<WinnerPerSearchEngineModel> KeywordPerSearchEngineWinnerCollection { get; set; }

        public string TotalWinnerKeyword { get; set; }

        public class SearchResultsPerKeywordModel
        {
            public SearchResultsPerKeywordModel(string keyword, IEnumerable<SearchResultModel> results)
            {
                Keyword = keyword;
                Results = results.ToList().AsReadOnly();
            }

            public string Keyword { get; }

            public IReadOnlyCollection<SearchResultModel> Results { get; }

            public class SearchResultModel
            {
                public string? SearchEngine { get; set; }
                public int ResultCount { get; set; }
            }
        }

        public class WinnerPerSearchEngineModel
        {
            public WinnerPerSearchEngineModel(string searchEngine, string keyword)
            {
                SearchEngine = searchEngine;
                Keyword = keyword;
            }

            public string SearchEngine { get; }

            public string Keyword { get; }
        }
    }
}
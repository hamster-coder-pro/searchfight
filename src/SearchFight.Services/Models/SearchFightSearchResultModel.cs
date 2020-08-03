using SearchFight.Common.Models;

namespace SearchFight.Services.Models
{
    internal class SearchFightSearchResultModel : SearchResultModelBase<SearchFightSearchRequestModel>
    {
        public SearchFightSearchResultModel(SearchFightSearchRequestModel request, string searchEngine)
            : base(request, searchEngine)
        {
        }

        public int ResultCount { get; set; }
    }
}
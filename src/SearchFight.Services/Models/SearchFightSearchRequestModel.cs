using SearchFight.Common.Models;

namespace SearchFight.Services.Models
{
    internal class SearchFightSearchRequestModel : ISearchRequestModel
    {
        public SearchFightSearchRequestModel(string keyword)
        {
            Keyword = keyword;
        }

        public string Keyword { get; }
    }
}
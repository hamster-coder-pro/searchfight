namespace SearchFight.Common.Models
{
    public abstract class SearchResultModelBase<TRequest> : ISearchResultModel
        where TRequest : ISearchRequestModel
    {
        protected SearchResultModelBase(TRequest request, string searchEngine)
        {
            Request = request;
            SearchEngine = searchEngine;
            IsSucceed = true;
            Error = null;
        }

        public TRequest Request { get; }

        public string SearchEngine { get; }

        public bool IsSucceed { get; set; }

        public string? Error { get; set; }
    }
}
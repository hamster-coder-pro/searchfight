using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using SearchFight.Services.Interfaces;
using SearchFight.Services.Models;

namespace SearchFight.Services.Services
{
    internal class BingSearchProvider : RawHtmlSearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel, BingSearchProvider>
    {
        public BingSearchProvider(ILogger<BingSearchProvider> logger, IHttpClientFactory httpClientFactory, IHtmlParser htmlParser)
            : base(logger, httpClientFactory, htmlParser)
        {
        }

        private const string SearchEngineName = "MSN";

        protected override string HttpClientName { get; } = KnownHttpClients.Bing;

        protected override string CreateUrl(SearchFightSearchRequestModel request)
        {
            var encodedKeyword = HttpUtility.UrlEncode($"\"{request.Keyword}\"");
            var result = $"https://www.bing.com/search?q={encodedKeyword}";
            return result;
        }

        protected override Task ParseHtmlAsync(string html, SearchFightSearchResultModel result)
        {
            var startIndex = 0;
            var endIndex = 0;

            startIndex = HtmlParser.AfterIndexOfTag(html, "<span class=\"sb_count\"", startIndex);
            startIndex = HtmlParser.AfterIndexOfTag(html, ">", startIndex);

            endIndex = HtmlParser.IndexOfTag(html, "<", startIndex);

            var text = html[startIndex..endIndex];
            result.ResultCount = HtmlParser.ExtractNumber(text);

            return Task.CompletedTask;
        }

        protected override SearchFightSearchResultModel CreateResult(SearchFightSearchRequestModel request)
        {
            return new SearchFightSearchResultModel(request, SearchEngineName);
        }
    }
}
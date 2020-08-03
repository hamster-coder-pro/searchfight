using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using SearchFight.Services.Interfaces;
using SearchFight.Services.Models;

namespace SearchFight.Services.Services
{
    internal class YahooSearchProvider : RawHtmlSearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel, YahooSearchProvider>
    {
        public YahooSearchProvider(ILogger<YahooSearchProvider> logger, IHttpClientFactory httpClientFactory, IHtmlParser htmlParser)
            : base(logger, httpClientFactory, htmlParser)
        {
        }

        private const string SearchEngineName = "Yahoo";

        protected override string HttpClientName { get; } = KnownHttpClients.Yahoo;

        protected override string CreateUrl(SearchFightSearchRequestModel request)
        {
            var encodedKeyword = HttpUtility.UrlEncode($"\"{request.Keyword}\"");
            var result = $"https://search.yahoo.com/search?p={encodedKeyword}";
            return result;
        }

        protected override Task ParseHtmlAsync(string html, SearchFightSearchResultModel result)
        {
            var startIndex = 0;
            var endIndex = 0;

            startIndex = HtmlParser.AfterIndexOfTag(html, "<div class=\"compPagination\"", startIndex);
            startIndex = HtmlParser.AfterIndexOfTag(html, "<span", startIndex);
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
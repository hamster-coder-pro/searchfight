using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using SearchFight.Services.Interfaces;
using SearchFight.Services.Models;

namespace SearchFight.Services.Services
{
    internal class GoogleSearchProvider : RawHtmlSearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel, GoogleSearchProvider>
    {
        public GoogleSearchProvider(ILogger<GoogleSearchProvider> logger, IHttpClientFactory httpClientFactory, IHtmlParser htmlParser)
            : base(logger, httpClientFactory, htmlParser)
        {
        }

        protected override string CreateUrl(SearchFightSearchRequestModel request)
        {
            var encodedKeyword = HttpUtility.UrlEncode($"\"{request.Keyword}\"");
            var result = $"https://www.google.com/search?q={encodedKeyword}";
            return result;
        }

        protected override SearchFightSearchResultModel CreateResult(SearchFightSearchRequestModel request)
        {
            return new SearchFightSearchResultModel(request, "Google");
        }

        protected override string HttpClientName { get; } = "google";

        protected override Task ParseHtmlAsync(string html, SearchFightSearchResultModel result)
        {
            var startIndex = 0;
            var endIndex = 0;

            startIndex = HtmlParser.AfterIndexOfTag(html, "<div id=\"result-stats\">", startIndex);
            endIndex = HtmlParser.IndexOfTag(html, "<nobr>", startIndex);

            var text = html[startIndex..endIndex];

            result.ResultCount = HtmlParser.ExtractNumber(text);
            return Task.CompletedTask;
        }
    }
}
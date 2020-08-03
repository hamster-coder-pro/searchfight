using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchFight.Common.Interfaces;
using SearchFight.Common.Models;
using SearchFight.Services.Exceptions;
using SearchFight.Services.Interfaces;

namespace SearchFight.Services.Services
{
    internal abstract class RawHtmlSearchProvider<TRequest, TResult, TInstance> : ISearchProvider<TRequest, TResult>
        where TRequest : ISearchRequestModel
        where TResult : ISearchResultModel
        where TInstance : RawHtmlSearchProvider<TRequest, TResult, TInstance>
    {
        protected ILogger<TInstance> Logger { get; }
        protected IHtmlParser HtmlParser { get; }

        protected HttpClient HttpClient
        {
            get
            {
                return LazyClient.Value;
            }
        }

        private Lazy<HttpClient> LazyClient { get; }

        protected abstract string HttpClientName { get; }

        protected RawHtmlSearchProvider(ILogger<TInstance> logger, IHttpClientFactory httpClientFactory, IHtmlParser htmlParser)
        {
            Logger = logger;
            HtmlParser = htmlParser;
            LazyClient = new Lazy<HttpClient>(() => httpClientFactory.CreateClient(HttpClientName));
        }

        protected abstract string CreateUrl(TRequest request);

        protected virtual async Task<string> DownloadHtmlAsync(string url)
        {
            var response = await HttpClient.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new DataSearcherException($"Error data requesting: Status code: \"{response.StatusCode}\", Reason phrase: \"{response.ReasonPhrase}\"");
            }

            var body = await response.Content.ReadAsStringAsync();
            return body;
        }

        protected abstract Task ParseHtmlAsync(string html, TResult result);

        protected abstract TResult CreateResult(TRequest request);

        public async Task<TResult> SearchAsync(TRequest request)
        {
            var result = CreateResult(request);

            try
            {
                var url = CreateUrl(request);
                var body = await DownloadHtmlAsync(url);
                await ParseHtmlAsync(body, result);
                return result;
            }
            catch (DataSearcherException exception)
            {
                result.IsSucceed = false;
                result.Error = exception.ToString();
                Logger.Log(LogLevel.Error, exception, "");
            }
            catch (Exception exception)
            {
                var exceptionWrapper = new DataSearcherException("Unexpected exception occured", exception);
                result.IsSucceed = false;
                result.Error = exceptionWrapper.ToString();
                Logger.Log(LogLevel.Error, exceptionWrapper, "");
            }

            return result;
        }
    }
}
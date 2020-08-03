using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SearchFight.Services;
using SearchFight.Services.Exceptions;
using SearchFight.Services.Interfaces;
using SearchFight.Services.Models;
using SearchFight.Services.Services;

namespace SearchFight.Tests
{
    public class YahooSearchProviderTests
    {
        private static readonly string SearchEngine = "Yahoo";

        [Test]
        public async Task SearchAsyncSucceed()
        {
            var loggerMock = new Mock<ILogger<YahooSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();
            var expectedResult = 123;

            htmlParser.Setup(x => x.ExtractNumber(It.IsAny<string>())).Returns(expectedResult).Verifiable();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Yahoo);
            
            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new YahooSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
            var result = await searchProvider.SearchAsync(request);

            htmlParser.VerifyAll();
            httpHandlerMock.VerifyAll();
            result.SearchEngine.Should().Be(SearchEngine);
            result.Should().NotBeNull();
            result.Request.Should().Be(request);
            result.ResultCount.Should().Be(expectedResult);
            result.IsSucceed.Should().BeTrue();
            result.Error.Should().BeNull();
        }

        [Test]
        public async Task SearchAsyncFailed()
        {
            var loggerMock = new Mock<ILogger<YahooSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();

            htmlParser.Setup(x => x.ExtractNumber(It.IsAny<string>())).Throws<DataSearcherException>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Yahoo);
            
            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new YahooSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
            var result = await searchProvider.SearchAsync(request);

            httpClientFactoryMock.VerifyAll();
            htmlParser.VerifyAll();
            httpHandlerMock.VerifyAll();

            result.Should().NotBeNull();
            result.SearchEngine.Should().Be(SearchEngine);
            result.ResultCount.Should().Be(default);
            result.Request.Should().Be(request);
            result.IsSucceed.Should().BeFalse();
            result.Error.Should().Contain($"{nameof(DataSearcherException)}");
        }

        [Test]
        public async Task SearchAsyncFailedUnxepected()
        {
            var loggerMock = new Mock<ILogger<YahooSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();
            var expectedResult = 123;

            htmlParser.Setup(x => x.ExtractNumber(It.IsAny<string>())).Throws<Exception>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Yahoo);
            
            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new YahooSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
            var result = await searchProvider.SearchAsync(request);

            httpClientFactoryMock.VerifyAll();
            htmlParser.VerifyAll();
            httpHandlerMock.VerifyAll();

            result.Should().NotBeNull();
            result.SearchEngine.Should().Be(SearchEngine);
            result.ResultCount.Should().Be(default);
            result.Request.Should().Be(request);
            result.IsSucceed.Should().BeFalse();
            result.Error.Should().Contain("Unexpected");
            result.Error.Should().Contain($"{nameof(DataSearcherException)}");
        }

        [Test]
        public async Task SearchAsyncBadRequest()
        {
            var loggerMock = new Mock<ILogger<YahooSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Yahoo, statusCode: HttpStatusCode.BadRequest);

            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new YahooSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
            var result = await searchProvider.SearchAsync(request);

            httpClientFactoryMock.VerifyAll();
            htmlParser.VerifyAll();
            httpHandlerMock.VerifyAll();

            result.Should().NotBeNull();
            result.SearchEngine.Should().Be(SearchEngine);
            result.ResultCount.Should().Be(default);
            result.Request.Should().Be(request);
            result.IsSucceed.Should().BeFalse();
            result.Error.Should().Contain($"{nameof(DataSearcherException)}");
        }

        [Test]
        public async Task SearchAsyncExpectedParseLogic()
        {
            var loggerMock = new Mock<ILogger<YahooSearchProvider>>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var element = $"<div class=\"compPagination\" ><span id=\"yui_3_10_0_1_1596410179970_452\">247,000,000 results</span></div>";
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Yahoo, stringContent: element);

            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new YahooSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, new HtmlParser());
            var result = await searchProvider.SearchAsync(request);

            httpClientFactoryMock.VerifyAll();
            httpHandlerMock.VerifyAll();

            result.Should().NotBeNull();
            result.SearchEngine.Should().Be(SearchEngine);
            result.ResultCount.Should().Be(247000000);
            result.Request.Should().Be(request);
            result.IsSucceed.Should().BeTrue();
            result.Error.Should().BeNull();
        }
    }
}
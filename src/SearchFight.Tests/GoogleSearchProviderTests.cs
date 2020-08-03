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
    public class GoogleSearchProviderTests
    {
        private static readonly string SearchEngine = "Google";

        [Test]
        public async Task SearchAsyncSucceed()
        {
            var loggerMock = new Mock<ILogger<GoogleSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();
            var expectedResult = 123;

            htmlParser.Setup(x => x.ExtractNumber(It.IsAny<string>())).Returns(expectedResult).Verifiable();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Google);
            
            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new GoogleSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
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
            var loggerMock = new Mock<ILogger<GoogleSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();

            htmlParser.Setup(x => x.ExtractNumber(It.IsAny<string>())).Throws<DataSearcherException>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Google);
            
            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new GoogleSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
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
            var loggerMock = new Mock<ILogger<GoogleSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();
            var expectedResult = 123;

            htmlParser.Setup(x => x.ExtractNumber(It.IsAny<string>())).Throws<Exception>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Google);
            
            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new GoogleSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
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
            var loggerMock = new Mock<ILogger<GoogleSearchProvider>>();
            var htmlParser = new Mock<IHtmlParser>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Google, statusCode: HttpStatusCode.BadRequest);

            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new GoogleSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, htmlParser.Object);
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
            var loggerMock = new Mock<ILogger<GoogleSearchProvider>>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpHandlerMock = httpClientFactoryMock.SetupHttpClientResponse(KnownHttpClients.Google, stringContent: "<div id=\"result-stats\">asdfkjh 12,.33-12%%%3+43 kuasdifu<nobr>");

            var request = new SearchFightSearchRequestModel("test");

            var searchProvider = new GoogleSearchProvider(loggerMock.Object, httpClientFactoryMock.Object, new HtmlParser());
            var result = await searchProvider.SearchAsync(request);

            httpClientFactoryMock.VerifyAll();
            httpHandlerMock.VerifyAll();

            result.Should().NotBeNull();
            result.SearchEngine.Should().Be(SearchEngine);
            result.ResultCount.Should().Be(123312343);
            result.Request.Should().Be(request);
            result.IsSucceed.Should().BeTrue();
            result.Error.Should().BeNull();
        }
    }
}
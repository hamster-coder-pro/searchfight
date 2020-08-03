using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Search.Common;
using SearchFight.Console.Application;
using SearchFight.Services.Models;

namespace SearchFight.Tests
{
    public class ApplicationTests
    {
        [Test]
        public void ConstructorPositive()
        {
            var loggerMock = new Mock<ILogger<Application>>();
            var searchStrategyMock = new Mock<ISearchStrategy<SearchFightSearchParametersModel>>();
            var testee = new Application(loggerMock.Object, searchStrategyMock.Object);
        }

        [Test]
        public async Task ExecuteAsyncPositive()
        {
            var loggerMock = new Mock<ILogger<Application>>();
            var searchStrategyMock = new Mock<ISearchStrategy<SearchFightSearchParametersModel>>();
            var testee = new Application(loggerMock.Object, searchStrategyMock.Object);
            var result = await testee.ExecuteAsync(new [] { "0", "1" });
            result.Should().Be(0);
            searchStrategyMock.VerifyAll();
        }

        [Test]
        public async Task ExecuteAsyncPositive2()
        {
            var loggerMock = new Mock<ILogger<Application>>();
            var searchStrategyMock = new Mock<ISearchStrategy<SearchFightSearchParametersModel>>();
            var testee = new Application(loggerMock.Object, searchStrategyMock.Object);
            var result = await testee.ExecuteAsync(null);
            result.Should().Be(0);
            searchStrategyMock.VerifyAll();
        }

        [Test]
        public async Task ExecuteAsyncPositive3()
        {
            var loggerMock = new Mock<ILogger<Application>>();
            var searchStrategyMock = new Mock<ISearchStrategy<SearchFightSearchParametersModel>>();
            searchStrategyMock.Setup(x => x.SearchAsync(It.IsAny<SearchFightSearchParametersModel>())).Throws(new SearchException()).Verifiable();
            var testee = new Application(loggerMock.Object, searchStrategyMock.Object);
            var result = await testee.ExecuteAsync(new string[0]);
            result.Should().Be(1);
            searchStrategyMock.VerifyAll();
        }
    }
}
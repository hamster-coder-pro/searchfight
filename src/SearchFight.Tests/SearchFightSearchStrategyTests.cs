using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SearchFight.Common.Interfaces;
using SearchFight.Services.Models;
using SearchFight.Services.Services;

namespace SearchFight.Tests
{
    public class SearchFightSearchStrategyTests
    {
        [Test]
        public void ConstructorSuccess()
        {
            var loggerMock = new Mock<ILogger<SearchFightSearchStrategy>>();
            var serviceFactoryMock = new Mock<ISearchServiceFactory>();

            var testee = new SearchFightSearchStrategy(loggerMock.Object, serviceFactoryMock.Object);

            testee.Should().NotBeNull();
        }

        [Test]
        public async Task SearchAsyncSuccess()
        {
            // arrange
            var loggerMock = new Mock<ILogger<SearchFightSearchStrategy>>();
            var serviceFactoryMock = new Mock<ISearchServiceFactory>();
            var reportProviderMock = new Mock<ISearchReportProvider<SearchFightReportModel>>();
            var searchProviderMock = new Mock<ISearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel>>();
            var reportBuilderMock = new Mock<ISearchReportBuilder<SearchFightSearchResultModel, SearchFightReportModel>>();
            serviceFactoryMock.Setup(x => x.CreateReportProvider<SearchFightReportModel>()).Returns(() => new[] {reportProviderMock.Object}).Verifiable();
            serviceFactoryMock.Setup(x => x.CreateSearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel>()).Returns(() => new[] {searchProviderMock.Object}).Verifiable();
            serviceFactoryMock.Setup(x => x.CreateReportBuilder<SearchFightSearchResultModel, SearchFightReportModel>()).Returns(() => reportBuilderMock.Object).Verifiable();
            
            var testee = new SearchFightSearchStrategy(loggerMock.Object, serviceFactoryMock.Object);

            // act
            await testee.SearchAsync(new SearchFightSearchParametersModel(new[] {"test", "test2", "test3"}));

            // assert
            serviceFactoryMock.Verify(x => x.CreateReportProvider<SearchFightReportModel>(), Times.Once);
            serviceFactoryMock.Verify(x => x.CreateSearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel>(), Times.Once);
            serviceFactoryMock.Verify(x => x.CreateReportBuilder<SearchFightSearchResultModel, SearchFightReportModel>(), Times.Once);
            reportProviderMock.Verify(x => x.ReportAsync(It.IsAny<SearchFightReportModel>()), Times.Once);
            searchProviderMock.Verify(x => x.SearchAsync(It.IsAny<SearchFightSearchRequestModel>()), Times.Exactly(3));
            reportBuilderMock.Verify(x => x.ExecuteAsync(It.IsAny<ICollection<SearchFightSearchResultModel>>()), Times.Once);
        }

        [Test]
        public void SearchAsyncException()
        {
            var loggerMock = new Mock<ILogger<SearchFightSearchStrategy>>();
            var serviceFactoryMock = new Mock<ISearchServiceFactory>();

            var testee = new SearchFightSearchStrategy(loggerMock.Object, serviceFactoryMock.Object);
            Assert.ThrowsAsync<ArgumentNullException>(async () => await testee.SearchAsync(null));
        }
    }
}
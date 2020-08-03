using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SearchFight.Services.Models;
using SearchFight.Services.Services;

namespace SearchFight.Tests
{
    public class SearchFightConsoleReportProviderTests
    {
        [Test]
        public void ConstructorSuccess()
        {
            var testee = new SearchFightConsoleReportProvider();
            testee.Should().NotBeNull();
        }

        [Test]
        public async Task ReportAsyncSuccess()
        {
            var report = new SearchFightReportModel();
            report.KeywordPerSearchEngineWinnerCollection.Add(new SearchFightReportModel.WinnerPerSearchEngineModel("MSN", "test"));
            report.KeywordCollection.Add(
                new SearchFightReportModel.SearchResultsPerKeywordModel(
                    "test"
                    , new[]
                    {
                        new SearchFightReportModel.SearchResultsPerKeywordModel.SearchResultModel
                        {
                            ResultCount = 10
                            , SearchEngine = "Google"
                        }
                        , new SearchFightReportModel.SearchResultsPerKeywordModel.SearchResultModel
                        {
                            ResultCount = 20
                            , SearchEngine = "MSN"
                        }
                        , new SearchFightReportModel.SearchResultsPerKeywordModel.SearchResultModel
                        {
                            ResultCount = -1
                            , SearchEngine = "Yahoo"
                        }
                    }
                )
            );
            report.TotalWinnerKeyword = "test";


            var testee = new SearchFightConsoleReportProvider();
            await testee.ReportAsync(report);
        }

        [Test]
        public async Task ReportAsyncException()
        {
            var testee = new SearchFightConsoleReportProvider();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await testee.ReportAsync(null));
        }
    }
}
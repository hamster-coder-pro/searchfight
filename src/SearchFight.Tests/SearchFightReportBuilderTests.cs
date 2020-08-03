using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SearchFight.Services.Models;
using SearchFight.Services.Services;

namespace SearchFight.Tests
{
    public class SearchFightReportBuilderTests
    {
        [Test]
        public void ConstructorSuccess()
        {
            var testee = new SearchFightReportBuilder();
            testee.Should().NotBeNull();
        }

        [Test]
        public async Task ReportAsyncSuccess()
        {
            var testee = new SearchFightReportBuilder();
            var results = new[]
            {
                new SearchFightSearchResultModel(new SearchFightSearchRequestModel("test"), "Google")
                {
                    IsSucceed = true
                    , Error = null
                    , ResultCount = 10
                }
                , new SearchFightSearchResultModel(new SearchFightSearchRequestModel("test"), "Yahoo")
                {
                    IsSucceed = false
                    , Error = "Some error"
                    , ResultCount = 0
                }
                , new SearchFightSearchResultModel(new SearchFightSearchRequestModel("test"), "MSN")
                {
                    IsSucceed = true
                    , Error = null
                    , ResultCount = 20
                }
            };
            var expectedResult = new SearchFightReportModel();
            expectedResult.KeywordPerSearchEngineWinnerCollection.Add(new SearchFightReportModel.WinnerPerSearchEngineModel("MSN", "test"));
            expectedResult.KeywordPerSearchEngineWinnerCollection.Add(new SearchFightReportModel.WinnerPerSearchEngineModel("Google", "test"));
            expectedResult.KeywordPerSearchEngineWinnerCollection.Add(new SearchFightReportModel.WinnerPerSearchEngineModel("Yahoo", "test"));
            expectedResult.KeywordCollection.Add(
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
            expectedResult.TotalWinnerKeyword = "test";

            var actualResult = await testee.ExecuteAsync(results);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
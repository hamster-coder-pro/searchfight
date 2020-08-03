using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Search.Common;
using SearchFight.Common.Interfaces;
using SearchFight.Services.Models;

namespace SearchFight.Services.Services
{
    internal sealed class SearchFightSearchStrategy : SearchStrategyBase<SearchFightSearchParametersModel, SearchFightSearchStrategy>
    {
        private ISearchServiceFactory ServiceFactory { get; }
        
        // can be configured in application settings // 10 parallel tasks for demo reasons
        private SemaphoreSlim SearchThrottler { get; } = new SemaphoreSlim(10);

        public SearchFightSearchStrategy(ILogger<SearchFightSearchStrategy> logger, ISearchServiceFactory serviceFactory): base(logger)
        {
            ServiceFactory = serviceFactory;
        }

        private IEnumerable<Task<SearchFightSearchResultModel>> CreateSearchTasks(IEnumerable<string> keywords)
        {
            var dataSearcherCollection = ServiceFactory.CreateSearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel>();

            foreach (var keyword in keywords)
            {
                foreach (var dataSearcher in dataSearcherCollection)
                {
                    yield return ThrottleSearcherAsync(dataSearcher, keyword);
                }
            }
        }

        public override Task SearchAsync(SearchFightSearchParametersModel parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return SearchInternalAsync(parameters);
        }

        // S4457 - Sonar
        internal async Task SearchInternalAsync(SearchFightSearchParametersModel parameters)
        {
            var results = await Task.WhenAll(CreateSearchTasks(parameters.Keywords));
            var aggregate = await ServiceFactory.CreateReportBuilder<SearchFightSearchResultModel, SearchFightReportModel>().ExecuteAsync(results);

            var reporters = ServiceFactory.CreateReportProvider<SearchFightReportModel>();
            await Task.WhenAll(reporters.Select(x => x.ReportAsync(aggregate)));
        }


        private async Task<SearchFightSearchResultModel> ThrottleSearcherAsync(ISearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel> searcher, string keyword)
        {
            await SearchThrottler.WaitAsync();
            try
            {
                return await searcher.SearchAsync(new SearchFightSearchRequestModel(keyword));
            }
            finally
            {
                SearchThrottler.Release();
            }
        }
    }
}
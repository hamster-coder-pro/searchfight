using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Search.Common;
using SearchFight.Common.Interfaces;
using SearchFight.Common.Models;

namespace SearchFight.Common.Services
{
    public class MicrosoftDependencyInjectionSearchServiceFactory : ISearchServiceFactory
    {
        private IServiceProvider ServiceProvider { get; }

        public MicrosoftDependencyInjectionSearchServiceFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public ISearchReportBuilder<TSearchResult, TAggregate> CreateReportBuilder<TSearchResult, TAggregate>()
            where TSearchResult : ISearchResultModel
            where TAggregate : ISearchReportModel
        {
            try
            {
                return ServiceProvider.GetRequiredService<ISearchReportBuilder<TSearchResult, TAggregate>>();
            }
            catch (Exception exception)
            {
                throw new SearchException($"Search Aggregator from \"{typeof(TSearchResult).FullName}\" to \"{typeof(TAggregate).FullName}\" not found.", exception);
            }
        }

        public ICollection<ISearchProvider<TRequest, TResult>> CreateSearchProvider<TRequest, TResult>()
            where TRequest : ISearchRequestModel
            where TResult : ISearchResultModel
        {
            try
            {
                return (ServiceProvider.GetServices<ISearchProvider<TRequest, TResult>>() ?? Enumerable.Empty<ISearchProvider<TRequest, TResult>>()).ToArray();
            }
            catch (Exception exception)
            {
                throw new SearchException($"Unexpected exception of getting Data \"{typeof(TRequest)}\" searchers.", exception);
            }
        }

        public ICollection<ISearchReportProvider<TReport>> CreateReportProvider<TReport>()
            where TReport : ISearchReportModel
        {
            try
            {
                return ServiceProvider.GetServices<ISearchReportProvider<TReport>>().ToArray();
            }
            catch (Exception exception)
            {
                throw new SearchException($"Report Provider \"{typeof(TReport).FullName}\" not found.", exception);
            }
        }
    }
}
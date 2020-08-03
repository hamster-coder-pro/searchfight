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

        public ISearchReportBuilder<TSearchResult, TReport> CreateReportBuilder<TSearchResult, TReport>()
            where TSearchResult : ISearchResultModel
            where TReport : ISearchReportModel
        {
            try
            {
                return ServiceProvider.GetRequiredService<ISearchReportBuilder<TSearchResult, TReport>>();
            }
            catch (Exception exception)
            {
                throw new SearchException($"Report Builder from \"{typeof(TSearchResult)}\" to \"{typeof(TReport)}\" not found.", exception);
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
                throw new SearchException($"Error resolving search providers from \"{typeof(TRequest)}\" to \"{typeof(TResult)}\".", exception);
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
                throw new SearchException($"Error resolving report providers for \"{typeof(TReport)}\".", exception);
            }
        }
    }
}
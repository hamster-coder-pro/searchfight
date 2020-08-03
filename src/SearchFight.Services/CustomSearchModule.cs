using Microsoft.Extensions.DependencyInjection;
using Search.Common;
using Search.Common.DI;
using SearchFight.Common.Interfaces;
using SearchFight.Common.Services;
using SearchFight.Services.Interfaces;
using SearchFight.Services.Models;
using SearchFight.Services.Services;

namespace SearchFight.Services
{
    public sealed class CustomSearchModule: IModule
    {
        public void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient(KnownHttpClients.Google, client =>
            {
                client.DefaultRequestHeaders.Add(
                    "user-agent"
                    , "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36"
                );
            });
            serviceCollection.AddHttpClient(KnownHttpClients.Yahoo, client =>
            {
                client.DefaultRequestHeaders.Add(
                    "user-agent"
                    , "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36"
                );
            });
            serviceCollection.AddHttpClient(KnownHttpClients.Bing, client =>
            {
                client.DefaultRequestHeaders.Add(
                    "user-agent"
                    , "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36"
                );
            });

            serviceCollection.AddSingleton<IHtmlParser, HtmlParser>();

            serviceCollection.AddSingleton<ISearchStrategy<SearchFightSearchParametersModel>, SearchFightSearchStrategy>();
            serviceCollection.AddSingleton<ISearchServiceFactory, MicrosoftDependencyInjectionSearchServiceFactory>();
            // register data searchers
            serviceCollection.AddTransient<ISearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel>, GoogleSearchProvider>();
            serviceCollection.AddTransient<ISearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel>, YahooSearchProvider>();
            serviceCollection.AddTransient<ISearchProvider<SearchFightSearchRequestModel, SearchFightSearchResultModel>, BingSearchProvider>();
            // register result aggregators
            serviceCollection.AddTransient<ISearchReportBuilder<SearchFightSearchResultModel, SearchFightReportModel>, SearchFightReportBuilder>();
            // register result reporters
            serviceCollection.AddTransient<ISearchReportProvider<SearchFightReportModel>, SearchFightConsoleReportProvider>();
        }
    }
}

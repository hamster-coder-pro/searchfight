using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Search.Common;
using SearchFight.Services.Models;

namespace SearchFight.Console.Application
{
    internal sealed class Application : IApplication
    {
        private ISearchStrategy<SearchFightSearchParametersModel> SearchService { get; }
        private ILogger Logger { get; }

        public Application(ILogger<Application> logger, ISearchStrategy<SearchFightSearchParametersModel> searchService)
        {
            SearchService = searchService;
            Logger = logger;
        }

        public async Task<int> Execute(string[] args)
        {
            Logger.Log(LogLevel.Information, "Application.Execute() started ...");
            try
            {
                var keywords = args.Except(new[] {"--wait"}).ToArray();
                if (keywords.Length == 0)
                {
                    keywords = new[] {"hamster-coder-pro"};
                }

                await SearchService.SearchAsync(new SearchFightSearchParametersModel(keywords));

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, ex.Message);
                return 1;
            }
            finally
            {
                Logger.Log(LogLevel.Information, "Application.Execute() completed ...");
            }
        }
    }
}
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search.Common.Extensions;
using SearchFight.Common.Interfaces;
using SearchFight.Services.Models;

namespace SearchFight.Services.Services
{
    internal class SearchFightConsoleReportProvider : ISearchReportProvider<SearchFightReportModel>
    {
        public Task ReportAsync(SearchFightReportModel report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            return InternalReportAsync(report);
        }

        internal Task InternalReportAsync(SearchFightReportModel report)
        {
            string line = string.Concat(Enumerable.Repeat("-", 30));

            System.Console.WriteLine(line);
            System.Console.WriteLine("RESULTS PER KEYWORD");
            foreach (var item in report.KeywordCollection)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append($"| {item.Keyword.Overflow(20),-20} |");
                foreach (var result in item.Results)
                {
                    stringBuilder.Append($" {result.SearchEngine,-10} - {(result.ResultCount == -1 ? "ERR" : result.ResultCount.ToString()),10} |");
                }

                System.Console.WriteLine(stringBuilder);
            }

            System.Console.WriteLine();
            System.Console.WriteLine("WINNERS PER SEARCH ENGINE");
            foreach (var perSearchEngine in report.KeywordPerSearchEngineWinnerCollection)
            {
                System.Console.WriteLine($"{perSearchEngine.SearchEngine} winner: {perSearchEngine.Keyword}");
            }

            System.Console.WriteLine();
            System.Console.WriteLine($"TOTAL WINNER: {report.TotalWinnerKeyword}");
            System.Console.WriteLine(line);

            return Task.CompletedTask;
        }
    }
}
using System.Threading.Tasks;
using SearchFight.Common.Models;

namespace SearchFight.Common.Interfaces
{
    public interface ISearchReportProvider<in TReport>
        where TReport : ISearchReportModel
    {
        Task ReportAsync(TReport report);
    }
}
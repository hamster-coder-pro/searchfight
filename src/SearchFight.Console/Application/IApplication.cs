using System.Threading.Tasks;

namespace SearchFight.Console.Application
{
    internal interface IApplication
    {
        Task<int> Execute(string[] args);
    }
}
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Search.Common
{
    public abstract class SearchStrategyBase<TParameters, TInstance> : ISearchStrategy<TParameters>
        where TParameters : ISearchParametersModel
        where TInstance: SearchStrategyBase<TParameters, TInstance>
    {
        protected ILogger<TInstance> Logger { get; }

        protected SearchStrategyBase(ILogger<TInstance> logger)
        {
            Logger = logger;
        }

        public abstract Task SearchAsync(TParameters parameters);
    }
}
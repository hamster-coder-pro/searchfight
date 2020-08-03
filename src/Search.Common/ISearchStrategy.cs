using System.Threading.Tasks;

namespace Search.Common
{
    public interface ISearchStrategy<in TParameter>
        where TParameter: ISearchParametersModel
    {
        Task SearchAsync(TParameter parameters);
    }
}
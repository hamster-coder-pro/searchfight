using System.Collections.Generic;
using System.Linq;
using Search.Common;
using Search.Common.Extensions;

namespace SearchFight.Services.Models
{
    public class SearchFightSearchParametersModel: ISearchParametersModel
    {
        public SearchFightSearchParametersModel(IEnumerable<string> keywords)
        {
            Keywords = keywords.Safe().ToArray();
        }

        public IEnumerable<string> Keywords { get; }
    }
}
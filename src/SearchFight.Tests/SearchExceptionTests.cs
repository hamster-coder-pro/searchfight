using System;
using Search.Common;

namespace SearchFight.Tests
{
    public class SearchExceptionTests: ExceptionTestsBase<SearchException>
    {
        protected override SearchException CreateException()
        {
            return new SearchException();
        }

        protected override SearchException CreateException(string exceptionMessage)
        {
            return new SearchException(exceptionMessage);
        }

        protected override SearchException CreateException(string exceptionMessage, Exception innerException)
        {
            return new SearchException(exceptionMessage, innerException);
        }
    }
}
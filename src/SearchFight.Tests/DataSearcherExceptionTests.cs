using System;
using SearchFight.Services.Exceptions;

namespace SearchFight.Tests
{
    public class DataSearcherExceptionTests: ExceptionTestsBase<DataSearcherException>
    {
        protected override DataSearcherException CreateException()
        {
            return new DataSearcherException();
        }

        protected override DataSearcherException CreateException(string exceptionMessage)
        {
            return new DataSearcherException(exceptionMessage);
        }

        protected override DataSearcherException CreateException(string exceptionMessage, Exception innerException)
        {
            return new DataSearcherException(exceptionMessage, innerException);
        }
    }
}
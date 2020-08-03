using System;
using System.Runtime.Serialization;
using Search.Common;

namespace SearchFight.Services.Exceptions
{
    [Serializable]
    public class DataSearcherException : SearchException
    {
        public DataSearcherException()
        {
        }

        public DataSearcherException(string? message)
            : base(message)
        {
        }

        public DataSearcherException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected DataSearcherException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
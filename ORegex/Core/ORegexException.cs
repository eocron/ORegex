using System;
using System.Runtime.Serialization;

namespace Eocron.Core
{
    [Serializable]
    public class ORegexException : Exception
    {
        public ORegexException()
        { }

        public ORegexException(string message)
            : base(message)
        { }

        public ORegexException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ORegexException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

    }
}

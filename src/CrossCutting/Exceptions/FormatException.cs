using System;
using System.Runtime.Serialization;

namespace CrossCutting.Exceptions
{
    [Serializable]
    public class FormatException : Exception
    {
        // Constructors
        public FormatException(string message)
            : base(message)
        { }

        // Ensure Exception is Serializable
        protected FormatException(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        { }
    }
}
using System;
using System.Runtime.Serialization;

namespace CrossCutting.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        // Constructors
        public ValidationException(string message)
            : base(message)
        { }

        // Ensure Exception is Serializable
        protected ValidationException(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        { }
    }
}
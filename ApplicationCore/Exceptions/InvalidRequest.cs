using System;

namespace ApplicationCore.Exceptions
{
    public class InvalidRequest : Exception
    {
        public InvalidRequest() : base() { }
        public InvalidRequest(string message) : base(message) { }
    }
}

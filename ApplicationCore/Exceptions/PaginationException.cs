using System;

namespace ApplicationCore.Exceptions
{
    public class PaginationException : Exception
    {
        public PaginationException() { }

        public PaginationException(string message) : base(message) { }
    }
}

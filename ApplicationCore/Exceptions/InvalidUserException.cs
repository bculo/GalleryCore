using System;

namespace ApplicationCore.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException() { }

        public InvalidUserException(string errorMesage) : base(errorMesage) { }
    }
}

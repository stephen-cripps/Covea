using System;

namespace Covea.Application.Exceptions
{
    /// <summary>
    /// The bad request exception should be thrown whenever an invalid input is encountered in the application layer
    /// </summary>
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {}
    }
}

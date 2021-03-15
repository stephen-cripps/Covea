using System;

namespace Covea.Application.Exceptions
{
    public class ServiceNotAvailableException : ApplicationException
    {
        /// <summary>
        /// This exception is thrown when an external service cannot be accessed
        /// </summary>
        /// <param name="message"></param>
        public ServiceNotAvailableException(string serviceName) : base($"{serviceName} is not available")
        { }
    }
}
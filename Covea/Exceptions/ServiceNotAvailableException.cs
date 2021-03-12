using System;

namespace BeerQuest.Application.Exceptions
{
    public class ServiceNotAvailableException : ApplicationException
    {
        /// <summary>
        /// To be thrown when an external service cannot be accessed
        /// </summary>
        /// <param name="message"></param>
        public ServiceNotAvailableException(string serviceName) : base($"{serviceName} is not available")
        { }
    }
}
using System;

namespace Covea.Application.Exceptions
{
    /// <summary>
    /// This exception should be thrown when an applicant type is not recognised
    /// </summary>
    public class InvalidApplicantTypeException : ApplicationException
    {
        public InvalidApplicantTypeException(string type) : base($"The applicant type ${type} is not valid")
        {}
    }
}

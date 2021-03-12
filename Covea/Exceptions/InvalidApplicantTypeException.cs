using System;

namespace Covea.Application.Exceptions
{
    public class InvalidApplicantTypeException : ApplicationException
    {
        public InvalidApplicantTypeException(string type) : base($"The applicant type ${type} is not valid")
        {}
    }
}

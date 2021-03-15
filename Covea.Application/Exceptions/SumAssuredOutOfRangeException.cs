namespace Covea.Application.Exceptions
{
    public class SumAssuredOutOfRangeException : BadRequestException
    {
        /// <summary>
        /// This exception should be thrown when the sum assured is not valid for a specific applicant's age
        /// </summary>
        public SumAssuredOutOfRangeException() : base("SumAssured is out of range for the applicant's age")
        { }
    }
}

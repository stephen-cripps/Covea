namespace Covea.Application.Exceptions
{
    public class SumAssuredOutOfRangeException : BadRequestException
    {
        public SumAssuredOutOfRangeException() : base("SumAssured is out of range for the applicant's age")
        { }
    }
}

namespace Covea.Application.Models.Applicants
{
    public interface IApplicantFactory
    {
        IApplicant CreateApplicant(int age, int sumAssured, string applicantType);
    }
}

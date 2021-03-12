namespace Covea.Application.Models.Applicants
{
    public interface IApplicant
    {
        int Age { get; }
        int SumAssured { get; }
        double CalculateGrossPremium();
    }
}

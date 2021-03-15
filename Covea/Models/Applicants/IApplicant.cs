namespace Covea.Application.Models.Applicants
{
    /// <summary>
    /// Defines an applicant. All applicants must have an Age, SumAssured and a method to calculate the gross premium. 
    /// </summary>
    public interface IApplicant
    {
        int Age { get; }
        int SumAssured { get; }
        double CalculateGrossPremium();
    }
}

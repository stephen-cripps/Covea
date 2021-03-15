using System.Threading.Tasks;

namespace Covea.Application.Models.Applicants
{
    public interface IApplicantFactory
    {
        Task<Applicant> CreateApplicantAsync(int age, int sumAssured, string applicantType);
    }
}

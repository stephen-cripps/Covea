using System.Collections.Generic;
using Covea.Application.Models.Strategies;

namespace Covea.Application.Models.Applicants
{
    public interface IApplicant
    {
        int Age { get; }
        int SumAssured { get; }
        double CalculateGrossPremium();
    }
}

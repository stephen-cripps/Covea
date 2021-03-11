using System.Collections.Generic;
using Covea.Application.Models.Strategies;

namespace Covea.Application.Models.Applicants
{
    public class BasicApplicant : IApplicant
    {
        public int Age { get; }
        public int SumAssured { get; }
        public ICostingStrategy GrossPremiumStrategy { get; }

        public BasicApplicant(int age, int sumAssured, ICostingStrategy grossPremiumStrategy)
        {
            Age = age;
            SumAssured = sumAssured;
            GrossPremiumStrategy = grossPremiumStrategy;
        }

        public double CalculateGrossPremium() => GrossPremiumStrategy.CalculateCost();
    }
}

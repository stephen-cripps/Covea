using System;
using Covea.Application.Models.Strategies;

namespace Covea.Application.Models.Applicants
{
    /// <summary>
    /// The basic implementation of an applicant applicant contains a costing strategy and method for calculating the Gross premium which depends on that costing strategy's CalculateCost method
    /// </summary>
    public class BasicApplicant : IApplicant
    {
        public int Age { get; }
        public int SumAssured { get; }
        public ICostingStrategy GrossPremiumStrategy { get; }

        public BasicApplicant(int age, int sumAssured, ICostingStrategy grossPremiumStrategy)
        {
            if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age));
            if (sumAssured <= 0) throw new ArgumentOutOfRangeException(nameof(sumAssured));

            Age = age;
            SumAssured = sumAssured;
            GrossPremiumStrategy = grossPremiumStrategy;
        }

        public double CalculateGrossPremium() => GrossPremiumStrategy.CalculateCost();
    }
}

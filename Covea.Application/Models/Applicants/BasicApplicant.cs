using System;
using Covea.Application.Models.Strategies;

namespace Covea.Application.Models.Applicants
{
    /// <summary>
    /// The basic implementation of an applicant applicant contains a costing strategy and method for calculating the Gross premium which depends on that costing strategy's CalculateCost method
    /// </summary>
    public class BasicApplicant : Applicant
    {
        public ICostingStrategy GrossPremiumStrategy { get; }

        public BasicApplicant(int age, int sumAssured, ICostingStrategy grossPremiumStrategy) : base(age, sumAssured)
        {
            GrossPremiumStrategy = grossPremiumStrategy;
        }

        public override double CalculateGrossPremium() => GrossPremiumStrategy.CalculateCost();
    }
}

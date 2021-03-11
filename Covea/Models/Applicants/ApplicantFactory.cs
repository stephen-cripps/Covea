using System;
using System.Threading.Tasks;
using Covea.Application.Models.Strategies;
using Covea.Application.Storage;

namespace Covea.Application.Models.Applicants
{
    public class ApplicantFactory : IApplicantFactory
    {
        readonly IRiskRateRepository riskRateRepo;

        public ApplicantFactory(IRiskRateRepository riskRateRepo)
        {
            this.riskRateRepo = riskRateRepo;
        }
        public IApplicant CreateApplicant(int age, int sumAssured, string applicantType)
        {
            return applicantType switch
            {
                "basic" => CreateBasicApplicant(age, sumAssured, applicantType),
                _ => throw new ArgumentOutOfRangeException(nameof(applicantType), "Type not recognised")
            };
        }

        BasicApplicant CreateBasicApplicant(int age, int sumAssured, string applicantType)
        {
            var lowerBand = riskRateRepo.GetLowerBand(sumAssured);
            var upperBand = riskRateRepo.GetUpperBand(sumAssured);

            var riskRate = CalculateRiskRate(sumAssured, age, lowerBand, upperBand);

            var riskPremiumStrategy = new BasicRiskPremiumStrategy(riskRate, sumAssured);
            var renewalCommissionStrategy = new BasicRenewalCommissionStrategy(riskPremiumStrategy);
            var netPremiumStrategy = new BasicNetPremiumStrategy(riskPremiumStrategy, renewalCommissionStrategy);
            var initialCommissionStrategy = new BasicInitialCommissionStrategy(netPremiumStrategy);
            var grossPremiumStrategy = new BasicGrossPremiumStrategy(initialCommissionStrategy, netPremiumStrategy);

            return new BasicApplicant(age, sumAssured, grossPremiumStrategy);
        }

        double CalculateRiskRate(int sumAssured, int age, RiskBand lowerBand, RiskBand upperBand) =>
            (((double)(sumAssured - lowerBand.SumAssured) / (upperBand.SumAssured - lowerBand.SumAssured)) *
             upperBand.GetRiskRate(age)) +
                (((double)(upperBand.SumAssured - sumAssured) / (upperBand.SumAssured - lowerBand.SumAssured)) *
                 lowerBand.GetRiskRate(age));
        
    }
}

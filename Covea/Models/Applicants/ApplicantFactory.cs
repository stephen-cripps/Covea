using System;
using System.Threading.Tasks;
using BeerQuest.Application.Exceptions;
using Covea.Application.Exceptions;
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
        public async Task<IApplicant> CreateApplicantAsync(int age, int sumAssured, string applicantType)
        {
            return applicantType switch
            {
                "basic" => await CreateBasicApplicant(age, sumAssured),
                _ => throw new InvalidApplicantTypeException(applicantType)
            };
        }

        async Task<BasicApplicant> CreateBasicApplicant(int age, int sumAssured)
        {
            if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age));
            if (sumAssured <= 0) throw new ArgumentOutOfRangeException(nameof(sumAssured));

            RiskBand lowerBand;
            RiskBand upperBand;
            try
            {
                lowerBand = await riskRateRepo.GetLowerBandAsync(sumAssured);
                upperBand = await riskRateRepo.GetUpperBandAsync(sumAssured);
            }
            catch
            {
                throw new ServiceNotAvailableException(nameof(riskRateRepo));
            }

            if (lowerBand?.GetRiskRate(age) == null || upperBand?.GetRiskRate(age) == null)
                throw new SumAssuredOutOfRangeException();

            var riskRate = CalculateRiskRate(sumAssured, age, lowerBand, upperBand);

            var riskPremiumStrategy = new BasicRiskPremiumStrategy(riskRate, sumAssured);
            var renewalCommissionStrategy = new BasicRenewalCommissionStrategy(riskPremiumStrategy);
            var netPremiumStrategy = new BasicNetPremiumStrategy(riskPremiumStrategy, renewalCommissionStrategy);
            var initialCommissionStrategy = new BasicInitialCommissionStrategy(netPremiumStrategy);
            var grossPremiumStrategy = new BasicGrossPremiumStrategy(initialCommissionStrategy, netPremiumStrategy);

            return new BasicApplicant(age, sumAssured, grossPremiumStrategy);
        }

        double CalculateRiskRate(int sumAssured, int age, RiskBand lowerBand, RiskBand upperBand) =>
            (double)((((double)(sumAssured - lowerBand.SumAssured) / (upperBand.SumAssured - lowerBand.SumAssured)) *
                       upperBand.GetRiskRate(age)) +
                      (((double)(upperBand.SumAssured - sumAssured) / (upperBand.SumAssured - lowerBand.SumAssured)) *
                       lowerBand.GetRiskRate(age)));

    }
}

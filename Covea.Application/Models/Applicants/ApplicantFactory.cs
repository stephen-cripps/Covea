using System;
using System.Threading.Tasks;
using Covea.Application.Exceptions;
using Covea.Application.Models.Strategies;
using Covea.Application.Storage;

namespace Covea.Application.Models.Applicants
{
    /// <summary>
    /// The applicant factory is used to instantiate an applicant. It is built to allow for different type of applicants with different requirements
    /// </summary>
    public class ApplicantFactory : IApplicantFactory
    {
        readonly IRiskRateRepository riskRateRepo;

        public ApplicantFactory(IRiskRateRepository riskRateRepo)
        {
            this.riskRateRepo = riskRateRepo;
        }

        /// <summary>
        /// This method calls a specific method for instantiating a concrete type of applicant
        /// </summary>
        /// <param name="age"></param>
        /// <param name="sumAssured"></param>
        /// <param name="applicantType">Currently only accepts "basic"</param>
        /// <returns></returns>
        public async Task<Applicant> CreateApplicantAsync(int age, int sumAssured, string applicantType)
        {
            return applicantType switch
            {
                "basic" => await CreateBasicApplicant(age, sumAssured),
                _ => throw new InvalidApplicantTypeException(applicantType)
            };
        }

        /// <summary>
        /// This method instantiates a BasicApplicant with all "basic" strategies
        /// </summary>
        /// <param name="age"></param>
        /// <param name="sumAssured"></param>
        /// <returns></returns>
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

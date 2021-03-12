using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covea.Application.Exceptions;
using Covea.Application.Models;
using Covea.Application.Models.Applicants;
using Covea.Application.Models.Strategies;
using Covea.Application.Storage;
using Moq;
using Xunit;

namespace Covea.Testing.Unit_Tests
{
    public class ApplicantTests
    {
        readonly ApplicantFactory factory;
        readonly Mock<IRiskRateRepository> mockRepo = new Mock<IRiskRateRepository>();

        public ApplicantTests()
        {
            factory = new ApplicantFactory(mockRepo.Object);

            var lowerBand = new RiskBand()
            {
                RiskRates = new Dictionary<string, double?>() { { "A", 1000 }, { "B", 2000 }, { "C", 3000 } },
                SumAssured = 1000
            };
            var upperBand = new RiskBand()
            {
                RiskRates = new Dictionary<string, double?>() { { "A", 10000 }, { "B", 20000 }, { "C", 30000 } },
                SumAssured = 2000
            };

            mockRepo.Setup(m => m.GetLowerBandAsync(It.IsAny<int>())).Returns(Task.FromResult(lowerBand));
            mockRepo.Setup(m => m.GetUpperBandAsync(It.IsAny<int>())).Returns(Task.FromResult(upperBand));
        }

        #region FactoryTests

        [Fact]
        public async Task CreateApplicant_BasicApplicant_ReturnsBasicApplicant()
        {
            //Arrange
            const int sumAssured = 1500;
            const int age = 20;

            //test
            var applicant = await factory.CreateApplicantAsync(age, sumAssured, "basic");

            //Assert
            Assert.Equal(typeof(BasicApplicant), applicant.GetType());

            var basicApplicant = applicant as BasicApplicant;
            Assert.Equal(age, basicApplicant.Age);
            Assert.Equal(sumAssured, basicApplicant.SumAssured);
            Assert.Equal(typeof(BasicGrossPremiumStrategy), basicApplicant.GrossPremiumStrategy.GetType());

            var grossPremiumStrategy = basicApplicant.GrossPremiumStrategy as BasicGrossPremiumStrategy;

            Assert.Equal(typeof(BasicInitialCommissionStrategy), grossPremiumStrategy.initialCommissionStrategy.GetType());
            Assert.Equal(typeof(BasicNetPremiumStrategy), grossPremiumStrategy.netPremiumStrategy.GetType());

            var netPremiumStrategy = grossPremiumStrategy.netPremiumStrategy as BasicNetPremiumStrategy;
            var initialCommissionStrategy = grossPremiumStrategy.initialCommissionStrategy as BasicInitialCommissionStrategy;

            Assert.Equal(typeof(BasicNetPremiumStrategy), initialCommissionStrategy.netPremiumStrategy.GetType());

            Assert.Equal(typeof(BasicRenewalCommissionStrategy), netPremiumStrategy.renewalCommissionStrategy.GetType());
            Assert.Equal(typeof(BasicRiskPremiumStrategy), netPremiumStrategy.riskPremiumStrategy.GetType());

            var renewalComissionStrategy = netPremiumStrategy.renewalCommissionStrategy as BasicRenewalCommissionStrategy;
            var riskPremiumStrategy = netPremiumStrategy.riskPremiumStrategy as BasicRiskPremiumStrategy;

            Assert.Equal(typeof(BasicRiskPremiumStrategy), renewalComissionStrategy.riskPremiumStrategy.GetType());

            Assert.Equal(sumAssured, riskPremiumStrategy.sumAssured);
            Assert.Equal(5500, riskPremiumStrategy.riskRate);
        }

        [Fact]
        public void CreateApplicant_InvalidApplicant_ThrowsInvalidApplicantTypeException()
        {
            Assert.ThrowsAsync<InvalidApplicantTypeException>(async () => await factory.CreateApplicantAsync(20, 1500, "invalid"));
        }

        [Fact]
        public async Task CreateApplicant_InvalidAge_ThrowsArgumentOutOfRangeException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await factory.CreateApplicantAsync(-20, 1500, "basic"));
            Assert.Equal("Specified argument was out of the range of valid values. (Parameter 'age')", exception.Message);
        }

        [Fact]
        public async Task CreateApplicant_InvalidSumAssured_ThrowsArgumentOutOfRangeException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await factory.CreateApplicantAsync(20, -1500, "basic"));
            Assert.Equal("Specified argument was out of the range of valid values. (Parameter 'sumAssured')", exception.Message);
        }
        #endregion

        #region BasicApplicant

        [Theory]
        [InlineData(20, 1000, 3141.5)]
        [InlineData(40, 1500, 51834.75)]
        [InlineData(60, 1800, 139105.62)]
        public async Task CalculateGrossPremium_ReturnsExpectedValue(int age, int sumAssured, double expectedValue)
        {
            //Arrange 
            var applicant = await factory.CreateApplicantAsync(age, sumAssured, "basic");

            //Test
            var premium = applicant.CalculateGrossPremium();

            //Assert
            Assert.Equal(expectedValue, premium, 2);
        }

        #endregion

    }
}

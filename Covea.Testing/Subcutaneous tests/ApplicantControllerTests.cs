using System.Collections.Generic;
using System.Threading.Tasks;
using Covea.Application.Controllers;
using Covea.Application.Models.Applicants;
using Covea.Application.Storage;
using Covea.Application.Views;
using Covea.Testing.MockStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Covea.Testing.Subcutaneous_tests
{
    public class ApplicantControllerTests
    {
        ApplicantController controller;
        IConfiguration config;

        public ApplicantControllerTests()
        {
            var host = new HostBuilder().ConfigureServices(services =>
            {
                services.AddTransient<IApplicantFactory, ApplicantFactory>()
    .AddTransient<IRiskRateRepository, MockRiskRateRepository>();
            })
            .Build();

            var configValues = new Dictionary<string, string>
            {
                {"MinimumGrossPremium", "2"},
                {"SumAssuredIterationStep", "5000"}
            };

            config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            controller = new ApplicantController(host.Services.GetRequiredService<IApplicantFactory>(), config);
        }


        [Theory]
        [InlineData(18, 250000, 11.43)]
        [InlineData(30, 50000, 2.59)]
        [InlineData(49, 60000, 18.58)]
        public async Task GetPremium_ValidInputs_ReturnsPremiumData(int age, int sumAssured, double expectedResult)
        {
            //Test
            var resp = await controller.GetGrossPremium(age, sumAssured) as OkObjectResult;

            //Assert
            Assert.NotNull(resp);
            Assert.Equal(200, resp.StatusCode);

            var result = resp.Value as GrossPremiumView; 
            Assert.Equal(age, result.Age);
            Assert.Equal(sumAssured, result.SumAssured);
            Assert.Equal(expectedResult, result.Premium, 2);

        }

        [Fact]
        public async Task GetPremium_GrossLessThanMinimum_ReturnsAdjustedPremiumData()
        {
            //Arrange
            const int sumAssured = 25000;
            const int age = 18;
            const double expectedPremium = 2.11;
            const int adjustedSumAssured = 40000;

            //Test
            var resp = await controller.GetGrossPremium(age, sumAssured) as OkObjectResult;
            var test = await controller.GetGrossPremium(age, sumAssured);
            //Assert
            Assert.NotNull(resp);
            Assert.Equal(200, resp.StatusCode);

            var result = resp.Value as GrossPremiumView;
            Assert.Equal(age, result.Age);
            Assert.Equal(adjustedSumAssured, result.SumAssured);
            Assert.Equal(expectedPremium, result.Premium, 2);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(100)]
        public async Task getPremium_InvalidAge_Returns400(int age)
        {
            //Test
            var resp = await controller.GetGrossPremium(age, 500000) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(resp);
            Assert.Equal(400, resp.StatusCode);
            Assert.Equal("Applicant's age must be between 18 and 65", resp.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5000000)]
        public async Task GetPremium_InvalidSumAssured_Returns400(int sumAssured)
        {
            //Test
            var resp = await controller.GetGrossPremium(20, sumAssured) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(resp);
            Assert.Equal(400, resp.StatusCode);
            Assert.Equal("Sum Assured must be between 25,000 and 500,000", resp.Value);
        }

        [Fact]
        public async Task GetPremium_AgeInvalidSumAssured_Returns400()
        {
            //Test
            var resp = await controller.GetGrossPremium(60, 400000) as BadRequestObjectResult;

            //Assert
            //Assert
            Assert.NotNull(resp);
            Assert.Equal(400, resp.StatusCode);
            Assert.Equal("SumAssured is out of range for the applicant's age", resp.Value);
        }

        [Fact]
        public async Task GetPremium_StorageUnavailable_Returns503()
        {
            //Arrange
            var host = new HostBuilder().ConfigureServices(services =>
            {
                services.AddTransient<IApplicantFactory, ApplicantFactory>()
    .AddTransient<IRiskRateRepository, UnavailableRiskRateRepository>();
            })
            .Build();

            controller = new ApplicantController(host.Services.GetRequiredService<IApplicantFactory>(), config);

            //Test
            var resp = await controller.GetGrossPremium(20, 400000) as StatusCodeResult;

            //Assert
            Assert.NotNull(resp);
            Assert.Equal(503, resp.StatusCode);
        }
    }
}

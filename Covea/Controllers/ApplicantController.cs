using System;
using System.Threading.Tasks;
using Covea.Application.Exceptions;
using Covea.Application.Extensions;
using Covea.Application.Models.Applicants;
using Covea.Application.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Covea.Application.Controllers
{
    /// <summary>
    /// The applicant controller handles all transactions related to an applicant
    /// </summary>
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("Controller")]
    public class ApplicantController : ControllerBase
    {
        readonly IApplicantFactory applicantFactory;
        readonly IConfiguration configuration;

        public ApplicantController(IApplicantFactory applicantFactory, IConfiguration configuration)
        {
            this.applicantFactory = applicantFactory;
            this.configuration = configuration;
        }

        /// <summary>
        /// This endpoint creates an applicant and calculates the gross premium.
        /// In a full solution, the applicant creation and returning the gross premium
        /// would most likely be two separate endpoints as they cover different concerns.
        /// Assuming that this approach matches the business use 
        /// </summary>
        /// <param name="age"></param>
        /// <param name="sumAssured"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("GrossPremium")]
        public async Task<IActionResult> GetGrossPremium(int age, int sumAssured)
        {
            try
            {
                return new OkObjectResult(await GenerateGrossPremium(age, sumAssured));
            }
            catch(ApplicationException e)
            {
                return e.ToActionResult();
            }
        }

        /// <summary>
        /// This method handles the request logic, returning the applicant's gross premium
        /// Where the gross premium is too low, the method is called recursively until it is above the set limit.
        /// In a full application, this would be pulled out into a separate application
        /// layer, aligning to a clean architecture model.
        /// </summary>
        /// <param name="age"></param>
        /// <param name="sumAssured"></param>
        /// <returns></returns>
        async Task<GrossPremiumView> GenerateGrossPremium(int age, int sumAssured)
        {
            if (age < 18 || age > 65)
                throw new BadRequestException("Applicant's age must be between 18 and 65");
            if (sumAssured < 25000 || sumAssured > 500000) 
                throw new BadRequestException("Sum Assured must be between 25,000 and 500,000");

            try
            {
                var applicant = await applicantFactory.CreateApplicantAsync(age, sumAssured, "basic");

                var premium = applicant.CalculateGrossPremium(); 

                if (premium < Convert.ToInt32(configuration["MinimumGrossPremium"]))
                    return await GenerateGrossPremium(age, sumAssured + Convert.ToInt32(configuration["SumAssuredIterationStep"]));

                return new GrossPremiumView()
                {
                    Age = applicant.Age,
                    SumAssured = applicant.SumAssured,
                    Premium = premium
                };
            }
            catch (ArgumentException e)
            {
                throw new BadRequestException(e.Message);
            }
        }
    }

}

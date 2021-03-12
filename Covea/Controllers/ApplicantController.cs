using System;
using System.Threading.Tasks;
using BeerQuest.Functions.Extensions;
using Covea.Application.Exceptions;
using Covea.Application.Models.Applicants;
using Covea.Application.Views;
using Microsoft.AspNetCore.Mvc;

namespace Covea.Application.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("Controller")]
    public class ApplicantController : ControllerBase
    {
        readonly IApplicantFactory applicantFactory;

        public ApplicantController(IApplicantFactory applicantFactory)
        {
            this.applicantFactory = applicantFactory;
        }

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

        //Ina full application, this would be pulled out into a separate application layer
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

                //The hardcoded values here should be ideally stored as environment variables. 
                if (premium < 2)
                    return await GenerateGrossPremium(age, sumAssured + 5000);

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

using System;

namespace Covea.Application.Models.Applicants
{
    /// <summary>
    /// Defines an applicant. All applicants must have an Age, SumAssured and a method to calculate the gross premium. 
    /// </summary>
    public abstract class Applicant
    {
        protected Applicant(int age, int sumAssured)
        {
            if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age));
            if (sumAssured <= 0) throw new ArgumentOutOfRangeException(nameof(sumAssured));

            Age = age;
            SumAssured = sumAssured;
        }
        public int Age { get; }
        public int SumAssured { get; }

        public abstract double CalculateGrossPremium();
    }
}

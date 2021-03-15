using System.Collections.Generic;

namespace Covea.Application.Models
{
    /// <summary>
    /// The risk band defines the risk rates for each age category for a specific sum assured
    /// </summary>
    public class RiskBand
    {
        public int SumAssured { get; set; }
        public Dictionary<string, double?> RiskRates { get; set; }

        /// <summary>
        /// get risk rates determines the age category and returns the relevant risk rate
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        public double? GetRiskRate(int age)
        {
            if (age <= 30)
                return RiskRates["A"];
            else if (age <= 50)
                return RiskRates["B"];
            else
                return RiskRates["C"];
        }
    }
}

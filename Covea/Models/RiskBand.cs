using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covea.Application.Models
{
    public class RiskBand
    {
        public int SumAssured { get; set; }
        public Dictionary<string, double> RiskRates { get; set; }

        public double GetRiskRate(int age)
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

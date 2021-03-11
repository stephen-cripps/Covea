using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covea.Application.Models;

namespace Covea.Application.Storage
{
    public interface IRiskRateRepository
    {
        RiskBand GetLowerBand(int sumAssured);
        RiskBand GetUpperBand(int sumAssured);
    }
}

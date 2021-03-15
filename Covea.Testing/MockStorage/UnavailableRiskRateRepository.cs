using System;
using System.Threading.Tasks;
using Covea.Application.Models;
using Covea.Application.Storage;

namespace Covea.Testing.MockStorage
{
    public class UnavailableRiskRateRepository : IRiskRateRepository
    {
        public Task<RiskBand> GetLowerBandAsync(int sumAssured)
        {
            throw new NotImplementedException();
        }

        public Task<RiskBand> GetUpperBandAsync(int sumAssured)
        {
            throw new NotImplementedException();
        }
    }
}

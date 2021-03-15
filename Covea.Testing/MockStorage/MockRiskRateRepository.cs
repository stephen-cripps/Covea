using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covea.Application.Models;
using Covea.Application.Storage;

namespace Covea.Testing.MockStorage
{
    public class MockRiskRateRepository : IRiskRateRepository
    {
        readonly IEnumerable<RiskBand> mockData = new List<RiskBand>()
        {
            new RiskBand()
            {
                SumAssured = 25000,
                RiskRates = new Dictionary<string, double?>()
                {
                    {"A", 0.0172 },
                    {"B", 0.1043 },
                    {"C", 0.2677 },
                }
            },
            new RiskBand()
            {
                SumAssured = 50000,
                RiskRates = new Dictionary<string, double?>()
                {
                    {"A", 0.0165 },
                    {"B", 0.0999 },
                    {"C", 0.2565 },
                }
            },
            new RiskBand()
            {
                SumAssured = 100000,
                RiskRates = new Dictionary<string, double?>()
                {
                    {"A", 0.0154 },
                    {"B", 0.0932 },
                    {"C", 0.2393 },
                }
            },
            new RiskBand()
            {
                SumAssured = 200000,
                RiskRates = new Dictionary<string, double?>()
                {
                    {"A", 0.0147 },
                    {"B", 0.0887 },
                    {"C", 0.2285 },
                }
            },
            new RiskBand()
            {
                SumAssured = 300000,
                RiskRates = new Dictionary<string, double?>()
                {
                    {"A", 0.0144 },
                    {"B", 0.0872 },
                    {"C", null }
                }
            },
            new RiskBand()
            {
            SumAssured = 500000,
            RiskRates = new Dictionary<string, double?>()
                {
                    {"A", 0.0172 },
                    {"B", null },
                    {"C", null }
                }
            }
        };


        public Task<RiskBand> GetLowerBandAsync(int sumAssured)
        {
            return Task.FromResult(mockData.Where(d => d.SumAssured <= sumAssured)
                .OrderByDescending(d => d.SumAssured)
                .FirstOrDefault());
        }

        public Task<RiskBand> GetUpperBandAsync(int sumAssured)
        {
            return Task.FromResult(mockData.Where(d => d.SumAssured > sumAssured)
                .OrderBy(d => d.SumAssured)
                .FirstOrDefault());
        }
    }
}

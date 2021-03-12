using System.Threading.Tasks;
using Covea.Application.Models;

namespace Covea.Application.Storage
{
    public interface IRiskRateRepository
    {
        Task<RiskBand> GetLowerBandAsync(int sumAssured);
        Task<RiskBand> GetUpperBandAsync(int sumAssured);
    }
}

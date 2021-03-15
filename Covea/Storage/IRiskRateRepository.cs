using System.Threading.Tasks;
using Covea.Application.Models;

namespace Covea.Application.Storage
{
    public interface IRiskRateRepository
    {
        /// <summary>
        /// Gets the first band below or equal to the input SumAssured
        /// </summary>
        /// <param name="sumAssured"></param>
        /// <returns></returns>
        Task<RiskBand> GetLowerBandAsync(int sumAssured);

        /// <summary>
        /// gets the first band above the input SumAssured
        /// </summary>
        /// <param name="sumAssured"></param>
        /// <returns></returns>
        Task<RiskBand> GetUpperBandAsync(int sumAssured);
    }
}

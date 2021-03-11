using System;

namespace Covea.Application.Models.Strategies
{
    public class BasicGrossPremiumStrategy: ICostingStrategy
    {
        readonly ICostingStrategy initialCommissionStrategy;
        readonly ICostingStrategy netPremiumStrategy;

        public BasicGrossPremiumStrategy(ICostingStrategy initialCommissionStrategy, ICostingStrategy netPremiumStrategy)
        {
            this.initialCommissionStrategy = initialCommissionStrategy;
            this.netPremiumStrategy = netPremiumStrategy;
        }

        public double CalculateCost() => initialCommissionStrategy.CalculateCost() + netPremiumStrategy.CalculateCost();

        public static BasicGrossPremiumStrategy Builder()
        {
            throw new NotImplementedException();
        }
    }
}

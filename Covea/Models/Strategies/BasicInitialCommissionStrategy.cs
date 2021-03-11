namespace Covea.Application.Models.Strategies
{
    public class BasicInitialCommissionStrategy : ICostingStrategy
    {
        readonly ICostingStrategy netPremiumStrategy;

        public BasicInitialCommissionStrategy(ICostingStrategy netPremiumStrategy)
        {
            this.netPremiumStrategy = netPremiumStrategy;
        }

        public double CalculateCost() => netPremiumStrategy.CalculateCost() * 2.05;
    }
}

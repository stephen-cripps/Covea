namespace Covea.Application.Models.Strategies
{
    public class BasicGrossPremiumStrategy : ICostingStrategy
    {
        public readonly ICostingStrategy initialCommissionStrategy;
        public readonly ICostingStrategy netPremiumStrategy;

        public BasicGrossPremiumStrategy(ICostingStrategy initialCommissionStrategy, ICostingStrategy netPremiumStrategy)
        {
            this.initialCommissionStrategy = initialCommissionStrategy;
            this.netPremiumStrategy = netPremiumStrategy;
        }

        public double CalculateCost() => initialCommissionStrategy.CalculateCost() + netPremiumStrategy.CalculateCost();
    }
}

namespace Covea.Application.Models.Strategies
{
    public class BasicRenewalCommissionStrategy : ICostingStrategy
    {
        public readonly ICostingStrategy riskPremiumStrategy;

        public BasicRenewalCommissionStrategy(ICostingStrategy riskPremiumStrategy)
        {
            this.riskPremiumStrategy = riskPremiumStrategy;
        }

        public double CalculateCost() => riskPremiumStrategy.CalculateCost() * 0.03;
    }
}

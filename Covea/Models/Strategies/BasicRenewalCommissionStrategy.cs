namespace Covea.Application.Models.Strategies
{
    public class BasicRenewalCommissionStrategy : ICostingStrategy
    {
        readonly ICostingStrategy riskPremiumStrategy;

        public BasicRenewalCommissionStrategy(ICostingStrategy riskPremiumStrategy)
        {
            this.riskPremiumStrategy = riskPremiumStrategy;
        }

        public double CalculateCost() => riskPremiumStrategy.CalculateCost() * 0.03;
    }
}

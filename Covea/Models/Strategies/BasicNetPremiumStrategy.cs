namespace Covea.Application.Models.Strategies
{
    public class BasicNetPremiumStrategy : ICostingStrategy
    {
        public readonly ICostingStrategy riskPremiumStrategy;
        public readonly ICostingStrategy renewalCommissionStrategy;

        public BasicNetPremiumStrategy(ICostingStrategy riskPremiumStrategy, ICostingStrategy renewalCommissionStrategy)
        {
            this.riskPremiumStrategy = riskPremiumStrategy;
            this.renewalCommissionStrategy = renewalCommissionStrategy;
        }

        public double CalculateCost() => riskPremiumStrategy.CalculateCost() + renewalCommissionStrategy.CalculateCost();
    }
}

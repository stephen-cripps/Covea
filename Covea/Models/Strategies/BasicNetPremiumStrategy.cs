namespace Covea.Application.Models.Strategies
{
    public class BasicNetPremiumStrategy : ICostingStrategy
    {
        readonly ICostingStrategy riskPremiumStrategy;
        readonly ICostingStrategy renewalCommissionStrategy;

        public BasicNetPremiumStrategy(ICostingStrategy riskPremiumStrategy, ICostingStrategy renewalCommissionStrategy)
        {
            this.riskPremiumStrategy = riskPremiumStrategy;
            this.renewalCommissionStrategy = renewalCommissionStrategy;
        }

        public double CalculateCost() => riskPremiumStrategy.CalculateCost() + renewalCommissionStrategy.CalculateCost();
    }
}

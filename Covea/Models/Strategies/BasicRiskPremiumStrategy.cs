namespace Covea.Application.Models.Strategies
{
    public class BasicRiskPremiumStrategy : ICostingStrategy
    {
        public readonly double riskRate;
        public readonly double sumAssured;

        public BasicRiskPremiumStrategy(double riskRate, double sumAssured)
        {
            this.riskRate = riskRate;
            this.sumAssured = sumAssured;
        }

        public double CalculateCost() => riskRate * (sumAssured / 1000);
    }
}

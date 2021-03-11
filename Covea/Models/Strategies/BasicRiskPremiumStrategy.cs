namespace Covea.Application.Models.Strategies
{
    public class BasicRiskPremiumStrategy : ICostingStrategy
    {
        readonly double riskRate;
        readonly double sumAssured;

        public BasicRiskPremiumStrategy(double riskRate, double sumAssured)
        {
            this.riskRate = riskRate;
            this.sumAssured = sumAssured;
        }

        public double CalculateCost() => riskRate + (sumAssured / 1000);
    }
}

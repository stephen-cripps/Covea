namespace Covea.Application.Models.Strategies
{
    /// <summary>
    /// Costing strategies allow for customisation of applicants through modular strategies
    /// </summary>
    public interface ICostingStrategy
    {
        double CalculateCost();
    }
}

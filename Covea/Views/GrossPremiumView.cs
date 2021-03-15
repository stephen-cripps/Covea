namespace Covea.Application.Views
{
    /// <summary>
    /// Defines the data to be returned from the GET GrossPremium endpoint
    /// </summary>
    public class GrossPremiumView
    {
        public int Age { get; set; }
        public int SumAssured { get; set; }
        public double Premium { get; set; }
    }
}

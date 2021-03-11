using Xunit;

namespace Covea.Testing.Subcutaneous_tests
{
    public class ApplicantControllerTests
    {
        [Theory]
        [InlineData(1,2,3)]
        public void GetPremium_ValidInputs_ReturnsPremiumData(int sumAssured, int age, double expectedResult)
        {
            //Arrange

            //Test

            //Assert
            // ExpectedResult== Actual Result
        }

        [Fact]
        public void getPremium_InvalidAge_Returns400()
        {
            //Arrange

            //Test

            //Assert
        }

        [Fact]
        public void GetPremium_InvalidSumAssured_Returns400()
        {
            //Arrange

            //Test

            //Assert
        }

        [Fact]
        public void GetPremium_StorageUnavailable_Returns503()
        {
            //Arrange

            //Test

            //Assert
        }
    }
}

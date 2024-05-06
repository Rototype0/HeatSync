namespace HeatItOn.Tests
{
    public class SourceDataManagerTests
    {
        [Fact]
        public void GetEnerginetAPIData_ReturnsContent()
        {
            // Arrange
            string url = "https://api.energidataservice.dk/dataset/Elspotprices?start=now-P6D&columns=HourDK%2CSpotPriceDKK&filter=%7B%22PriceArea%22%3A%5B%22DK1%22%5D%7D&sort=HourDK&limit=0";
            SourceDataManager SDM = new();

            // Act
            var content = SDM.ReadAPISourceData(url); // also checks if returns success code (200-299)

            // Assert
            Assert.NotNull(content);
        }
    }
}
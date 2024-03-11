namespace HeatItOn.Tests
{
    public class JsonAssetManagerTests
    {
        [Fact]
        public void LoadHeatingGridData_ValidFilePath_ReturnsHeatingGrid()
        {
            // Arrange
            JsonAssetManager jsonAssetManager = new();
            string filePath = "heatington.json";

            // Act
            HeatingGrid heatingGrid = jsonAssetManager.LoadHeatingGridData(filePath);

            // Assert
            Assert.Equal("heatington", heatingGrid.Name);
            Assert.Equal("StaticAssets\\ProductionUnit\\electricBoiler.jpg", heatingGrid.ImagePath);
        }

        [Fact]
        public void LoadProductionUnitData_ValidFilePath_ReturnsProductionUnit()
        {
            // Arrange
            JsonAssetManager jsonAssetManager = new();
            string filePath = "gasBoiler.json";

            // Act
            ProductionUnit productionUnit = jsonAssetManager.LoadProductionUnitData(filePath);

            // Assert
            Assert.Equal("GB", productionUnit.Name);
            Assert.Equal("StaticAssets\\ProductionUnit\\gasBoiler.jpg", productionUnit.ImagePath);
            Assert.Equal(100.0, productionUnit.MaxHeat);
            Assert.Equal(50.0, productionUnit.MaxElectricity);
            Assert.Equal(500, productionUnit.ProductionCosts);
            Assert.Equal(25.0, productionUnit.CO2Emissions);
            Assert.Equal(10.0, productionUnit.GasConsumption);
        }
    }
}
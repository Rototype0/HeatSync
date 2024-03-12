using Moq;

namespace HeatItOn.Tests
{
    public class JsonAssetManagerTests
    {
        [Fact]
        public void LoadHeatingGridData_ValidFilePath_ReturnsHeatingGrid()
        {
            // Arrange
            string filePath = "HeatingGridTest\\heatington.json";
            JsonAssetManager jsonAssetManager = new();
            HeatingGrid heatingGrid = new HeatingGrid();
            Mock<IAssetManager> JsonAssetManager = new Mock<IAssetManager>();
            JsonAssetManager.Setup(x=>x.LoadHeatingGridData(filePath)).Returns(heatingGrid);
            //string filePath = "heatington.json";

            // Act
            jsonAssetManager.LoadHeatingGridData(filePath);
            //HeatingGrid heatingGrid = jsonAssetManager.LoadHeatingGridData(filePath);

            // Assert
            Assert.Equal("heatington", heatingGrid.Name);
            Assert.Equal("HeatingGridTest\\heatington.jpg", heatingGrid.ImagePath);
        }
        /*
        [Fact]
        public void LoadProductionUnitData_ValidFilePath_ReturnsProductionUnit()
        {
            // Arrange
            JsonAssetManager jsonAssetManager = new();
            string filePath = "gasBoilerTest\\gasBoiler.json";

            // Act
            ProductionUnit productionUnit = jsonAssetManager.LoadProductionUnitData(filePath);

            // Assert
            Assert.Equal("GB", productionUnit.Name);
            Assert.Equal("gasBoilerTest\\gasBoiler.jpg", productionUnit.ImagePath);
            Assert.Equal(100.0, productionUnit.MaxHeat);
            Assert.Equal(50.0, productionUnit.MaxElectricity);
            Assert.Equal(500, productionUnit.ProductionCosts);
            Assert.Equal(25.0, productionUnit.CO2Emissions);
            Assert.Equal(10.0, productionUnit.GasConsumption);
        }*/
    }
}
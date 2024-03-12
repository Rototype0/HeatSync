namespace HeatItOn.Tests
{
    public class JsonAssetManagerTests
    {
        [Fact]
        public void LoadHeatingGridData_ReturnsHeatingGrid()
        {
            // Arrange
            string testString = @"{""Name"":""TestHeatingGrid"",""ImagePath"":""test_image.jpg""}";
            JsonAssetManager jsonAssetManager = new();

            // Act
            HeatingGrid heatingGrid = jsonAssetManager.LoadHeatingGridData(testString);

            // Assert
            Assert.Equal("TestHeatingGrid", heatingGrid.Name);
            Assert.Equal("test_image.jpg", heatingGrid.ImagePath);
        }
        
        [Fact]
        public void LoadProductionUnitData_ValidFilePath_ReturnsProductionUnit()
        {
            // Arrange
            string testString = @"{""Name"":""TestProductionUnit"",""ImagePath"":""test_image.jpg"",""MaxHeat"":100.0,""MaxElectricity"":50.0,""ProductionCosts"":500,""CO2Emissions"":25.0,""GasConsumption"":10.0}";
            JsonAssetManager jsonAssetManager = new();

            // Act
            ProductionUnit productionUnit = jsonAssetManager.LoadProductionUnitData(testString);

            // Assert
            Assert.Equal("TestProductionUnit", productionUnit.Name);
            Assert.Equal("test_image.jpg", productionUnit.ImagePath);
            Assert.Equal(100.0, productionUnit.MaxHeat);
            Assert.Equal(50.0, productionUnit.MaxElectricity);
            Assert.Equal(500, productionUnit.ProductionCosts);
            Assert.Equal(25.0, productionUnit.CO2Emissions);
            Assert.Equal(10.0, productionUnit.GasConsumption);
        }
    }
}
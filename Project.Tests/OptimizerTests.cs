namespace HeatItOn.Tests
{
    // Testing each method for calculating production costs from Optmizer class 

    public class OptimizerTests
    {
        // Kinda of an useless test, because in the method, it's just value copying, but I'll leave it here :))
        [Fact]
        public void CalculateNetProdCostHeatOnly_CorrectGBNetProductionCost()
        {
            // Arrange
            string testString = @"{""Name"":""GB"",""ImagePath"":""StaticAssets\\ProductionUnit\\gasBoiler.jpg"",""MaxHeat"":5.0,""MaxElectricity"":0.0,""ProductionCosts"":500,""CO2Emissions"":215,""GasConsumption"":1.1}";
            var optimizer = new Optimizer();
            JsonAssetManager jsonAssetManager = new();
            ProductionUnit gasBoiler = jsonAssetManager.LoadProductionUnitData(testString);

            // Act 
            optimizer.CalculateNetProdCostHeatOnly(gasBoiler);

            // Assert
            Assert.Equal(500,gasBoiler.ProductionCosts);
        }
        [Fact]
        public void CalculateNetProdCostGasMotor_CorrectGMNetProductionCost()
        {
            // Arrange
            string testString = @"{""Name"":""GM"",""ImagePath"":""StaticAssets\\ProductionUnit\\gasMotor.jpg"",""MaxHeat"":3.6,""MaxElectricity"":2.7,""ProductionCosts"":1100,""CO2Emissions"":640,""GasConsumption"":1.9}";
            JsonAssetManager jsonAssetManager = new();
            var optimizer = new Optimizer();
            // SDM sdm = new SDM(csvFilePath,100);
            ProductionUnit gasMotor = jsonAssetManager.LoadProductionUnitData(testString);

            // Act 
            optimizer.CalculateNetProdCostGasMotor(gasMotor);

            // Assert
            Assert.Equal(500,gasMotor.ProductionCosts);

        }
        [Fact]
        public void CalculateNetProdCostElectricBoiler_CorrectEBNetProductionCost()
        {
            // Arrange
            string testString = @"{""Name"":""GM"",""ImagePath"":""StaticAssets\\ProductionUnit\\electricBoiler.jpg"",""MaxHeat"":8.0,""MaxElectricity"":-8.0,""ProductionCosts"":50,""CO2Emissions"":0,""GasConsumption"":0.0}";
            JsonAssetManager jsonAssetManager = new();
            var optimizer = new Optimizer();
            // SDM sdm = new SDM(csvFilePath,100);
            ProductionUnit electricBoiler = jsonAssetManager.LoadProductionUnitData(testString);

            // Act 
            optimizer.CalculateNetProdCostElectricBoiler(electricBoiler);

            // Assert
            Assert.Equal(500,electricBoiler.ProductionCosts);

        }
    }
}
namespace HeatItOn.Tests
{
    public class OptimizerTests
    {
        private ProductionUnit gasMotorUnit;
        private ProductionUnit electricBoilerUnit;
        private SDMData sourceDataPoint;
        public Optimizer optimizer = new();

        public OptimizerTests()
        {
            gasMotorUnit = new ProductionUnit
            {
                ProductionCosts = 1000,
                MaxElectricity = 50
            };

            electricBoilerUnit = new ProductionUnit
            {
                ProductionCosts = 1200,
                MaxElectricity = -30
            };

            sourceDataPoint = new SDMData
            {
                ElectricityPrice = 0.1
            };
        }

        [Fact]
        public void CalculateNetProductionCost_GasMotorUnit_CorrectCalculation()
        {
            double expectedNetCost = 1000 - (50 * 0.1);
            double actualNetCost = optimizer.CalculateNetProductionCost(gasMotorUnit, sourceDataPoint);
            Assert.Equal(expectedNetCost, actualNetCost);
        }

        [Fact]
        public void CalculateNetProductionCost_ElectricBoilerUnit_CorrectCalculation()
        {
            double expectedNetCost = 1200 - (-30 * 0.1);
            double actualNetCost = optimizer.CalculateNetProductionCost(electricBoilerUnit, sourceDataPoint);
            Assert.Equal(expectedNetCost, actualNetCost);
        }

        [Fact]
        public void GetLowestNetProductionCostUnit_ReturnsLowestCostUnit()
        {
            List<ProductionUnit> productionUnits = [gasMotorUnit, electricBoilerUnit];
            ProductionUnit expectedUnit = gasMotorUnit; // Assuming gasMotorUnit has the lowest cost
            ProductionUnit actualUnit = optimizer.GetLowestNetProductionCostUnit(productionUnits, sourceDataPoint);
            Assert.Equal(expectedUnit, actualUnit);
        }

        
        // Kinda of an useless test, because in the method, it's just value copying, but I'll leave it here :))
        /*[Fact]
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

        }*/
    }
}
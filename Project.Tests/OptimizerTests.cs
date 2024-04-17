namespace HeatItOn.Tests
{
    public class OptimizerTests
    {
        private readonly Optimizer optimizer;
        private readonly SourceData sourceDataPoint;
        private readonly List<ProductionUnit> productionUnits;

        public OptimizerTests()
        {
            optimizer = new();

            productionUnits =
            [
                new ProductionUnit
                {
                    Name = "Boiler 1",
                    ProductionCosts = 1000,
                    MaxElectricity = 20,
                    MaxHeat = 50,
                    CO2Emissions = 50,
                    GasConsumption = 100
                },
                new ProductionUnit
                {
                    Name = "Boiler 2",
                    ProductionCosts = 1200,
                    MaxElectricity = 30,
                    MaxHeat = 70,
                    CO2Emissions = 60,
                    GasConsumption = 120
                }
            ];

            sourceDataPoint = new SourceData
            {
                TimeFrom = DateTime.Now,
                TimeTo = DateTime.Now.AddHours(1),
                HeatDemand = 80,
                ElectricityPrice = 0.1
            };
        }

        [Fact]
        public void CalculateNetProductionCost_ReturnsCorrectValue()
        {
            double expectedNetCost = 1000 - (20 * 0.1);
            double actualNetCost = optimizer.CalculateNetProductionCost(productionUnits[0], sourceDataPoint);
            Assert.Equal(expectedNetCost, actualNetCost);
        }

        [Fact]
        public void GetLowestNetProductionCostUnit_ReturnsLowestCostUnit()
        {
            ProductionUnit expectedUnit = productionUnits[0];
            ProductionUnit actualUnit = optimizer.GetLowestNetProductionCostUnit(productionUnits, sourceDataPoint);
            Assert.Equal(expectedUnit, actualUnit);
        }

        [Fact]
        public void ChangeOperationalPercentage_ChangesPercentageCorrectly()
        {
            ResultData originalData = new()
            {
                ProducedHeat = 50,
                NetElectricity = 20,
                ProducedCO2 = 30,
                PrimaryEnergyConsumption = 60,
                ProductionCosts = 1000,
                OperationPercentage = 1
            };

            double newPercentage = 0.5;
            ResultData updatedData = optimizer.ChangeOperationalPercentage(originalData, newPercentage, 2);

            Assert.Equal(newPercentage, updatedData.OperationPercentage);
            Assert.Equal(originalData.ProducedHeat * newPercentage, updatedData.ProducedHeat);
            Assert.Equal(originalData.NetElectricity * newPercentage, updatedData.NetElectricity);
            Assert.Equal(originalData.ProducedCO2 * newPercentage, updatedData.ProducedCO2);
            Assert.Equal(originalData.PrimaryEnergyConsumption * newPercentage, updatedData.PrimaryEnergyConsumption);
            Assert.Equal(originalData.ProductionCosts * newPercentage, updatedData.ProductionCosts);
        }

        [Fact]
        public void OptimizeData_ReturnsNonEmptyList()
        {
            List<ResultData> resultDatas = optimizer.OptimizeData(productionUnits, [sourceDataPoint]);
            Assert.NotEmpty(resultDatas);
        }

        [Fact]
        public void OptimizeData_ReturnsOptimizedList()
        {
            List<SourceData> sourceData =
            [
                new SourceData
                {
                    TimeFrom = DateTime.Now,
                    TimeTo = DateTime.Now.AddHours(1),
                    HeatDemand = 80,
                    ElectricityPrice = 0.1
                },
                new SourceData
                {
                    TimeFrom = DateTime.Now.AddHours(1),
                    TimeTo = DateTime.Now.AddHours(2),
                    HeatDemand = 90,
                    ElectricityPrice = 0.12
                }
            ];

            // Expected result data
            List<ResultData> expectedResults =
            [
                new ResultData
                {
                    TimeFrom = sourceData[0].TimeFrom,
                    TimeTo = sourceData[0].TimeTo,
                    ProductionUnitName = "Boiler 1",
                    ProducedHeat = 50,
                    NetElectricity = 20,
                    ProductionCosts = 1000,
                    ProducedCO2 = 50,
                    PrimaryEnergyConsumption = 100,
                    OperationPercentage = 1
                },
                new ResultData
                {
                    TimeFrom = sourceData[0].TimeFrom,
                    TimeTo = sourceData[0].TimeTo,
                    ProductionUnitName = "Boiler 2",
                    ProducedHeat = 30,
                    NetElectricity = 12.86,
                    ProductionCosts = 514.29,
                    ProducedCO2 = 25.71,
                    PrimaryEnergyConsumption = 51.43,
                    OperationPercentage = 0.43
                },
                new ResultData
                {
                    TimeFrom = sourceData[1].TimeFrom,
                    TimeTo = sourceData[1].TimeTo,
                    ProductionUnitName = "Boiler 1",
                    ProducedHeat = 50,
                    NetElectricity = 20,
                    ProductionCosts = 1000,
                    ProducedCO2 = 50,
                    PrimaryEnergyConsumption = 100,
                    OperationPercentage = 1
                },
                new ResultData
                {
                    TimeFrom = sourceData[1].TimeFrom,
                    TimeTo = sourceData[1].TimeTo,
                    ProductionUnitName = "Boiler 2",
                    ProducedHeat = 40,
                    NetElectricity = 17.14,
                    ProductionCosts = 685.71,
                    ProducedCO2 = 34.29,
                    PrimaryEnergyConsumption = 68.57,
                    OperationPercentage = 0.57
                }
            ];

            // Get actual results
            List<ResultData> actualResults = optimizer.OptimizeData(productionUnits, sourceData, 2);

            // Compare actual and expected results
            Assert.Equal(expectedResults, actualResults);
        }
    }
}
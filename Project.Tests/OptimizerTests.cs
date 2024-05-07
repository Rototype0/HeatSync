namespace HeatSync.Tests
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
            ResultData updatedData = optimizer.ChangeOperationalPercentage(originalData, newPercentage);

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
                    NetElectricity = 12.857142857142856,
                    ProductionCosts = 514.2857142857142,
                    ProducedCO2 = 25.71428571428571,
                    PrimaryEnergyConsumption = 51.42857142857142,
                    OperationPercentage = 0.42857142857142855
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
                    NetElectricity = 17.142857142857142,
                    ProductionCosts = 685.7142857142857,
                    ProducedCO2 = 34.285714285714285,
                    PrimaryEnergyConsumption = 68.57142857142857,
                    OperationPercentage = 0.5714285714285714
                }
            ];

            // Get actual results
            List<ResultData> actualResults = optimizer.OptimizeData(productionUnits, sourceData);

            // Compare actual and expected results
            Assert.Equal(expectedResults, actualResults);
        }
    }
}
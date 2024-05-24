namespace HeatSync.Tests
{
    public class DataVisualizerTests
    {
        [Fact]
        public void SeparateResultDataListByProductionUnit_ReturnsCorrectList()
        {
            DataVisualizer dataVisualizer = new();
            // Arrange
            List<ResultData> resultDataList = [
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T15:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T16:00:00"), 
                    ProductionUnitName = "GB",
                    ProducedHeat = 5,
                    NetElectricity = 0,
                    ProductionCosts = 500,
                    ProducedCO2 = 215,
                    PrimaryEnergyConsumption = 1.1,
                    OperationPercentage = 1,
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T15:00:00"),
                    TimeTo =  DateTime.Parse("2024-04-28T16:00:00"), 
                    ProductionUnitName = "GM",
                    ProducedHeat = 2.262432699679598,
                    NetElectricity = 1.6968245247596985,
                    ProductionCosts = 691.2988804576548,
                    ProducedCO2 = 402.21025772081737,
                    PrimaryEnergyConsumption = 1.1940617026086766,
                    OperationPercentage = 0.6284535276887772,
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T16:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T17:00:00"), 
                    ProductionUnitName = "OB",
                    ProducedHeat = 0.460270660119258,
                    NetElectricity = 0,
                    ProductionCosts = 80.54736552087014,
                    ProducedCO2 = 30.1477282378114,
                    PrimaryEnergyConsumption = 0.1380811980357774,
                    OperationPercentage = 0.1150676650298145,
                },
                new()
                {
                    TimeFrom =  DateTime.Parse("2024-04-28T17:00:00"),
                    TimeTo =  DateTime.Parse("2024-04-28T18:00:00"), 
                    ProductionUnitName = "EK",
                    ProducedHeat = 4.82151253140602,
                    NetElectricity = -4.82151253140602,
                    ProductionCosts = 30.134453321287623,
                    ProducedCO2 = 0,
                    PrimaryEnergyConsumption = 0,
                    OperationPercentage = 0.6026890664257525,
                },
                new()
                {
                    TimeFrom =  DateTime.Parse("2024-04-28T18:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T19:00:00"), 
                    ProductionUnitName = "GB",
                    ProducedHeat = 4.573253595909687,
                    NetElectricity = 0,
                    ProductionCosts = 457.32535959096873,
                    ProducedCO2 = 196.64990462411654,
                    PrimaryEnergyConsumption = 1.0061157911001313,
                    OperationPercentage = 0.9146507191819374,
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T19:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T20:00:00"), 
                    ProductionUnitName = "GM",
                    ProducedHeat = 3.6,
                    NetElectricity = 2.7,
                    ProductionCosts = 1100,
                    ProducedCO2 = 640,
                    PrimaryEnergyConsumption = 1.9,
                    OperationPercentage = 1,
                },
            ];

            List<List<ResultData>> expectedSortedList = [
                // First List (GB)
                [
                    new()
                    {
                        TimeFrom = DateTime.Parse("2024-04-28T15:00:00"),
                        TimeTo = DateTime.Parse("2024-04-28T16:00:00"), 
                        ProductionUnitName = "GB",
                        ProducedHeat = 5,
                        NetElectricity = 0,
                        ProductionCosts = 500,
                        ProducedCO2 = 215,
                        PrimaryEnergyConsumption = 1.1,
                        OperationPercentage = 1,
                    },
                    new()
                    {
                        TimeFrom =  DateTime.Parse("2024-04-28T18:00:00"),
                        TimeTo = DateTime.Parse("2024-04-28T19:00:00"), 
                        ProductionUnitName = "GB",
                        ProducedHeat = 4.573253595909687,
                        NetElectricity = 0,
                        ProductionCosts = 457.32535959096873,
                        ProducedCO2 = 196.64990462411654,
                        PrimaryEnergyConsumption = 1.0061157911001313,
                        OperationPercentage = 0.9146507191819374,
                    },
                ],
                // Second list (GM)
                [
                    new()
                    {
                        TimeFrom = DateTime.Parse("2024-04-28T15:00:00"),
                        TimeTo =  DateTime.Parse("2024-04-28T16:00:00"), 
                        ProductionUnitName = "GM",
                        ProducedHeat = 2.262432699679598,
                        NetElectricity = 1.6968245247596985,
                        ProductionCosts = 691.2988804576548,
                        ProducedCO2 = 402.21025772081737,
                        PrimaryEnergyConsumption = 1.1940617026086766,
                        OperationPercentage = 0.6284535276887772,
                    },
                    new()
                    {
                        TimeFrom = DateTime.Parse("2024-04-28T19:00:00"),
                        TimeTo = DateTime.Parse("2024-04-28T20:00:00"), 
                        ProductionUnitName = "GM",
                        ProducedHeat = 3.6,
                        NetElectricity = 2.7,
                        ProductionCosts = 1100,
                        ProducedCO2 = 640,
                        PrimaryEnergyConsumption = 1.9,
                        OperationPercentage = 1,
                    },
                ],
                // Third List (OB)
                [
                    new()
                    {
                        TimeFrom = DateTime.Parse("2024-04-28T16:00:00"),
                        TimeTo = DateTime.Parse("2024-04-28T17:00:00"), 
                        ProductionUnitName = "OB",
                        ProducedHeat = 0.460270660119258,
                        NetElectricity = 0,
                        ProductionCosts = 80.54736552087014,
                        ProducedCO2 = 30.1477282378114,
                        PrimaryEnergyConsumption = 0.1380811980357774,
                        OperationPercentage = 0.1150676650298145,
                    },
                ],
                // Fourth List (EK)
                [
                    new()
                    {
                        TimeFrom =  DateTime.Parse("2024-04-28T17:00:00"),
                        TimeTo =  DateTime.Parse("2024-04-28T18:00:00"), 
                        ProductionUnitName = "EK",
                        ProducedHeat = 4.82151253140602,
                        NetElectricity = -4.82151253140602,
                        ProductionCosts = 30.134453321287623,
                        ProducedCO2 = 0,
                        PrimaryEnergyConsumption = 0,
                        OperationPercentage = 0.6026890664257525,
                    }
                ],
            ];
            // Act

            List<List<ResultData>> sortedList = dataVisualizer.SeparateResultDataListByProductionUnit(resultDataList);

            // Assert

            Assert.Equal(expectedSortedList,sortedList);
        }
    }
}
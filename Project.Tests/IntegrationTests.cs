namespace HeatItOn.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async void GetAndOptimizeAPIData_EqualsExpectedData()
        {
            SourceDataManager sourceDataManager = new();
            Optimizer optimizer = new();
            
            string url = "https://api.energidataservice.dk/dataset/Elspotprices?offset=6&start=2024-04-28T00:00&end=2024-04-29T00:00&columns=HourDK,SpotPriceDKK&filter={%22PriceArea%22:[%22DK1%22]}&sort=HourDK&limit=6";
            List<SourceData> expectedSourceData = [
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T06:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T07:00:00"),
                    HeatDemand = 7.262432699679598,
                    ElectricityPrice = 74.209999
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T07:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T08:00:00"),
                    HeatDemand = 8.173253595909687,
                    ElectricityPrice = 1.86
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T08:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T09:00:00"),
                    HeatDemand = 7.680226893946634,
                    ElectricityPrice = 1.94
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T09:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T10:00:00"),
                    HeatDemand = 5.5816119143653715,
                    ElectricityPrice = 1.64
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T10:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T11:00:00"),
                    HeatDemand = 2.060331540210327,
                    ElectricityPrice = 0
                },
                new()
                {
                    TimeFrom = DateTime.Parse("2024-04-28T11:00:00"),
                    TimeTo = DateTime.Parse("2024-04-28T12:00:00"),
                    HeatDemand = 5.588847946184151,
                    ElectricityPrice = -7.16
                },
            ];

            List<ResultData> expectedResultData =
            [
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 06:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 07:00:00"),
                    ProductionUnitName = "GB",
                    ProducedHeat = 5,
                    NetElectricity = 0,
                    ProductionCosts = 500,
                    ProducedCO2 = 215,
                    PrimaryEnergyConsumption = 1.1,
                    OperationPercentage = 1
                },
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 06:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 07:00:00"),
                    ProductionUnitName = "EK",
                    ProducedHeat = 2.262432699679598,
                    NetElectricity = -2.262432699679598,
                    ProductionCosts = 14.140204372997486,
                    ProducedCO2 = 0,
                    PrimaryEnergyConsumption = 0,
                    OperationPercentage = 0.2828040874599497
                },
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 07:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 08:00:00"),
                    ProductionUnitName = "EK",
                    ProducedHeat = 8,
                    NetElectricity = -8,
                    ProductionCosts = 50,
                    ProducedCO2 = 0,
                    PrimaryEnergyConsumption = 0,
                    OperationPercentage = 1
                },
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 07:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 08:00:00"),
                    ProductionUnitName = "GB",
                    ProducedHeat = 0.173253595909687,
                    NetElectricity = 0,
                    ProductionCosts = 17.3253595909687,
                    ProducedCO2 = 7.449904624116541,
                    PrimaryEnergyConsumption = 0.03811579110013114,
                    OperationPercentage = 0.0346507191819374
                },
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 08:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 09:00:00"),
                    ProductionUnitName = "EK",
                    ProducedHeat = 7.680226893946634,
                    NetElectricity = -7.680226893946634,
                    ProductionCosts = 48.00141808716646,
                    ProducedCO2 = 0,
                    PrimaryEnergyConsumption = 0,
                    OperationPercentage = 0.9600283617433293
                },
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 09:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 10:00:00"),
                    ProductionUnitName = "EK",
                    ProducedHeat = 5.5816119143653715,
                    NetElectricity = -5.5816119143653715,
                    ProductionCosts = 34.88507446478357,
                    ProducedCO2 = 0,
                    PrimaryEnergyConsumption = 0,
                    OperationPercentage = 0.6977014892956714
                },
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 10:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 11:00:00"),
                    ProductionUnitName = "EK",
                    ProducedHeat = 2.060331540210327,
                    NetElectricity = -2.060331540210327,
                    ProductionCosts = 12.877072126314543,
                    ProducedCO2 = 0,
                    PrimaryEnergyConsumption = 0,
                    OperationPercentage = 0.25754144252629085
                },
                new()
                {
                    TimeFrom = DateTime.Parse("04/28/2024 11:00:00"),
                    TimeTo = DateTime.Parse("04/28/2024 12:00:00"),
                    ProductionUnitName = "EK",
                    ProducedHeat = 5.588847946184151,
                    NetElectricity = -5.588847946184151,
                    ProductionCosts = 34.93029966365094,
                    ProducedCO2 = 0,
                    PrimaryEnergyConsumption = 0,
                    OperationPercentage = 0.6986059932730189
                }
            ];

            List<ProductionUnit> productionUnits =
            [
                new()
                {
                    Name = "GB",
                    ImagePath = "StaticAssets\\ProductionUnit\\gasBoiler.jpg",
                    MaxHeat = 5.0,
                    MaxElectricity = 0.0,
                    ProductionCosts = 500,
                    CO2Emissions = 215,
                    GasConsumption = 1.1
                },
                new()
                {
                    Name = "OB",
                    ImagePath = "StaticAssets\\ProductionUnit\\oilBoiler.jpg",
                    MaxHeat = 4.0,
                    MaxElectricity = 0,
                    ProductionCosts = 700,
                    CO2Emissions = 262,
                    GasConsumption = 1.2
                },
                new()
                {
                    Name = "GM",
                    ImagePath = "StaticAssets\\ProductionUnit\\gasMotor.jpg",
                    MaxHeat = 3.6,
                    MaxElectricity = 2.7,
                    ProductionCosts = 1100,
                    CO2Emissions = 640,
                    GasConsumption = 1.9
                },
                new()
                {
                    Name = "EK",
                    ImagePath = "StaticAssets\\ProductionUnit\\electricBoiler.jpg",
                    MaxHeat = 8.0,
                    MaxElectricity = -8.0,
                    ProductionCosts = 50,
                    CO2Emissions = 0,
                    GasConsumption = 0
                }
            ];

            List<SourceData> sourceData = await sourceDataManager.ReadAPISourceData(url);
            List<ResultData> resultData = optimizer.OptimizeData(productionUnits, sourceData);

            Assert.Equal(sourceData, expectedSourceData);
            Assert.Equal(resultData, expectedResultData);
        }
    }
}
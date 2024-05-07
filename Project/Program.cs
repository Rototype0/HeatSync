namespace HeatSync
{
    class Program
    {
        private static void Main(string[] args)
        {
            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new();
            ResultDataManager resultDataManager = new();
            Optimizer optimizer = new();

            List<SourceData> data = sourceDataManager.ReadAPISourceData().Result;

            // string sourceDataPath = "wintertest"; // can be "summertest" or "wintertest"
            // List<SourceData> data = sourceDataManager.ReadSourceData(sourceDataPath);
            
            // foreach (SourceData dataPoint in data)
            // {
            //     Console.WriteLine(dataPoint.TimeFrom);
            //     Console.WriteLine(dataPoint.TimeTo);
            //     Console.WriteLine(dataPoint.HeatDemand);
            //     Console.WriteLine(dataPoint.ElectricityPrice);
            // }
            
            List<ProductionUnit> productionUnits = jsonAssetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, data);

            string fileName = "ResultDataTest";
            resultDataManager.WriteResultData(writeRecords, fileName);

            // var readRecords = resultDataManager.ReadResultData(fileName);
            // Console.WriteLine(readRecords[0].ProductionUnitName);
            // Console.WriteLine(readRecords[0].ProducedHeat);
            // Console.WriteLine(readRecords[0].NetElectricity);
            // Console.WriteLine(readRecords[0].ProductionCosts);
            // Console.WriteLine(readRecords[0].ProducedCO2);
            // Console.WriteLine(readRecords[0].PrimaryEnergyConsumption);
            // Console.WriteLine(readRecords[0].OperationPercentage);
        }
    }
}

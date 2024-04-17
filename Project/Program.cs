namespace HeatItOn
{
    class Program
    {
        private static void Main(string[] args)
        {
            string sourceDataPath = "wintertest"; // can be "summertest" or "wintertest"

            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new(sourceDataPath, 1000); // temp
            ResultDataManager resultDataManager = new();
            Optimizer optimizer = new();

            List<ProductionUnit> productionUnits = jsonAssetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, sourceDataManager.ReadSourceData(sourceDataPath));

            Console.WriteLine("Test writing result data");
            string fileName = "ResultDataTest";
            resultDataManager.WriteResultData(writeRecords, fileName);

            Console.WriteLine("Test reading result data");
            var readRecords = resultDataManager.ReadResultData(fileName);

            Console.WriteLine(readRecords[0].ProductionUnitName);
            Console.WriteLine(readRecords[0].ProducedHeat);
            Console.WriteLine(readRecords[0].NetElectricity);
            Console.WriteLine(readRecords[0].ProductionCosts);
            Console.WriteLine(readRecords[0].ProducedCO2);
            Console.WriteLine(readRecords[0].PrimaryEnergyConsumption);
            Console.WriteLine(readRecords[0].OperationPercentage);
        }
    }
}

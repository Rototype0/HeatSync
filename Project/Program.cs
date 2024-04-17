namespace HeatItOn
{
    class Program
    {
        private static void Main(string[] args)
        {
            string sourceDataPath = "wintertest"; // can be "summertest" or "wintertest"

            List<ProductionUnit> productionUnits = [];
            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new(sourceDataPath, 1000); // temp
            ResultDataManager resultDataManager = new();
            Optimizer optimizer = new();
            
            // comment out whichever one you want to remove from the list that optimizer uses
            string gasBoilerPath = "StaticAssets\\ProductionUnits\\gasBoiler.json";
            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(gasBoilerPath)));

            string oilBoilerPath = "StaticAssets\\ProductionUnits\\oilBoiler.json";
            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(oilBoilerPath)));

            string gasMotorPath = "StaticAssets\\ProductionUnits\\gasMotor.json";
            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(gasMotorPath)));

            string electricBoilerPath = "StaticAssets\\ProductionUnits\\electricBoiler.json";
            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(electricBoilerPath)));
            
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

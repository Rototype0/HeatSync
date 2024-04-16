namespace HeatItOn
{
    class Program
    {
        private static void Main(string[] args)
        {
            string sourceDataPath = "StaticAssets\\ProductionUnits\\electricBoiler.json"; // temp

            string gasBoilerPath = "StaticAssets\\ProductionUnits\\gasBoiler.json";
            string oilBoilerPath = "StaticAssets\\ProductionUnits\\oilBoiler.json";
            string gasMotorPath = "StaticAssets\\ProductionUnits\\gasMotor.json";
            string electricBoilerPath = "StaticAssets\\ProductionUnits\\electricBoiler.json";

            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new(sourceDataPath, 1000); // temp
            ResultDataManager resultDataManager = new();
            Optimizer optimizer = new();

            List<ProductionUnit> productionUnits = [];

            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(gasBoilerPath)));
            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(oilBoilerPath)));
            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(gasMotorPath)));
            productionUnits.Add(jsonAssetManager.LoadProductionUnitData(File.ReadAllText(electricBoilerPath)));
            
            /*var writeRecords = new List<ResultData>
            {
                new() { ProductionUnitName = "Name", ProducedHeat = 0.25, NetElectricity = 0.5, ProductionCosts = 75, ProducedCO2 = 75.8, PrimaryEnergyConsumption = 4.5678 },
                new() { ProductionUnitName = "Name2", ProducedHeat = 0.35, NetElectricity = 0.45, ProductionCosts = 775, ProducedCO2 = 45.8, PrimaryEnergyConsumption = 1.23 },
            };*/
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, sourceDataManager.ReadSourceData(sourceDataPath));

            string fileName = "ResultDataTest";
            resultDataManager.WriteResultData(writeRecords, fileName);

            var readRecords = resultDataManager.ReadResultData(fileName);

            foreach (var item in readRecords)
            {
                Console.WriteLine(item.ProductionUnitName);
                Console.WriteLine(item.ProducedHeat);
                Console.WriteLine(item.NetElectricity);
                Console.WriteLine(item.ProductionCosts);
                Console.WriteLine(item.ProducedCO2);
                Console.WriteLine(item.PrimaryEnergyConsumption);
                Console.WriteLine(item.OperationPercentage);
            }
        }
    }
}

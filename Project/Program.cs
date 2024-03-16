namespace HeatItOn
{
    class Program
    {
        private static void Main(string[] args)
        {
            // JsonAssetManager jsonAssetManager = new();

            // string testFilePath = "StaticAssets\\ProductionUnits\\oilBoiler.json";

            // ProductionUnit testUnit = jsonAssetManager.LoadProductionUnitData(File.ReadAllText(testFilePath));
            // Console.WriteLine("Name: " + testUnit.Name);
            // Console.WriteLine("Image: " + testUnit.ImagePath);
            // Console.WriteLine("MaxHeat: " + testUnit.MaxHeat);
            // Console.WriteLine("MaxElectricity: " + testUnit.MaxElectricity);
            // Console.WriteLine("ProductionCosts: " + testUnit.ProductionCosts);
            // Console.WriteLine("CO2Emissions: " + testUnit.CO2Emissions);
            // Console.WriteLine("GasConsumption: " + testUnit.GasConsumption);

            ResultDataManager resultDataManager = new();
            
            var writeRecords = new List<ResultData>
            {
                new() { ProductionUnitName = "Name", ProducedHeat = 0.25, NetElectricity = 0.5, ProductionCosts = 75, ProducedCO2 = 75.8, PrimaryEnergyConsumption = 4.5678 },
                new() { ProductionUnitName = "Name2", ProducedHeat = 0.35, NetElectricity = 0.45, ProductionCosts = 775, ProducedCO2 = 45.8, PrimaryEnergyConsumption = 1.23 },
            };

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
            }
        }
    }
}

namespace HeatItOn
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            JsonAssetManager jsonAssetManager = new();

            string testFilePath = "StaticAssets\\ProductionUnits\\oilBoiler.json";

            ProductionUnit testUnit = jsonAssetManager.LoadProductionUnitData(File.ReadAllText(testFilePath));
            Console.WriteLine("Name: " + testUnit.Name);
            Console.WriteLine("Image: " + testUnit.ImagePath);
            Console.WriteLine("MaxHeat: " + testUnit.MaxHeat);
            Console.WriteLine("MaxElectricity: " + testUnit.MaxElectricity);
            Console.WriteLine("ProductionCosts: " + testUnit.ProductionCosts);
            Console.WriteLine("CO2Emissions: " + testUnit.CO2Emissions);
            Console.WriteLine("GasConsumption: " + testUnit.GasConsumption);
        }
    }
}

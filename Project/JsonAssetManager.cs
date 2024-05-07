using System.Text.Json;

namespace HeatSync
{
    public class JsonAssetManager : IAssetManager
    {
        private static TValue? LoadJsonData<TValue>(string data)
        {
            try
            {
                TValue jsonData = JsonSerializer.Deserialize<TValue>(data)!;
                return jsonData;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }
        
        public HeatingGrid LoadHeatingGridData(string data)
        {
            return LoadJsonData<HeatingGrid>(data);
        }
        public ProductionUnit LoadProductionUnitData(string data)
        {
            return LoadJsonData<ProductionUnit>(data);
        }

        // Helper method for getting all valid production units quickly.
        public List<ProductionUnit> GetAllProductionUnits()
        {
            List<ProductionUnit> productionUnits = [];

            string[] prodUnitPaths = Directory.GetFiles("StaticAssets\\ProductionUnits", "*.json");
            foreach (string prodUnitPath in prodUnitPaths)
                productionUnits.Add(LoadProductionUnitData(File.ReadAllText(prodUnitPath)));
            
            return productionUnits;
        }
    }
}

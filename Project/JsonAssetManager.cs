using System.Text.Json;

namespace HeatItOn
{
    public class JsonAssetManager : IAssetManager
    {
        private static TValue? LoadJsonData<TValue>(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                TValue jsonData = JsonSerializer.Deserialize<TValue>(jsonString)!;

                return jsonData;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }
        
        public HeatingGrid LoadHeatingGridData(string filePath)
        {
            return LoadJsonData<HeatingGrid>(filePath);
        }
        public ProductionUnit LoadProductionUnitData(string filePath)
        {
            return LoadJsonData<ProductionUnit>(filePath);
        }
    }
}

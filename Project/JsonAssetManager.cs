using System.Text.Json;

namespace HeatItOn
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
    }
}

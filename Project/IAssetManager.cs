namespace HeatItOn
{
    public interface IAssetManager
    {
        public HeatingGrid LoadHeatingGridData(string filePath);
        public ProductionUnit LoadProductionUnitData(string filePath);
        public string ReadAllData(string filePath);
    }
}
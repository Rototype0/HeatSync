namespace HeatSync
{
    public interface IDataVisualizer
    {
        public void UpdateData(int WindowWidth, int WindowHeight, List<SourceData> Data, List<ProductionUnit> ProductionUnits, List<ResultData> WriteRecords);
        public void Render();
    }
}
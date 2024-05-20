namespace HeatSync
{
    public interface IDataVisualizer
    {
        public void UpdateData(List<SourceData> Data, List<ProductionUnit> ProductionUnits, List<ResultData> WriteRecords);
        public void Render();
    }
}
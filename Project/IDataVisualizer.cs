namespace HeatSync
{
    public interface IDataVisualizer
    {
        public void UpdateData(List<SourceData> Data, HeatingGrid GridData, List<ProductionUnit> ProductionUnits, List<ResultData> WriteRecords);
        public void Render();
    }
}
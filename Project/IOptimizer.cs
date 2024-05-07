namespace HeatSync
{
    public interface IOptimizer
    {
        public List<ResultData> OptimizeData(List<ProductionUnit> productionUnits, List<SourceData> sourceData);
    }
}

namespace HeatItOn
{
    public interface IOptimizer
    {
        public List<ResultData> OptimizeData(List<ProductionUnit> productionUnits, List<SourceData> sourceData, int roundToDigits = 2);
    }
}

namespace HeatItOn
{
    public interface IOptimizer
    {
        public List<ResultData> OptimizeData(List<ProductionUnit> productionUnits, List<SourceData> sourceData);
        //
        public void CalculateNetProdCostHeatOnly(ProductionUnit productionUnit);
        public void CalculateNetProdCostGasMotor(ProductionUnit productionUnit);
        public void CalculateNetProdCostElectricBoiler(ProductionUnit productionUnit);
        public void CompareNetProdCosts();
    }
}

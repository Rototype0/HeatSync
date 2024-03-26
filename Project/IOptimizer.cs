namespace HeatItOn
{
    public interface IOptimizer
    {
        public void CalculateNetProdCostHeatOnly(ProductionUnit productionUnit);
        public void CalculateNetProdCostGasMotor(ProductionUnit productionUnit);
        public void CalculateNetProdCostElectricBoiler(ProductionUnit productionUnit);
        public void CompareNetProdCosts();
    }
}

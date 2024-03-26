namespace HeatItOn
{
    public class Optimizer : IOptimizer
    {
        // Method for calculating net production costs only for Gas Boiler and Oil Boiler
        // Net prod cost are prod cost themselves
        public void CalculateNetProdCostHeatOnly(ProductionUnit productionUnit)
        {
            ResultData resultDataHeatOnlyUnit = new();
            resultDataHeatOnlyUnit.ProductionUnitName = productionUnit.Name;
            resultDataHeatOnlyUnit.ProductionCosts = productionUnit.ProductionCosts;
        }
        // For gas motor, net prod costs = prod costs - Max Electricity * Electricity price
        public void CalculateNetProdCostGasMotor(ProductionUnit productionUnit)
        {
            // SDM gasMotorSDM = new();
            ResultData resultDataGasMotor = new();
            resultDataGasMotor.ProductionUnitName = productionUnit.Name;
            


        }
        // For Electric Boiler, net prod costs = prod costs + Max Electricity * Electricity price
        public void CalculateNetProdCostElectricBoiler(ProductionUnit productionUnit)
        {
            
        }
        public void CompareNetProdCosts()
        {

        }


    }
}
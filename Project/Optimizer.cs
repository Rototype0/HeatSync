namespace HeatItOn
{
    public class Optimizer : IOptimizer
    {
        // For gas motor, net prod costs = prod costs - Max Electricity * Electricity price
        // For Electric Boiler, net prod costs = prod costs + Max Electricity * Electricity price
        public double CalculateNetProductionCost(ProductionUnit productionUnit, SDMData sourceDataPoint)
        {
            double netProductionCost = productionUnit.ProductionCosts;
            double electricityPrice = sourceDataPoint.ElectricityPrice;
            netProductionCost -= productionUnit.MaxElectricity * electricityPrice;
            return netProductionCost;
        }

        public ProductionUnit GetLowestNetProductionCostUnit(List<ProductionUnit> productionUnits, SDMData sourceDataPoint)
        {
            ProductionUnit lowestCostUnit = new();
            double lowestCost = double.MaxValue;

            foreach (ProductionUnit productionUnit in productionUnits)
            {
                double netProductionCost = CalculateNetProductionCost(productionUnit, sourceDataPoint);
                if (netProductionCost < lowestCost)
                {
                    lowestCost = netProductionCost;
                    lowestCostUnit = productionUnit;
                }
            }

            return lowestCostUnit;
        }

        private ResultData CalculateResultDataPoint(List<ProductionUnit> productionUnits, SDMData sourceDataPoint)
        {
            // TODO: handle sub operation points here
            ResultData resultData = new();
            return resultData;
        }
        
        // secure heat availability? could be a unit test
        // also ensure list sizes of SDM and RDM data are the same in unit tests
        public List<ResultData> OptimizeData(List<ProductionUnit> productionUnits, List<SDMData> sourceData)
        {
            /*
            TODO:
            - fetch List<SourceData>
            - fetch List<ProductionUnit>
            - fetch HeatingGrid
            - get start/end point, and interval from source data
            - For each hour:
                - Find lowest net prod cost unit
                - Use lowest cost unit at some or full capacity (depending on heat demand)
                - If heat demand still not met for the hour, repeat and find the next cheapest net prod cost unit
            */

            List<ResultData> resultDatas = [];
            foreach (SDMData sourceDataPoint in sourceData)
            {
                foreach (ProductionUnit productionUnit in productionUnits)
                {
                    CalculateNetProductionCost(productionUnit, sourceDataPoint);

                    // after calculations are done, make sure to add separate result datapoints for each production unit.
                    // as a result, if we use 3 prod units for each hour and we have 50 source data points, we should have 150 result data points
                    ResultData resultData = new();
                    resultData.ProductionUnitName = "test";
                    resultData.ProducedHeat = 0;
                    resultData.NetElectricity = 0;
                    resultData.ProductionCosts = 0;
                    resultData.ProducedCO2 = 0;
                    resultData.PrimaryEnergyConsumption = 0;
                    resultDatas.Add(resultData);
                }
            }
            return resultDatas;
        }

        // Method for calculating net production costs only for Gas Boiler and Oil Boiler
        // Net prod cost are prod cost themselves
        public void CalculateNetProdCostHeatOnly(ProductionUnit productionUnit)
        {
            ResultData resultDataHeatOnlyUnit = new();
            resultDataHeatOnlyUnit.ProductionUnitName = productionUnit.Name;
            resultDataHeatOnlyUnit.ProductionCosts = productionUnit.ProductionCosts;
        }
        
        public void CalculateNetProdCostGasMotor(ProductionUnit productionUnit)
        {
            // SDM gasMotorSDM = new();
            ResultData resultDataGasMotor = new();
            resultDataGasMotor.ProductionUnitName = productionUnit.Name;
            


        }
        
        public void CalculateNetProdCostElectricBoiler(ProductionUnit productionUnit)
        {
            
        }
        public void CompareNetProdCosts()
        {

        }


    }
}
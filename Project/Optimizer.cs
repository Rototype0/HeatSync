namespace HeatItOn
{
    public class Optimizer : IOptimizer
    {
        // For gas motor, net prod costs = prod costs - Max Electricity * Electricity price
        // For Electric Boiler, net prod costs = prod costs + Max Electricity * Electricity price
        public double CalculateNetProductionCost(ProductionUnit productionUnit, SourceData sourceDataPoint)
        {
            double netProductionCost = productionUnit.ProductionCosts;
            double electricityPrice = sourceDataPoint.ElectricityPrice;
            netProductionCost -= productionUnit.MaxElectricity * electricityPrice;
            return netProductionCost;
        }

        public ProductionUnit GetLowestNetProductionCostUnit(List<ProductionUnit> productionUnits, SourceData sourceDataPoint)
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

        public ResultData ChangeOperationalPercentage(ResultData originalData, double newPercentage)
        {
            originalData.OperationPercentage = newPercentage;
            originalData.ProducedHeat *= newPercentage;
            originalData.NetElectricity *= newPercentage;
            originalData.ProducedCO2 *= newPercentage;
            originalData.PrimaryEnergyConsumption *= newPercentage;
            originalData.ProductionCosts = (int)(originalData.ProductionCosts * newPercentage); // TODO: look into how this would be rounded
            
            return originalData;
        }

        // Returns a list of ResultData for meeting heat demand of a single time interval (in this case, a single hour) from a source data point.

        // TODO: secure heat availability? could be a unit test
        // also ensure list sizes of SDM and RDM data are the same in unit tests (nvm)
        private List<ResultData> CalculateResultDataForInterval(List<ProductionUnit> productionUnits, SourceData sourceDataPoint)
        {
            List<ResultData> resultDatas = [];
            List<ProductionUnit> unusedProductionUnits = productionUnits;

            double currentHeatDemand = sourceDataPoint.HeatDemand;

            while (currentHeatDemand > 0)
            {
                // throw exception here if we exhausted all prod units and heat demand still not met?

                ProductionUnit cheapestUnit = GetLowestNetProductionCostUnit(unusedProductionUnits, sourceDataPoint);
                unusedProductionUnits.Remove(cheapestUnit);
                
                // after calculations are done, make sure to add separate result datapoints for each production unit.
                // as a result, if we use 3 prod units for each hour and we have 50 source data points, we should have 150 result data points
                ResultData resultData = new()
                {
                    ProductionUnitName = cheapestUnit.Name,
                    ProducedHeat = cheapestUnit.MaxHeat,
                    NetElectricity = cheapestUnit.MaxElectricity,
                    ProductionCosts = cheapestUnit.ProductionCosts,
                    ProducedCO2 = cheapestUnit.CO2Emissions,
                    PrimaryEnergyConsumption = cheapestUnit.GasConsumption,
                    OperationPercentage = 1
                };

                // Use lowest cost unit at some or full capacity (depending on heat demand)
                // If heat demand still not met for the hour, repeat and find the next cheapest net prod cost unit
                // TODO: handle sub operation points here, result data will need an operation percentage too
                if (cheapestUnit.MaxHeat > currentHeatDemand)
                {
                    double newPercentage = Math.Round((cheapestUnit.MaxHeat - currentHeatDemand) / cheapestUnit.MaxHeat * 100, 2);
                    resultData = ChangeOperationalPercentage(resultData, newPercentage);
                    currentHeatDemand -= cheapestUnit.MaxHeat * newPercentage;
                }
                else
                    currentHeatDemand -= cheapestUnit.MaxHeat;

                resultDatas.Add(resultData);
            }
            return resultDatas;
        }
        
        public List<ResultData> OptimizeData(List<ProductionUnit> productionUnits, List<SourceData> sourceData)
        {
            List<ResultData> resultDatas = [];
            foreach (SourceData sourceDataPoint in sourceData)
            {
                List<ResultData> intervalResultDatas = CalculateResultDataForInterval(productionUnits, sourceDataPoint);
                resultDatas.AddRange(intervalResultDatas);
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
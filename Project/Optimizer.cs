namespace HeatSync
{
    public class Optimizer : IOptimizer
    {
        // Calculates the net production cost of a specific unit.
        // The net production costs for heat only boilers are the productions costs themselves. 
        // For electricity producing units, net production costs = production costs minus the value of the electricity that can be sold.
        // For electricity consuming units, net production costs = production costs plus the value of the electricity that has to be purchased.
        public double CalculateNetProductionCost(ProductionUnit productionUnit, SourceData sourceDataPoint)
        {
            double netProductionCost = productionUnit.ProductionCosts;
            double electricityPrice = sourceDataPoint.ElectricityPrice;
            netProductionCost -= productionUnit.MaxElectricity * electricityPrice;
            return netProductionCost;
        }

        // Gets the lowest net cost production unit out of a given list in a specific time interval.
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

        // Tweaks data of a single data point to match the provided operation percentage (from 0.0 to 1.0)
        public ResultData ChangeOperationalPercentage(ResultData originalData, double newPercentage)
        {
            originalData.OperationPercentage = newPercentage;
            originalData.ProducedHeat *= newPercentage;
            originalData.NetElectricity *= newPercentage;
            originalData.ProducedCO2 *= newPercentage;
            originalData.PrimaryEnergyConsumption *= newPercentage;
            originalData.ProductionCosts *= newPercentage;
            
            return originalData;
        }

        // Returns a list of ResultData for meeting heat demand of a single time interval (in this case, a single hour) from a source data point.
        private List<ResultData> CalculateResultDataForInterval(List<ProductionUnit> productionUnits, SourceData sourceDataPoint)
        {
            List<ResultData> resultDatas = [];

            List<ProductionUnit> unusedProductionUnits = productionUnits.ToList();

            double currentHeatDemand = sourceDataPoint.HeatDemand;

            while (currentHeatDemand > 0.0)
            {
                // In cases where all production units are being used and heat demand is still not met, use everything available.
                if (unusedProductionUnits.Count == 0 && currentHeatDemand > 0)
                {
                    Console.WriteLine("Unable to meet heat demand for time interval " + sourceDataPoint.TimeFrom + " to " + sourceDataPoint.TimeTo);
                    return resultDatas;
                }
                
                ProductionUnit cheapestUnit = GetLowestNetProductionCostUnit(unusedProductionUnits, sourceDataPoint);
                unusedProductionUnits.Remove(cheapestUnit);
                
                ResultData resultData = new()
                {
                    TimeFrom = sourceDataPoint.TimeFrom,
                    TimeTo = sourceDataPoint.TimeTo,
                    ProductionUnitName = cheapestUnit.Name,
                    ProducedHeat = cheapestUnit.MaxHeat,
                    NetElectricity = cheapestUnit.MaxElectricity,
                    ProductionCosts = cheapestUnit.ProductionCosts,
                    ProducedCO2 = cheapestUnit.CO2Emissions,
                    PrimaryEnergyConsumption = cheapestUnit.GasConsumption,
                    OperationPercentage = 1
                };

                // Don't run unit on max operation if heat demand is less than unit's max heat.
                double newPercentage = 1;
                if (cheapestUnit.MaxHeat > currentHeatDemand)
                {
                    newPercentage = currentHeatDemand / cheapestUnit.MaxHeat;
                    resultData = ChangeOperationalPercentage(resultData, newPercentage);
                }

                // [Dan] Math.Round seems to be necessary here, otherwise the optimizer sees an insignificantly small bit of heat demand left
                // and attempts to fill it with remaining units with an insignificantly small operation percentage.
                currentHeatDemand = Math.Round(currentHeatDemand - cheapestUnit.MaxHeat * newPercentage, 15);
                
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
    }
}
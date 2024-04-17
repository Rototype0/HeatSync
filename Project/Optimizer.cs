namespace HeatItOn
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
        // roundToDigits can be used to round up each variable to a specific amount of numbers after the decimal point.
        public ResultData ChangeOperationalPercentage(ResultData originalData, double newPercentage, int roundToDigits)
        {
            // TODO: this may round values down to the point where it doesn't meet heat demand, check to make sure
            originalData.OperationPercentage = Math.Round(newPercentage, roundToDigits);
            originalData.ProducedHeat = Math.Round(originalData.ProducedHeat * newPercentage, roundToDigits);
            originalData.NetElectricity = Math.Round(originalData.NetElectricity * newPercentage, roundToDigits);
            originalData.ProducedCO2 = Math.Round(originalData.ProducedCO2 * newPercentage, roundToDigits);
            originalData.PrimaryEnergyConsumption = Math.Round(originalData.PrimaryEnergyConsumption * newPercentage, roundToDigits);
            originalData.ProductionCosts = Math.Round(originalData.ProductionCosts * newPercentage, roundToDigits);
            
            return originalData;
        }

        // Returns a list of ResultData for meeting heat demand of a single time interval (in this case, a single hour) from a source data point.
        private List<ResultData> CalculateResultDataForInterval(List<ProductionUnit> productionUnits, SourceData sourceDataPoint, int roundToDigits)
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
                if (cheapestUnit.MaxHeat > currentHeatDemand)
                {
                    double newPercentage = currentHeatDemand / cheapestUnit.MaxHeat;
                    resultData = ChangeOperationalPercentage(resultData, newPercentage, roundToDigits);
                    currentHeatDemand = Math.Round(currentHeatDemand - cheapestUnit.MaxHeat * newPercentage, 2);
                }
                else
                    currentHeatDemand = Math.Round(currentHeatDemand - cheapestUnit.MaxHeat, 2);
                
                resultDatas.Add(resultData);
            }
            return resultDatas;
        }
        
        public List<ResultData> OptimizeData(List<ProductionUnit> productionUnits, List<SourceData> sourceData, int roundToDigits = 2)
        {
            List<ResultData> resultDatas = [];
            foreach (SourceData sourceDataPoint in sourceData)
            {
                List<ResultData> intervalResultDatas = CalculateResultDataForInterval(productionUnits, sourceDataPoint, roundToDigits);
                resultDatas.AddRange(intervalResultDatas);
            }
            return resultDatas;
        }
    }
}
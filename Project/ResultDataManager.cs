namespace HeatItOn
{
    public struct ResultData // draft structure
    {
        public string Name { get; set; }
        public double HeatProduction { get; set; }
        public double Electricity { get; set; }
        public int NetProductionCosts { get; set; }
        public double CO2Emissions { get; set; }
        public double GasConsumption { get; set; }
    }
}
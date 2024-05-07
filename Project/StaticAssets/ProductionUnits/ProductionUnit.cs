namespace HeatSync
{
    public struct ProductionUnit
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public double MaxHeat { get; set; }
        public double MaxElectricity { get; set; }
        public double ProductionCosts { get; set; }
        public double CO2Emissions { get; set; }
        public double GasConsumption { get; set; }
    }
}
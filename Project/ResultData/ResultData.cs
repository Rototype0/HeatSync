namespace HeatItOn
{
    public struct ResultData
    {
        public string ProductionUnitName { get; set; }
        public double ProducedHeat { get; set; }
        public double NetElectricity { get; set; }
        public int ProductionCosts { get; set; }
        public double ProducedCO2 { get; set; }
        public double PrimaryEnergyConsumption { get; set; }
    }
}
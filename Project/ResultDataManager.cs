using CsvHelper;

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

    public class ResultDataManager : IResultDataManager
    {
        public ResultData ReadResultData(string data)
        {
            ResultData resultData = new();
            return resultData;
        }
        public void WriteResultData(ResultData resultData)
        {
            resultData.Name = "Name";
            resultData.HeatProduction = 0.25;
            resultData.Electricity = 0.5;
            resultData.NetProductionCosts = 75;
            resultData.CO2Emissions = 75.8;
            resultData.CO2Emissions = 4.5678;

            var records = new List<ResultData>
            {
                new() { Name = "Name", HeatProduction = 0.25, Electricity = 0.5, NetProductionCosts = 75, CO2Emissions = 75.8, GasConsumption = 4.5678 },
                new() { Name = "Name2", HeatProduction = 0.35, Electricity = 0.45, NetProductionCosts = 775, CO2Emissions = 45.8, GasConsumption = 1.23 },
            };

            using (var writer = new StreamWriter("test.csv"))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
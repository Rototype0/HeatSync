using CsvHelper;

namespace HeatItOn
{
    public struct ResultData
    {
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public string ProductionUnitName { get; set; }
        public double ProducedHeat { get; set; }
        public double NetElectricity { get; set; }
        public int ProductionCosts { get; set; }
        public double ProducedCO2 { get; set; }
        public double PrimaryEnergyConsumption { get; set; }
        public double OperationPercentage { get; set; }
    }
    public class ResultDataManager : IResultDataManager
    {
        public List<ResultData> ReadResultData(string fileName)
        {
            try
            {
                using var reader = new StreamReader("ResultData\\" + fileName + ".csv");
                using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
                var records = csv.GetRecords<ResultData>();
                return records.ToList();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }
        public void WriteResultData(List<ResultData> resultData, string fileName)
        {
            using var writer = new StreamWriter("ResultData\\" + fileName + ".csv");
            using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(resultData);
        }
    }
}
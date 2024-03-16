using CsvHelper;

namespace HeatItOn
{
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
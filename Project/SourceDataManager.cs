using CsvHelper;
using System.Text.Json;

namespace HeatItOn
{
    public struct SourceData
    {
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public double HeatDemand { get; set; }
        public double ElectricityPrice { get; set; }
    }
    
    public class SourceDataManager : ISourceDataManager
    {
        public struct HourlyElectricityPrice
        {
            public DateTime HourDK { get; set; }
            public double SpotPriceDKK { get; set; }
        }
        public struct ElectricityPrices
        {
            public List<HourlyElectricityPrice>? records { get; set; }
        }
        public async Task<byte[]?> GetEnerginetAPIData(string url)
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                return content;
            }

            return null;
        }

        private static double GetEnerginetHeatDemand()
        {
            Random random = new();
            return random.NextDouble() * 10; // randomized from 0 to 10.0 since we can't pull heat demand data from APIs at the moment
        }
        public async Task<List<SourceData>> ReadEnerginetAPISourceData()
        {
            List<SourceData> sourceData = [];

            // Getting electricity prices
            // dataset=Elspotprices; start=now-P6D; columns=HourDK,SpotPriceDKK; filter={PriceArea=["DK1"]}; sort=HourDK; limit=0
            string url = "https://api.energidataservice.dk/dataset/Elspotprices?start=now-P6D&columns=HourDK%2CSpotPriceDKK&filter=%7B%22PriceArea%22%3A%5B%22DK1%22%5D%7D&sort=HourDK&limit=0";

            var electricityPrices = await GetEnerginetAPIData(url);
            if (electricityPrices == null)
                return sourceData;
            
            ElectricityPrices priceList = JsonSerializer.Deserialize<ElectricityPrices>(electricityPrices)!;
            if (priceList.records == null)
                return sourceData;
            
            foreach (HourlyElectricityPrice price in priceList.records)
            {
                SourceData sourceDataPoint = new()
                {
                    TimeFrom = price.HourDK,
                    TimeTo = price.HourDK.AddHours(1),
                    HeatDemand = GetEnerginetHeatDemand(),
                    ElectricityPrice = price.SpotPriceDKK
                };
                sourceData.Add(sourceDataPoint);
            }
            
            return sourceData;
        }
        public List<SourceData> ReadSourceData(string fileName)
        {
            try
            {
                using var reader = new StreamReader("SourceData\\" + fileName + ".csv");
                using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
                var records = csv.GetRecords<SourceData>();
                return records.ToList();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }

        public void WriteSourceData(List<SourceData> sourceData, string fileName)
        {
            using var writer = new StreamWriter("SourceData\\" + fileName + ".csv");
            using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(sourceData);
        }
    }
}

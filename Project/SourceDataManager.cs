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
        public struct ElectricityPrices
        {
            public DateTime HourDK { get; set; }
            public double SpotPriceDKK { get; set; }
        }
        public struct EnerginetAPIRecords
        {
            public List<ElectricityPrices>? records { get; set; }
        }
        private static async Task<byte[]?> GetEnerginetAPIData(string url)
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

        // Gets electricity prices from energinet.dk API and returns List<ElectricityPrices>.
        // Default URL is equivalent to:
        // dataset=Elspotprices; start=now-P6D; columns=HourDK,SpotPriceDKK; filter={PriceArea=["DK1"]}; sort=HourDK; limit=0
        private static async Task<List<ElectricityPrices>> GetEnerginetElectricityPrices(string url)
        {
            var content = await GetEnerginetAPIData(url);
            if (content == null)
                return [];

            EnerginetAPIRecords priceList = JsonSerializer.Deserialize<EnerginetAPIRecords>(content)!;
            if (priceList.records == null)
                return [];
            
            return priceList.records;
        }

        // Pseudorandom number generator for getting a list of bounded heat demand values (since we can't pull heat demand data from APIs at the moment)
        // This needs to be pseudorandom (instead of truly random) in order to be usable in tests.
        public static List<double> GenerateHeatDemands(int count, int seed = 0)
        {
            List<double> heatDemands = [];
            Random random = new(seed);

            for (int i = 0; i < count; i++)
            {
                heatDemands.Add(random.NextDouble() * 10); // randomized from 0 to 10.0
            }

            return heatDemands;
        }

        public async Task<List<SourceData>> ReadAPISourceData(
            string url = "https://api.energidataservice.dk/dataset/Elspotprices?start=now-P6D&columns=HourDK%2CSpotPriceDKK&filter=%7B%22PriceArea%22%3A%5B%22DK1%22%5D%7D&sort=HourDK&limit=0"
        )
        {
            List<SourceData> sourceData = [];
            List<ElectricityPrices> priceList = await GetEnerginetElectricityPrices(url);
            List<double> heatDemands = GenerateHeatDemands(priceList.Count);

            for (int i = 0; i < priceList.Count; i++)
            {
                SourceData sourceDataPoint = new()
                {
                    TimeFrom = priceList[i].HourDK,
                    TimeTo = priceList[i].HourDK.AddHours(1),
                    HeatDemand = heatDemands[i],
                    ElectricityPrice = priceList[i].SpotPriceDKK
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

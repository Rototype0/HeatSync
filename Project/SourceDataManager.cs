using CsvHelper;
using System.Data;
using System.Timers;
using System.Text.Json;

namespace HeatItOn
{
    public struct SourceData
    {
        //public DateTime CurrentTime { get; set; }
        //public double TimeSinceLastUpdate { get; set; }

        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public double HeatDemand { get; set; }
        public double ElectricityPrice { get; set; }
    }

    public class APIRecord
    {
        public DateTime HourDK { get; set; }
        public double SpotPriceDKK { get; set; }
    }
    public class APIRecords
    {
        public List<APIRecord>? records { get; set; } // FIXME: if this variable name is Capitalized, this no longer works for some reason?
    }
    public class SourceDataManager
    {
        static readonly HttpClient client = new();
        public static async Task<List<SourceData>> GetEnerginetElectricityPrices()
        {
            Random random = new();
            List<SourceData> sourceData = [];
            // dataset=Elspotprices; start=now-P6D; columns=HourDK,SpotPriceDKK; filter={PriceArea=["DK1"]}; sort=HourDK; limit=0
            string url = "https://api.energidataservice.dk/dataset/Elspotprices?start=now-P6D&columns=HourDK%2CSpotPriceDKK&filter=%7B%22PriceArea%22%3A%5B%22DK1%22%5D%7D&sort=HourDK&limit=0";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                APIRecords recordList = JsonSerializer.Deserialize<APIRecords>(content)!;
                if (recordList.records == null)
                    return sourceData;
                foreach (APIRecord record in recordList.records)
                {
                    SourceData sourceDataPoint = new()
                    {
                        TimeFrom = record.HourDK,
                        TimeTo = record.HourDK.AddHours(1),
                        HeatDemand = random.NextDouble() * 10, // randomized from 0 to 10.0 since we can't pull heat demand data from APIs at the moment
                        ElectricityPrice = record.SpotPriceDKK
                    };
                    sourceData.Add(sourceDataPoint);
                }
            }
            return sourceData;
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private SourceData CurrentSourceData;
        private System.Timers.Timer SaveTimer;

        public SourceDataManager(string FileName, float SaveInterval)
        {
            this.FileName = FileName;
            //SetTimer(SaveInterval);
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

        public void UpdateSourceData( SourceData Data) 
        {
            CurrentSourceData = Data;
        }

        private void SetTimer(float SaveInterval)
        {
            // Create a timer with a two second interval.
            SaveTimer = new System.Timers.Timer(SaveInterval);
            // Hook up the Elapsed event for the timer. 
            SaveTimer.Elapsed += OnTimedEvent;
            SaveTimer.AutoReset = true;
            SaveTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (!File.Exists(FileName))
            {
                using (var writer = new StreamWriter(FileName))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.WriteRecord(CurrentSourceData);
                }
            }
            else
            {
                IEnumerable<SourceData> CSVRecords = new List<SourceData>();

                using (var reader = new StreamReader(FileName))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    CSVRecords = csv.GetRecords<SourceData>();
                }

                using (var writer = new StreamWriter(FileName))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(CSVRecords);
                    csv.WriteRecord(CurrentSourceData);
                }
            }
        }
    }
}

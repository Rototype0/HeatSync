using CsvHelper;
using System.Data;
using System.Timers;

namespace HeatItOn
{
    public struct SourceData
    {
        public DateTime CurrentTime { get; set; }
        public double TimeSinceLastUpdate { get; set; }
        public double HeatDemand { get; set; }
        public double ElectricityPrice { get; set; }
    }
    public class SourceDataManager
    {
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
            SetTimer(SaveInterval);
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

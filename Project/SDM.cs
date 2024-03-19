using CsvHelper;
using System.Data;
using System.Timers;

namespace HeatItOn
{
    public struct SDMData
    {
        public DateTime CurrentTime { get; set; }
        public float TimeSinceLastUpdate { get; set; }
        public float HeatDemand { get; set; }
        public float ElecticityPrice { get; set; }
    }
    public class SDM
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private SDMData CurrentSDMData;
        private System.Timers.Timer SaveTimer;


        public SDM(string FileName, float SaveInterval)
        {
            this.FileName = FileName;
            SetTimer(SaveInterval);
        }

        public void UpdateSDMData( SDMData Data) 
        {
            CurrentSDMData = Data;
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
                    csv.WriteRecord(CurrentSDMData);
                }
            }
            else
            {
                IEnumerable<SDMData> CSVRecords = new List<SDMData>();

                using (var reader = new StreamReader(FileName))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    CSVRecords = csv.GetRecords<SDMData>();
                }

                using (var writer = new StreamWriter(FileName))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(CSVRecords);
                    csv.WriteRecord(CurrentSDMData);
                }
            }
        }
    }
}

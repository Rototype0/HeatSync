using Raylib_cs;

namespace HeatItOn
{
    class Program
    {
        private static void Main(string[] args)
        {
            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new();
            ResultDataManager resultDataManager = new();
            Window MainWindow = new Window();
            Optimizer optimizer = new();
            
            List<SourceData> data = sourceDataManager.ReadEnerginetAPISourceData().Result;
            
            /*
            foreach (SourceData dataPoint in data)
            {
                Console.WriteLine(dataPoint.TimeFrom);
                Console.WriteLine(dataPoint.TimeTo);
                Console.WriteLine(dataPoint.HeatDemand);
                Console.WriteLine(dataPoint.ElectricityPrice);
            }
            */
            List<ProductionUnit> productionUnits = jsonAssetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, data);

            string fileName = "ResultDataTest";
            resultDataManager.WriteResultData(writeRecords, fileName);

            while(!Raylib.WindowShouldClose() && MainWindow.IsImGUIWindowOpen)
            {
                MainWindow.Render();
            }
        }
    }
}

using Raylib_cs;

namespace HeatSync
{
    class Program
    {
        private static void Main(string[] args)
        {
            //JsonAssetManager assetManager = new();
            XmlAssetManager assetManager = new();

            SourceDataManager sourceDataManager = new();
            ResultDataManager resultDataManager = new();
            
            Optimizer optimizer = new();

            //List<SourceData> data = sourceDataManager.ReadAPISourceData().Result;
            List<SourceData> data = sourceDataManager.ReadSourceData("summertest");
            List<ProductionUnit> productionUnits = assetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, data);

            DataVisualizer MainWindow = new DataVisualizer(1280, 720, data, productionUnits, writeRecords);

            string fileName = "ResultDataTest";
            resultDataManager.WriteResultData(writeRecords, fileName);

            while(!Raylib.WindowShouldClose() && MainWindow.IsImGUIWindowOpen)
            {
                MainWindow.Render();
            }

            MainWindow.controller.Shutdown();
        }
    }
}

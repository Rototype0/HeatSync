using Raylib_cs;

namespace HeatSync
{
    class Program
    {
        private static void Main()
        {
            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new();
            ResultDataManager resultDataManager = new();
            Optimizer optimizer = new();
          
            List<SourceData> data = sourceDataManager.ReadAPISourceData().Result;
            List<SourceData> initialData = sourceDataManager.ReadSourceData("summertest");
            List<ProductionUnit> productionUnits = jsonAssetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, data);

            DataVisualizer MainWindow = new DataVisualizer(1280, 720, initialData, productionUnits, writeRecords);

            string fileName = "ResultData";
            resultDataManager.WriteResultData(writeRecords, fileName);

            while(!Raylib.WindowShouldClose() && MainWindow.IsImGUIWindowOpen)
            {
                MainWindow.Render();
                if(MainWindow.UpdateDataFlag)
                {
                    MainWindow.UpdateData(data, productionUnits, writeRecords);
                    MainWindow.UpdateDataFlag = false;
                }
            }

            MainWindow.controller.Shutdown();
        }
    }
}

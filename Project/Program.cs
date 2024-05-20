using System.Collections.Generic;
using Raylib_cs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HeatSync
{
    class Program
    {
        private static void Main(string[] args)
        {
            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new();
            ResultDataManager resultDataManager = new();
            Optimizer optimizer = new();
          
            List<SourceData> data = sourceDataManager.ReadAPISourceData().Result;
            List<SourceData> initial = sourceDataManager.ReadSourceData("summertest");
            List<ProductionUnit> productionUnits = jsonAssetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, data);

            DataVisualizer MainWindow = new DataVisualizer(1280, 720, initial, productionUnits, writeRecords);

            string fileName = "ResultDataTest";
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

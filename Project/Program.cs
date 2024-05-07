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
            //List<SourceData> data = sourceDataManager.ReadSourceData("summertest");
            List<ProductionUnit> productionUnits = jsonAssetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, data);

            Window MainWindow = new Window(1280, 720, data, productionUnits, writeRecords);

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

﻿using Raylib_cs;

namespace HeatSync
{
    class Program
    {
        private static void Main()
        {
            JsonAssetManager assetManager = new();
            //XmlAssetManager assetManager = new();

            SourceDataManager sourceDataManager = new();
            ResultDataManager resultDataManager = new();
            
            Optimizer optimizer = new();
            
            // Sometimes (especially during morning), the API doesn't work and the program crashes, hence it's not used by default 
            //List<SourceData> data = sourceDataManager.ReadAPISourceData().Result;
            List<SourceData> data = sourceDataManager.ReadSourceData("wintertest");
            List<SourceData> initialData = sourceDataManager.ReadSourceData("summertest");

            HeatingGrid gridData = assetManager.LoadHeatingGridData(File.ReadAllText("StaticAssets\\HeatingGrids\\heatington.json"));

            List<ProductionUnit> productionUnits = assetManager.GetAllProductionUnits();

            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, initialData);

            DataVisualizer MainWindow = new DataVisualizer(1280, 720, initialData, gridData, productionUnits, writeRecords);

            string fileName = "ResultData";
            resultDataManager.WriteResultData(writeRecords, fileName);

            while(!Raylib.WindowShouldClose() && MainWindow.IsImGUIWindowOpen)
            {
                MainWindow.Render();
                if(MainWindow.UpdateDataFlag)
                {
                    MainWindow.UpdateData(data, gridData, productionUnits, writeRecords);
                    MainWindow.UpdateDataFlag = false;
                }
            }

            MainWindow.controller?.Shutdown();
        }
    }
}

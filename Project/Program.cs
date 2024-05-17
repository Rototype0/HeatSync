using Raylib_cs;

namespace HeatSync
{
    class Program
    {
        private static void Main(string[] args)
        {
            JsonAssetManager jsonAssetManager = new();
            SourceDataManager sourceDataManager = new();
            ResultDataManager resultDataManager = new();
            XmlAssetManager xmlAssetManager= new();
            Optimizer optimizer = new();

            Console.WriteLine("hello?");

            ProductionUnit productionUnit = xmlAssetManager.LoadProductionUnitData("StaticAssets\\ProductionUnits\\gasBoiler.xml");

            if (productionUnit.Name == "")
            {
                Console.WriteLine("something's wrong yo");
            }

            Console.Write(
            productionUnit.Name + "\t" +
            productionUnit.CO2Emissions + "\t" +
            productionUnit.MaxElectricity + "\t");
          
            /*List<SourceData> data = sourceDataManager.ReadAPISourceData().Result;
            //List<SourceData> data = sourceDataManager.ReadSourceData("summertest");
            List<ProductionUnit> productionUnits = jsonAssetManager.GetAllProductionUnits();
            List<ResultData> writeRecords = optimizer.OptimizeData(productionUnits, data);

            DataVisualizer MainWindow = new DataVisualizer(1280, 720, data, productionUnits, writeRecords);

            string fileName = "ResultDataTest";
            resultDataManager.WriteResultData(writeRecords, fileName);

            while(!Raylib.WindowShouldClose() && MainWindow.IsImGUIWindowOpen)
            {
                MainWindow.Render();
            }

            MainWindow.controller.Shutdown();*/
        }
    }
}

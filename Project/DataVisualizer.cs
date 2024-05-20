using ImGuiNET;
using System.Numerics;
using Raylib_cs;

namespace HeatSync
{
    internal class DataVisualizer : IDataVisualizer
    {
        private bool isImGUIWindowOpen;

        internal bool IsImGUIWindowOpen
        {
            get { return isImGUIWindowOpen; }
        }

        private ProductionUnit[] ProductionUnits;

        private List<List<ResultData>> ResultDataListByProductionUnit;
        private float[] HeatDemand;
        private float[] ElectricityPrices;
        private float[] CollectiveCO2Emissions;
        private float[] CollectiveProductionCosts;


        private List<float[]> SeperateCO2Emissions;
        private List<float[]> SeperateHeatDemand;
        private List<float[]> SeperateGasConsumption;

        internal ImGuiController controller;
        private ImGuiWindowFlags ImGuiWindowFlags;
        private ConfigFlags RaylibWindowFlags;

        private int FontScale = 1;

        private JsonAssetManager AssetManager;
        private HeatingGrid HeatingGridData;
        private Texture2D HeatingGridTexture;
        private List<Texture2D> ProductionUnitsTextures;

        internal DataVisualizer(int WindowWidth, int WindowHeight, List<SourceData> Data, List<ProductionUnit> ProductionUnits, List<ResultData> WriteRecords)
        {
            isImGUIWindowOpen = true;
            ImGuiWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoBringToFrontOnFocus;
            RaylibWindowFlags = ConfigFlags.VSyncHint | ConfigFlags.UndecoratedWindow;

            Raylib.SetConfigFlags(RaylibWindowFlags);
            Raylib.InitWindow(WindowWidth, WindowHeight, "Plot");
            Raylib.SetTargetFPS(60);

            var context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);

            controller = new ImGuiController();
            AssetManager = new JsonAssetManager();

            UpdateData(WindowWidth, WindowHeight, Data, ProductionUnits, WriteRecords);
        }

        public void UpdateData(int WindowWidth, int WindowHeight, List<SourceData> Data, List<ProductionUnit> ProductionUnits, List<ResultData> WriteRecords)
        {
            this.ProductionUnits = ProductionUnits.ToArray();

            ResultDataListByProductionUnit = SeperateResultDataListByProductionUnit(WriteRecords);
            CollectiveCO2Emissions = new float[Data.Count];
            CollectiveProductionCosts = new float[Data.Count];
            SeperateCO2Emissions = new List<float[]>();
            SeperateHeatDemand = new List<float[]>();
            SeperateGasConsumption = new List<float[]>();

            for (int i = 0; i < ProductionUnits.Count; i++)
            {
                SeperateCO2Emissions.Add(new float[Data.Count]);
                SeperateHeatDemand.Add(new float[Data.Count]);
                SeperateGasConsumption.Add(new float[Data.Count]);
            }

            for (int i = 0; i < Data.Count; i++)
            {
                for (int y = 0; y < ProductionUnits.Count; y++)
                {
                    CollectiveCO2Emissions[i] += (float)ResultDataListByProductionUnit[y][i].ProducedCO2;
                    CollectiveProductionCosts[i] += (float)ResultDataListByProductionUnit[y][i].ProductionCosts;
                    SeperateCO2Emissions[y][i] = (float)ResultDataListByProductionUnit[y][i].ProducedCO2;
                    SeperateHeatDemand[y][i] = (float)ResultDataListByProductionUnit[y][i].ProducedHeat;
                    SeperateGasConsumption[y][i] = (float)ResultDataListByProductionUnit[y][i].PrimaryEnergyConsumption;
                }
            }

            HeatDemand = new float[Data.Count];
            ElectricityPrices = new float[Data.Count];
            
            for (int i = 0; i < Data.Count; i++)
            {
                HeatDemand[i] = (float)Data[i].HeatDemand;
                ElectricityPrices[i] = (float)Data[i].ElectricityPrice;
            }

            HeatingGridData = AssetManager.LoadHeatingGridData(File.ReadAllText("StaticAssets\\HeatingGrids\\heatington.json"));
            ProductionUnitsTextures = new List<Texture2D>();

            HeatingGridTexture = LoadImage(HeatingGridData.ImagePath);
            for (int i = 0; i < ProductionUnits.Count; i++)
            {
                ProductionUnitsTextures.Add(LoadImage(ProductionUnits[i].ImagePath));
            }
        }

        private List<List<ResultData>> SeperateResultDataListByProductionUnit(List<ResultData> ResultDataList)
        {
            List<List<ResultData>> SeperatedListByProductionUnit = new List<List<ResultData>>();
            List<List<ResultData>> SeperatedList = new List<List<ResultData>>();
            List<ResultData> CurrentBoilerStack = new List<ResultData>() { ResultDataList[0] } ;
            DateTime CurrentUpperTimeLimit = ResultDataList[0].TimeFrom;

            foreach (var SamplePoint in ResultDataList)
            {
                if (SamplePoint.TimeFrom == CurrentUpperTimeLimit)
                {
                    SeperatedList.Add(CurrentBoilerStack);
                    CurrentUpperTimeLimit = SamplePoint.TimeTo;
                    CurrentBoilerStack = new List<ResultData>();
                }
                CurrentBoilerStack.Add(SamplePoint);
            }
            
            foreach (List<ResultData> BoilerStack in SeperatedList)
            {
                List<ResultData> FilledBoilerStack = BoilerStack;
                foreach (ProductionUnit ProductionUnit in ProductionUnits)
                {
                    if(!FilledBoilerStack.Any(Boiler => Boiler.ProductionUnitName == ProductionUnit.Name))
                    {
                        FilledBoilerStack.Add(new ResultData()
                        {
                            TimeFrom = BoilerStack[0].TimeFrom,
                            TimeTo = BoilerStack[0].TimeTo,
                            ProductionUnitName = ProductionUnit.Name,
                            ProducedHeat = 0,
                            NetElectricity = 0,
                            ProductionCosts = 0,
                            ProducedCO2 = 0,
                            PrimaryEnergyConsumption = 0,
                            OperationPercentage = 0
                        });
                    }
                }

                foreach (ResultData Boiler in FilledBoilerStack)
                {
                    bool HasFoundBoiler = false;

                    for (int i = 0; i < SeperatedListByProductionUnit.Count; i++)
                    {
                        if(SeperatedListByProductionUnit[i].Any(ProductionUnit => ProductionUnit.ProductionUnitName == Boiler.ProductionUnitName))
                        {
                            if(SeperatedListByProductionUnit.Count == 1)
                            {
                                SeperatedListByProductionUnit[i][0] = Boiler;
                            }
                            SeperatedListByProductionUnit[i].Add(Boiler);
                            HasFoundBoiler = true;
                            i = SeperatedListByProductionUnit.Count;
                        }
                    }

                    if(!HasFoundBoiler)
                    {
                        SeperatedListByProductionUnit.Add(new List<ResultData> { Boiler });
                    }
                }
            }

            return SeperatedListByProductionUnit;
        }

        public void Render()
        {
            controller.NewFrame();
            controller.ProcessEvent();
            
            ImGui.NewFrame();
            ImGui.SetNextWindowPos(new Vector2(0, 0));

            ImGui.Begin("Test", ref isImGUIWindowOpen, ImGuiWindowFlags);

            Raylib.SetWindowSize((int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y);

            InputHandling();
            
            /*
            ImGui.LabelText(ResultDataListByProductionUnit[0][0].TimeFrom.ToString(),
    ResultDataListByProductionUnit[ResultDataListByProductionUnit.Count - 1][ResultDataListByProductionUnit[ResultDataListByProductionUnit.Count - 1].Count - 1].TimeTo.ToString());
            */
            
            ImGui.BeginTabBar("Main");
            if(ImGui.BeginTabItem("Source Data"))
            {
                RenderPlotLines(HeatDemand, "Heat Demand");
                RenderPlotLines(ElectricityPrices, "Electricity Prices");
                ImGui.Button("Update and optimize data", new Vector2(ImGui.CalcTextSize("Update and optimize data").X + 32, ImGui.CalcTextSize("Update and optimize data").Y + 8));


                ImGui.EndTabItem();
            }

            if(ImGui.BeginTabItem("Result Data"))
            {
                ImGui.BeginTabBar("Settings#left_tabs_bar");
                if (ImGui.BeginTabItem("General Information"))
                {
                    RenderPlotLines(HeatDemand, "Heat Production");
                    RenderPlotHistogram(CollectiveCO2Emissions, "Collective CO2 Emissions");
                    RenderTable(CollectiveProductionCosts, "Fuel expences", 3);


                    ImGui.EndTabItem();
                }

                for (int i = 0; i < ProductionUnits.Length; i++)
                {
                    if (ImGui.BeginTabItem(ResultDataListByProductionUnit[i][0].ProductionUnitName))
                    {
                        RenderPlotLines(SeperateHeatDemand[i], "Heat Production");
                        RenderTable(SeperateGasConsumption[i], "Fuel Consumption", 3);
                        RenderTable(SeperateCO2Emissions[i], "CO2 Emissions", 3);

                        ImGui.EndTabItem();
                    }
                }

                ImGui.EndTabBar();
                ImGui.EndTabItem();
            }

            if(ImGui.BeginTabItem("Settings"))
            {
                ImGui.BeginTabBar("Production Units");

                if(ImGui.BeginTabItem("Visualizer settings"))
                {
                    ImGui.SliderInt("Font scale:", ref FontScale, 1, 5);
                    ImGui.SetWindowFontScale(FontScale);
                    ImGui.EndTabItem();
                }

                for (int i = 0; i < ProductionUnits.Length; i++)
                {
                    if (ImGui.BeginTabItem(ProductionUnits[i].Name))
                    {
                        ImGui.LabelText(ProductionUnits[i].Name, "Production unit name:");
                        ImGui.LabelText(ProductionUnits[i].MaxHeat.ToString(), "Max heat production:");
                        ImGui.LabelText(ProductionUnits[i].MaxElectricity.ToString(), "Max electricity consumption/production:");
                        ImGui.LabelText(ProductionUnits[i].ProductionCosts.ToString(), "Max production cost:");
                        ImGui.LabelText(ProductionUnits[i].CO2Emissions.ToString(), "Max CO2 production:");
                        ImGui.LabelText(ProductionUnits[i].GasConsumption.ToString(), "Max fuel consumption:");


                        ImGui.EndTabItem();
                    }
                }

                if(ImGui.BeginTabItem("Heating Grid"))
                {
                    
                    ImGui.LabelText(HeatingGridData.City, "Heating grid name:");
                    ImGui.LabelText(HeatingGridData.Size, "Heating grid size:");
                    ImGui.LabelText(HeatingGridData.Architecture, "Heating grid architecture:");
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
                ImGui.EndTabItem();
            }


            ImGui.EndTabBar();
            ImGui.End();
            

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            ImGui.Render();
            controller.Render(ImGui.GetDrawData());

            //DrawTexture(HeatingGridTexture,100,0);

            Raylib.EndDrawing();

        }

        private void RenderTable(float[] samples, string Label, int Width)
        {
            if(ImGui.CollapsingHeader(Label))
            {
                if (ImGui.BeginTable(Label, Width, ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollY, new Vector2(ImGui.GetWindowSize().X - 16, 200)))
                {
                    ImGui.TableSetupColumn(Label); ImGui.TableSetupColumn("From"); ImGui.TableSetupColumn("To");
                    ImGui.TableHeadersRow();
                    for (int i = 0; i < samples.Length; i++)
                    {
                        ImGui.TableNextColumn(); ImGui.Text(samples[i].ToString());
                        ImGui.TableNextColumn(); ImGui.Text(ResultDataListByProductionUnit[0][i].TimeFrom.ToString());
                        ImGui.TableNextColumn(); ImGui.Text(ResultDataListByProductionUnit[0][i].TimeTo.ToString());
                    }
                    ImGui.EndTable();
                }
            }
        }
        private void RenderPlotHistogram(float[] samples, string Label)
        {
            if (ImGui.CollapsingHeader(Label)) 
            {
                ImGui.PlotHistogram("", ref samples[0], samples.Length, 0, Label, samples.Min(), samples.Max() + samples.Max() / 10, new System.Numerics.Vector2(ImGui.GetWindowSize().X - 16, 250));
            }
            
        }
        private void RenderPlotLines(float[] samples, string Label)
        {
            if (ImGui.CollapsingHeader(Label, ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.PlotLines("", ref samples[0], samples.Length, 0, Label, samples.Min(), samples.Max() + samples.Max() / 10, new System.Numerics.Vector2(ImGui.GetWindowSize().X - 16, 250));
            }
        }

        private void InputHandling()
        {
            if (ImGui.IsMouseDragging(ImGuiMouseButton.Left) && ImGui.GetMousePos().Y <= 20 + ImGui.GetMouseDragDelta().Y)
            {
                var CurrentWindowPosition = Raylib.GetWindowPosition();
                Raylib.SetWindowPosition((int)(CurrentWindowPosition.X + ImGui.GetMouseDragDelta().X), (int)(CurrentWindowPosition.Y + ImGui.GetMouseDragDelta().Y));
            }
        }

        private Texture2D LoadImage(string FilePath)
        {
            Image image = Raylib.LoadImage(FilePath);

            Raylib.ImageDrawPixel(ref image, 0,0, Color.RayWhite);
            Texture2D texture = Raylib.LoadTextureFromImage(image);

            Raylib.UnloadImage(image);

            return texture;
        }

        private void DrawTexture(Texture2D Texture, int PosX, int PosY)
        {
            Raylib.DrawTexture(Texture, PosX, PosY, Color.White);
        }
    }
}

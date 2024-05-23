using ImGuiNET;
using System.Numerics;
using Raylib_cs;

namespace HeatSync
{
    internal class DataVisualizer : IDataVisualizer
    {
        public bool UpdateDataFlag;
        private bool isImGUIWindowOpen;
        internal bool IsImGUIWindowOpen
        {
            get { return isImGUIWindowOpen; }
        }
        private ProductionUnit[] ProductionUnits = [];
        Vector4[] ProductionUnitColors = [];
        private List<List<ResultData>> ResultDataListByProductionUnit = [];
        private float[] HeatDemand = [];
        private float[] ElectricityPrices = [];
        private float[] CollectiveCO2Emissions = [];
        private float[] CollectiveProductionCosts = [];
        private List<float[]> SeparateCO2Emissions = [];
        private List<float[]> SeparateHeatDemand = [];
        private List<float[]> SeparateGasConsumption = [];
        internal ImGuiController controller;
        private ImGuiWindowFlags ImGuiWindowFlags;
        private ConfigFlags RaylibWindowFlags;
        private int FontScale = 1;
        private HeatingGrid HeatingGridData;
        private Texture2D HeatingGridTexture;
        private List<Texture2D> ProductionUnitsTextures = [];
        internal DataVisualizer(int WindowWidth, int WindowHeight, List<SourceData> Data, HeatingGrid GridData, List<ProductionUnit> ProductionUnits, List<ResultData> WriteRecords)
        {
            UpdateDataFlag = false;
            isImGUIWindowOpen = true;
            ImGuiWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoBringToFrontOnFocus;
            RaylibWindowFlags = ConfigFlags.VSyncHint | ConfigFlags.UndecoratedWindow;

            Raylib.SetConfigFlags(RaylibWindowFlags);
            Raylib.InitWindow(WindowWidth, WindowHeight, "Plot");
            Raylib.SetTargetFPS(60);

            var context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);

            controller = new ImGuiController();

            UpdateData(Data, GridData, ProductionUnits, WriteRecords);
        }

        public void UpdateData(List<SourceData> Data, HeatingGrid GridData, List<ProductionUnit> ProductionUnits, List<ResultData> WriteRecords)
        {
            this.ProductionUnits = ProductionUnits.ToArray();

            ProductionUnitColors = GenerateColors(this.ProductionUnits.Length).ToArray();

            ResultDataListByProductionUnit = SeparateResultDataListByProductionUnit(WriteRecords);
            CollectiveCO2Emissions = new float[Data.Count];
            CollectiveProductionCosts = new float[Data.Count];
            SeparateCO2Emissions = [];
            SeparateHeatDemand = new List<float[]>();
            SeparateGasConsumption = [];

            for (int i = 0; i < ProductionUnits.Count; i++)
            {
                SeparateCO2Emissions.Add(new float[Data.Count]);
                SeparateHeatDemand.Add(new float[Data.Count]);
                SeparateGasConsumption.Add(new float[Data.Count]);
            }

            for (int i = 0; i < Data.Count; i++)
            {
                for (int y = 0; y < ProductionUnits.Count; y++)
                {
                    CollectiveCO2Emissions[i] += (float)ResultDataListByProductionUnit[y][i].ProducedCO2;
                    CollectiveProductionCosts[i] += (float)ResultDataListByProductionUnit[y][i].ProductionCosts;
                    SeparateCO2Emissions[y][i] = (float)ResultDataListByProductionUnit[y][i].ProducedCO2;
                    SeparateHeatDemand[y][i] = (float)ResultDataListByProductionUnit[y][i].ProducedHeat;
                    SeparateGasConsumption[y][i] = (float)ResultDataListByProductionUnit[y][i].PrimaryEnergyConsumption;
                }
            }

            HeatDemand = new float[Data.Count];
            ElectricityPrices = new float[Data.Count];

            for (int i = 0; i < Data.Count; i++)
            {
                HeatDemand[i] = (float)Data[i].HeatDemand;
                ElectricityPrices[i] = (float)Data[i].ElectricityPrice;
            }

            HeatingGridData = GridData;
            ProductionUnitsTextures = [];

            HeatingGridTexture = LoadImage(HeatingGridData.ImagePath);
            for (int i = 0; i < ProductionUnits.Count; i++)
            {
                ProductionUnitsTextures.Add(LoadImage(ProductionUnits[i].ImagePath));
            }
        }

        private List<List<ResultData>> SeparateResultDataListByProductionUnit(List<ResultData> ResultDataList)
        {
            List<List<ResultData>> SeparatedListByProductionUnit = new List<List<ResultData>>();
            List<List<ResultData>> SeparatedList = new List<List<ResultData>>();
            List<ResultData> CurrentBoilerStack = new List<ResultData>();
            DateTime CurrentUpperTimeLimit = ResultDataList[0].TimeTo;

            foreach (var SamplePoint in ResultDataList)
            {
                if (SamplePoint.TimeFrom == CurrentUpperTimeLimit)
                {
                    SeparatedList.Add(CurrentBoilerStack);
                    CurrentUpperTimeLimit = SamplePoint.TimeTo;
                    CurrentBoilerStack = new List<ResultData>();
                }
                CurrentBoilerStack.Add(SamplePoint);
            }

            SeparatedList.Add(CurrentBoilerStack);

            foreach (List<ResultData> BoilerStack in SeparatedList)
            {
                List<ResultData> FilledBoilerStack = BoilerStack;
                foreach (ProductionUnit ProductionUnit in ProductionUnits)
                {
                    if (!FilledBoilerStack.Any(Boiler => Boiler.ProductionUnitName == ProductionUnit.Name))
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

                    for (int i = 0; i < SeparatedListByProductionUnit.Count; i++)
                    {
                        if (SeparatedListByProductionUnit[i].Any(ProductionUnit => ProductionUnit.ProductionUnitName == Boiler.ProductionUnitName))
                        {
                            if (SeparatedListByProductionUnit.Count == 1)
                            {
                                SeparatedListByProductionUnit[i][0] = Boiler;
                            }
                            SeparatedListByProductionUnit[i].Add(Boiler);
                            HasFoundBoiler = true;
                            i = SeparatedListByProductionUnit.Count;
                        }
                    }

                    if (!HasFoundBoiler)
                    {
                        SeparatedListByProductionUnit.Add([Boiler]);
                    }
                }
            }

            return SeparatedListByProductionUnit;
        }

        public void Render()
        {
            controller.NewFrame();
            controller.ProcessEvent();

            ImGui.NewFrame();
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.SetNextWindowSize(new Vector2(Raylib.GetRenderWidth(), Raylib.GetRenderHeight()));

            ImGui.Begin("HeatSync", ref isImGUIWindowOpen, ImGuiWindowFlags);


            Raylib.SetWindowSize((int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y);

            InputHandling();

            ImGui.BeginTabBar("Main");
            if (ImGui.BeginTabItem("Source Data"))
            {
                RenderPlotLines(HeatDemand, "Heat Demand");
                RenderPlotLines(ElectricityPrices, "Electricity Prices");
                if (ImGui.Button("Update and optimize data", new Vector2(ImGui.CalcTextSize("Update and optimize data").X + 32, ImGui.CalcTextSize("Update and optimize data").Y + 8)))
                {
                    UpdateDataFlag = true;
                }

                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Result Data"))
            {
                ImGui.BeginTabBar("Settings#left_tabs_bar");
                if (ImGui.BeginTabItem("General Information"))
                {
                    //RenderPlotLines(HeatDemand, "Heat Production");
                    RenderPlotMultipleHistogram(SeparateHeatDemand.ToArray(), "Segmented Heat Production");
                    RenderPlotHistogram(CollectiveCO2Emissions, "Collective CO2 Emissions");
                    RenderTable(CollectiveProductionCosts, "Fuel expenses", 3);

                    ImGui.EndTabItem();
                }

                for (int i = 0; i < ProductionUnits.Length; i++)
                {
                    if (ImGui.BeginTabItem(ResultDataListByProductionUnit[i][0].ProductionUnitName))
                    {
                        RenderPlotLines(SeparateHeatDemand[i], "Heat Production");
                        RenderTable(SeparateGasConsumption[i], "Fuel Consumption", 3);
                        RenderTable(SeparateCO2Emissions[i], "CO2 Emissions", 3);
                        DrawTexture(ProductionUnitsTextures[i]);

                        ImGui.EndTabItem();
                    }
                }

                ImGui.EndTabBar();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Settings"))
            {
                ImGui.BeginTabBar("Production Units");

                if (ImGui.BeginTabItem("Visualizer settings"))
                {
                    ImGui.SliderInt("Font scale:", ref FontScale, 1, 5);
                    ImGui.SetWindowFontScale(FontScale);
                    ImGui.ShowStyleEditor();
                    ImGui.EndTabItem();
                }

                for (int i = 0; i < ProductionUnits.Length; i++)
                {
                    if (ImGui.BeginTabItem(ProductionUnits[i].Name))
                    {
                        DrawTexture(ProductionUnitsTextures[i]);
                        ImGui.LabelText(ProductionUnits[i].Name, "Production unit name:");
                        ImGui.LabelText(ProductionUnits[i].MaxHeat.ToString(), "Max heat production:");
                        ImGui.LabelText(ProductionUnits[i].MaxElectricity.ToString(), "Max electricity consumption/production:");
                        ImGui.LabelText(ProductionUnits[i].ProductionCosts.ToString(), "Max production cost:");
                        ImGui.LabelText(ProductionUnits[i].CO2Emissions.ToString(), "Max CO2 production:");
                        ImGui.LabelText(ProductionUnits[i].GasConsumption.ToString(), "Max fuel consumption:");

                        ImGui.EndTabItem();
                    }
                }

                if (ImGui.BeginTabItem("Heating Grid"))
                {
                    ImGui.LabelText(HeatingGridData.City, "Heating grid name:");
                    ImGui.LabelText(HeatingGridData.Size, "Heating grid size:");
                    ImGui.LabelText(HeatingGridData.Architecture, "Heating grid architecture:");
                    DrawTexture(HeatingGridTexture);

                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
            ImGui.End();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blank);

            ImGui.Render();
            controller.Render(ImGui.GetDrawData());

            Raylib.EndDrawing();
        }

        private void RenderTable(float[] samples, string Label, int Width)
        {
            if (ImGui.CollapsingHeader(Label))
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
                ImGui.PlotLines("", ref samples[0], samples.Length, 0, Label, samples.Min(), samples.Max() + samples.Max() / 10, new Vector2(ImGui.GetWindowSize().X - 16, 250));
            }
        }

        private void RenderPlotMultipleHistogram(float[][] SampleArray, string Label)
        {
            ImGuiStylePtr CurrentStyle = ImGui.GetStyle();
            var CurrentTextColor = CurrentStyle.Colors[(int)ImGuiCol.Text];
            var CurrentPlotColor = CurrentStyle.Colors[(int)ImGuiCol.PlotHistogram];
            var CurrentFrameBGColor = CurrentStyle.Colors[(int)ImGuiCol.FrameBg];
            var CurrentFrameBGHoveredColor = CurrentStyle.Colors[(int)ImGuiCol.FrameBgHovered];
            var CurrentFrameBGActiveColor = CurrentStyle.Colors[(int)ImGuiCol.FrameBgActive];
            

            if (ImGui.CollapsingHeader(Label, ImGuiTreeNodeFlags.DefaultOpen))
            {
                float[] RenderedSamples = (float[])HeatDemand.Clone();
                Vector2 CursorPos = ImGui.GetCursorPos();

                //ImGui.PlotHistogram("", ref RenderedSamples[0], RenderedSamples.Length, 0, Label, HeatDemand.Min(), HeatDemand.Max() + HeatDemand.Max() / 10, new Vector2(ImGui.GetWindowSize().X - 16, 250));

                //CurrentStyle.Colors[(int)ImGuiCol.FrameBg] = Vector4.Zero;
                CurrentStyle.Colors[(int)ImGuiCol.FrameBgHovered] = Vector4.Zero;
                CurrentStyle.Colors[(int)ImGuiCol.FrameBgActive] = Vector4.Zero;

                for (int i = 0; i < SampleArray.Length; i++)
                {
                    CurrentStyle.Colors[(int)ImGuiCol.PlotHistogram] = ProductionUnitColors[i];
                    ImGui.SetCursorPos(CursorPos);

                    ImGui.PlotHistogram("", ref RenderedSamples[0], RenderedSamples.Length, 0, Label, HeatDemand.Min(), HeatDemand.Max() + HeatDemand.Max() / 10, new Vector2(ImGui.GetWindowSize().X - 16, 250));

                    for (int y = 0; y < RenderedSamples.Length; y++)
                    {
                        float value = SampleArray[i][y];
                        RenderedSamples[y] = RenderedSamples[y] - SampleArray[i][y];
                        if(RenderedSamples[y] < 0.001)
                        {
                            RenderedSamples[y] = 0;
                        }
                    }
                    CurrentStyle.Colors[(int)ImGuiCol.FrameBg] = Vector4.Zero;
                }

                CurrentStyle.Colors[(int)ImGuiCol.PlotHistogram] = CurrentPlotColor;
                CurrentStyle.Colors[(int)ImGuiCol.FrameBg] = CurrentFrameBGColor;
                CurrentStyle.Colors[(int)ImGuiCol.FrameBgHovered] = CurrentFrameBGHoveredColor;
                CurrentStyle.Colors[(int)ImGuiCol.FrameBgActive] = CurrentFrameBGActiveColor;
                
                for (int i = 0; i < ProductionUnits.Length; i++)
                {
                    CurrentStyle.Colors[(int)ImGuiCol.Text] = ProductionUnitColors[i];
                    ImGui.Text(ResultDataListByProductionUnit[i][0].ProductionUnitName);
                    ImGui.SameLine();
                }

                CurrentStyle.Colors[(int)ImGuiCol.Text] = CurrentTextColor;
                ImGui.Text("");
            }
        }

        private List<Vector4> GenerateColors(int Amount)
        {
            Random r = new Random();
            List<Vector4> Colors = new List<Vector4>();

            float CurrentHue = 0f;

            for (int i = 0; i < Amount; i++)
            {
                //Golden Angle
                var CurrentColor = Raylib.ColorFromHSV(CurrentHue, 1, 1f);
                Colors.Add((Raylib.ColorNormalize(CurrentColor) + Vector4.One) / 2);
                CurrentHue += 137.5f;
            }

            return Colors;
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

            Raylib.ImageDrawPixel(ref image, 0, 0, Color.RayWhite);
            Texture2D texture = Raylib.LoadTextureFromImage(image);

            Raylib.UnloadImage(image);

            return texture;
        }
        
        private void DrawTexture(Texture2D Texture)
        {
            if (ImGui.CollapsingHeader("Image", ImGuiTreeNodeFlags.DefaultOpen | ImGuiTreeNodeFlags.Leaf))
            {
                ImGui.Image((nint)Texture.Id, new Vector2(Texture.Width, Texture.Height));
            }
        }
    }
}

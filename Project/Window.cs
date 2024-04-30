using ImGuiNET;
using System.Numerics;
using Raylib_cs;

namespace HeatItOn
{
    internal class Window
    {
        private bool isImGUIWindowOpen;

        internal bool IsImGUIWindowOpen
        {
            get { return isImGUIWindowOpen; }
        }


        private ImGuiController controller;
        private ImGuiWindowFlags ImGuiWindowFlags;
        private ConfigFlags RaylibWindowFlags;

        internal Window()
        {
            Init();
            Render();
        }
        internal void Init()
        {
            isImGUIWindowOpen = true;
            ImGuiWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoBringToFrontOnFocus;
            RaylibWindowFlags = ConfigFlags.VSyncHint | ConfigFlags.UndecoratedWindow;

            Raylib.SetConfigFlags(RaylibWindowFlags);
            Raylib.InitWindow(1280, 720, "Plot");
            Raylib.SetTargetFPS(60);
            
            var context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);

            controller = new ImGuiController();
        }
        internal void Render()
        {
            controller.NewFrame();
            controller.ProcessEvent();

            InputHandling();

            ImGui.NewFrame();
            ImGui.SetNextWindowPos(new Vector2(0, 0));

            ImGui.Begin("Test", ref isImGUIWindowOpen, ImGuiWindowFlags);
            Raylib.SetWindowSize((int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y);

            ImGui.Text(Raylib.GetFPS().ToString());

            ImGui.End();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blank);

            ImGui.Render();
            controller.Render(ImGui.GetDrawData());
            Raylib.EndDrawing();

        }

        private void InputHandling()
        {
            if (ImGui.IsMouseDragging(ImGuiMouseButton.Left) && ImGui.GetMousePos().Y <= 20 + ImGui.GetMouseDragDelta().Y)
            {
                var CurrentWindowPosition = Raylib.GetWindowPosition();
                Raylib.SetWindowPosition((int)(CurrentWindowPosition.X + ImGui.GetMouseDragDelta().X), (int)(CurrentWindowPosition.Y + ImGui.GetMouseDragDelta().Y));
            }
        }
    }
}

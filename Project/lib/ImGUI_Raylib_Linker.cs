using ImGuiNET;
using Raylib_cs;
using System.Numerics;
using System.Runtime.InteropServices;

namespace HeatSync
{
    internal unsafe class ImGuiController
    {

        private Vector2 mousePosition;
        private Vector2 displaySize;
        private float delta;
        private bool isKeyCtrl;
        private bool isKeyShift;
        private bool isKeyAlt;
        private bool isKeySuper;

        static double g_Time = 0.0;
        static bool g_UnloadAtlas = false;
        static uint g_AtlasTexID = 0;

        static string GetClipboardText()
        {
            return Raylib.GetClipboardText_();
        }

        static void SetClipboardText(string text)
        {
            Raylib.SetClipboardText(text);
        }

        public ImGuiController()
        {
            Init();
        }

        private void Init()
        {
            var io = ImGui.GetIO();
            io.AddKeyEvent(ImGuiKey.Tab, Raylib.IsKeyDown(KeyboardKey.Tab));
            io.AddKeyEvent(ImGuiKey.LeftArrow, Raylib.IsKeyDown(KeyboardKey.Left));
            io.AddKeyEvent(ImGuiKey.RightArrow, Raylib.IsKeyDown(KeyboardKey.Right));
            io.AddKeyEvent(ImGuiKey.UpArrow, Raylib.IsKeyDown(KeyboardKey.Up));
            io.AddKeyEvent(ImGuiKey.DownArrow, Raylib.IsKeyDown(KeyboardKey.Down));
            io.AddKeyEvent(ImGuiKey.PageUp, Raylib.IsKeyDown(KeyboardKey.PageUp));
            io.AddKeyEvent(ImGuiKey.PageDown, Raylib.IsKeyDown(KeyboardKey.PageDown));
            io.AddKeyEvent(ImGuiKey.Home, Raylib.IsKeyDown(KeyboardKey.Home));
            io.AddKeyEvent(ImGuiKey.End, Raylib.IsKeyDown(KeyboardKey.End));
            io.AddKeyEvent(ImGuiKey.Insert, Raylib.IsKeyDown(KeyboardKey.Insert));
            io.AddKeyEvent(ImGuiKey.Delete, Raylib.IsKeyDown(KeyboardKey.Delete));
            io.AddKeyEvent(ImGuiKey.Backspace, Raylib.IsKeyDown(KeyboardKey.Backspace));
            io.AddKeyEvent(ImGuiKey.Space, Raylib.IsKeyDown(KeyboardKey.Space));
            io.AddKeyEvent(ImGuiKey.Enter, Raylib.IsKeyDown(KeyboardKey.Enter));
            io.AddKeyEvent(ImGuiKey.Escape, Raylib.IsKeyDown(KeyboardKey.Escape));
            io.AddKeyEvent(ImGuiKey.KeypadEnter, Raylib.IsKeyDown(KeyboardKey.KpEnter));
            io.AddKeyEvent(ImGuiKey.A, Raylib.IsKeyDown(KeyboardKey.A));
            io.AddKeyEvent(ImGuiKey.Backspace, Raylib.IsKeyDown(KeyboardKey.Tab));
            io.AddKeyEvent(ImGuiKey.C, Raylib.IsKeyDown(KeyboardKey.C));
            io.AddKeyEvent(ImGuiKey.V, Raylib.IsKeyDown(KeyboardKey.V));
            io.AddKeyEvent(ImGuiKey.X, Raylib.IsKeyDown(KeyboardKey.X));
            io.AddKeyEvent(ImGuiKey.Y, Raylib.IsKeyDown(KeyboardKey.Y));
            io.AddKeyEvent(ImGuiKey.Z, Raylib.IsKeyDown(KeyboardKey.Z));

            mousePosition = new Vector2(0, 0);
            io.MousePos = mousePosition;

            // Use this space to add more fonts
            io.Fonts.AddFontFromFileTTF("OpenSans-Regular.ttf", 16f);
            LoadDefaultFontAtlas();
        }

        public void Shutdown()
        {
            if (g_UnloadAtlas)
            {
                ImGui.GetIO().Fonts.ClearFonts();
            }
            g_Time = 0.0;
        }

        private void UpdateMousePosAndButtons()
        {
            var io = ImGui.GetIO();

            if (io.WantSetMousePos)
                Raylib.SetMousePosition((int)io.MousePos.X, (int)io.MousePos.Y);

            io.MouseDown[0] = Raylib.IsMouseButtonDown(MouseButton.Left);
            io.MouseDown[1] = Raylib.IsMouseButtonDown(MouseButton.Right);
            io.MouseDown[2] = Raylib.IsMouseButtonDown(MouseButton.Middle);


            if (!Raylib.IsWindowMinimized())
                mousePosition = new Vector2(Raylib.GetMouseX(), Raylib.GetMouseY());

            io.MousePos = mousePosition;
        }

        private void UpdateMouseCursor()
        {
            var io = ImGui.GetIO();
            if (io.ConfigFlags.HasFlag(ImGuiConfigFlags.NoMouseCursorChange))
                return;

            var imgui_cursor = ImGui.GetMouseCursor();
            if (io.MouseDrawCursor || imgui_cursor == ImGuiMouseCursor.None)
            {
                Raylib.HideCursor();
            }
            else
            {
                Raylib.ShowCursor();
            }
        }

        public void NewFrame()
        {
            var io = ImGui.GetIO();

            displaySize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            io.DisplaySize = displaySize;

            double current_time = Raylib.GetTime();
            delta = g_Time > 0.0 ? (float)(current_time - g_Time) : 1.0f / 60.0f;
            io.DeltaTime = delta;

            isKeyCtrl = Raylib.IsKeyDown(KeyboardKey.RightControl) || Raylib.IsKeyDown(KeyboardKey.LeftControl);
            isKeyShift = Raylib.IsKeyDown(KeyboardKey.RightShift) || Raylib.IsKeyDown(KeyboardKey.LeftShift);
            isKeyAlt = Raylib.IsKeyDown(KeyboardKey.RightAlt) || Raylib.IsKeyDown(KeyboardKey.LeftAlt);
            isKeySuper = Raylib.IsKeyDown(KeyboardKey.RightSuper) || Raylib.IsKeyDown(KeyboardKey.LeftSuper);

            io.KeyCtrl = isKeyCtrl;
            io.KeyAlt = isKeyAlt;
            io.KeyShift = isKeyShift;
            io.KeySuper = isKeySuper;

            UpdateMousePosAndButtons();
            UpdateMouseCursor();

            if (Raylib.GetMouseWheelMove() > 0)
            {
                io.MouseWheel += 1;
            }
            else if (Raylib.GetMouseWheelMove() < 0)
            {
                io.MouseWheel -= 1;
            }
        }

        public bool ProcessEvent()
        {
            var io = ImGui.GetIO();

            io.AddKeyEvent(ImGuiKey.Apostrophe, Raylib.IsKeyDown(KeyboardKey.Apostrophe));
            io.AddKeyEvent(ImGuiKey.Comma, Raylib.IsKeyDown(KeyboardKey.Comma));
            io.AddKeyEvent(ImGuiKey.Minus, Raylib.IsKeyDown(KeyboardKey.Minus));
            io.AddKeyEvent(ImGuiKey.Period, Raylib.IsKeyDown(KeyboardKey.Period));
            io.AddKeyEvent(ImGuiKey.Slash, Raylib.IsKeyDown(KeyboardKey.Slash));
            io.AddKeyEvent(ImGuiKey._0, Raylib.IsKeyDown(KeyboardKey.Zero));
            io.AddKeyEvent(ImGuiKey._1, Raylib.IsKeyDown(KeyboardKey.One));
            io.AddKeyEvent(ImGuiKey._2, Raylib.IsKeyDown(KeyboardKey.Two));
            io.AddKeyEvent(ImGuiKey._3, Raylib.IsKeyDown(KeyboardKey.Three));
            io.AddKeyEvent(ImGuiKey._4, Raylib.IsKeyDown(KeyboardKey.Four));
            io.AddKeyEvent(ImGuiKey._5, Raylib.IsKeyDown(KeyboardKey.Five));
            io.AddKeyEvent(ImGuiKey._6, Raylib.IsKeyDown(KeyboardKey.Six));
            io.AddKeyEvent(ImGuiKey._7, Raylib.IsKeyDown(KeyboardKey.Seven));
            io.AddKeyEvent(ImGuiKey._8, Raylib.IsKeyDown(KeyboardKey.Eight));
            io.AddKeyEvent(ImGuiKey._9, Raylib.IsKeyDown(KeyboardKey.Nine));
            io.AddKeyEvent(ImGuiKey.Semicolon, Raylib.IsKeyDown(KeyboardKey.Semicolon));
            io.AddKeyEvent(ImGuiKey.Equal, Raylib.IsKeyDown(KeyboardKey.Equal));
            io.AddKeyEvent(ImGuiKey.A, Raylib.IsKeyDown(KeyboardKey.A));
            io.AddKeyEvent(ImGuiKey.B, Raylib.IsKeyDown(KeyboardKey.B));
            io.AddKeyEvent(ImGuiKey.C, Raylib.IsKeyDown(KeyboardKey.C));
            io.AddKeyEvent(ImGuiKey.D, Raylib.IsKeyDown(KeyboardKey.D));
            io.AddKeyEvent(ImGuiKey.E, Raylib.IsKeyDown(KeyboardKey.E));
            io.AddKeyEvent(ImGuiKey.F, Raylib.IsKeyDown(KeyboardKey.F));
            io.AddKeyEvent(ImGuiKey.G, Raylib.IsKeyDown(KeyboardKey.G));
            io.AddKeyEvent(ImGuiKey.H, Raylib.IsKeyDown(KeyboardKey.H));
            io.AddKeyEvent(ImGuiKey.I, Raylib.IsKeyDown(KeyboardKey.I));
            io.AddKeyEvent(ImGuiKey.J, Raylib.IsKeyDown(KeyboardKey.J));
            io.AddKeyEvent(ImGuiKey.K, Raylib.IsKeyDown(KeyboardKey.K));
            io.AddKeyEvent(ImGuiKey.L, Raylib.IsKeyDown(KeyboardKey.L));
            io.AddKeyEvent(ImGuiKey.M, Raylib.IsKeyDown(KeyboardKey.M));
            io.AddKeyEvent(ImGuiKey.N, Raylib.IsKeyDown(KeyboardKey.N));
            io.AddKeyEvent(ImGuiKey.O, Raylib.IsKeyDown(KeyboardKey.O));
            io.AddKeyEvent(ImGuiKey.P, Raylib.IsKeyDown(KeyboardKey.P));
            io.AddKeyEvent(ImGuiKey.Q, Raylib.IsKeyDown(KeyboardKey.Q));
            io.AddKeyEvent(ImGuiKey.R, Raylib.IsKeyDown(KeyboardKey.R));
            io.AddKeyEvent(ImGuiKey.S, Raylib.IsKeyDown(KeyboardKey.S));
            io.AddKeyEvent(ImGuiKey.T, Raylib.IsKeyDown(KeyboardKey.T));
            io.AddKeyEvent(ImGuiKey.U, Raylib.IsKeyDown(KeyboardKey.U));
            io.AddKeyEvent(ImGuiKey.V, Raylib.IsKeyDown(KeyboardKey.V));
            io.AddKeyEvent(ImGuiKey.W, Raylib.IsKeyDown(KeyboardKey.W));
            io.AddKeyEvent(ImGuiKey.X, Raylib.IsKeyDown(KeyboardKey.X));
            io.AddKeyEvent(ImGuiKey.Y, Raylib.IsKeyDown(KeyboardKey.Y));
            io.AddKeyEvent(ImGuiKey.Z, Raylib.IsKeyDown(KeyboardKey.Z));
            io.AddKeyEvent(ImGuiKey.C, Raylib.IsKeyDown(KeyboardKey.C));
            io.AddKeyEvent(ImGuiKey.D, Raylib.IsKeyDown(KeyboardKey.D));
            io.AddKeyEvent(ImGuiKey.Space, Raylib.IsKeyDown(KeyboardKey.Space));
            io.AddKeyEvent(ImGuiKey.Escape, Raylib.IsKeyDown(KeyboardKey.Escape));
            io.AddKeyEvent(ImGuiKey.Enter, Raylib.IsKeyDown(KeyboardKey.Enter));
            io.AddKeyEvent(ImGuiKey.Tab, Raylib.IsKeyDown(KeyboardKey.Tab));
            io.AddKeyEvent(ImGuiKey.Backspace, Raylib.IsKeyDown(KeyboardKey.Backspace));
            io.AddKeyEvent(ImGuiKey.Insert, Raylib.IsKeyDown(KeyboardKey.Insert));
            io.AddKeyEvent(ImGuiKey.Delete, Raylib.IsKeyDown(KeyboardKey.Delete));
            io.AddKeyEvent(ImGuiKey.RightArrow, Raylib.IsKeyDown(KeyboardKey.Right));
            io.AddKeyEvent(ImGuiKey.LeftArrow, Raylib.IsKeyDown(KeyboardKey.Left));
            io.AddKeyEvent(ImGuiKey.UpArrow, Raylib.IsKeyDown(KeyboardKey.Up));
            io.AddKeyEvent(ImGuiKey.DownArrow, Raylib.IsKeyDown(KeyboardKey.Down));
            io.AddKeyEvent(ImGuiKey.PageUp, Raylib.IsKeyDown(KeyboardKey.PageUp));
            io.AddKeyEvent(ImGuiKey.PageDown, Raylib.IsKeyDown(KeyboardKey.PageDown));
            io.AddKeyEvent(ImGuiKey.Home, Raylib.IsKeyDown(KeyboardKey.Home));
            io.AddKeyEvent(ImGuiKey.End, Raylib.IsKeyDown(KeyboardKey.End));
            io.AddKeyEvent(ImGuiKey.CapsLock, Raylib.IsKeyDown(KeyboardKey.CapsLock));
            io.AddKeyEvent(ImGuiKey.ScrollLock, Raylib.IsKeyDown(KeyboardKey.ScrollLock));
            io.AddKeyEvent(ImGuiKey.NumLock, Raylib.IsKeyDown(KeyboardKey.NumLock));
            io.AddKeyEvent(ImGuiKey.PrintScreen, Raylib.IsKeyDown(KeyboardKey.PrintScreen));
            io.AddKeyEvent(ImGuiKey.Pause, Raylib.IsKeyDown(KeyboardKey.Pause));
            io.AddKeyEvent(ImGuiKey.F1, Raylib.IsKeyDown(KeyboardKey.F1));
            io.AddKeyEvent(ImGuiKey.F2, Raylib.IsKeyDown(KeyboardKey.F2));
            io.AddKeyEvent(ImGuiKey.F3, Raylib.IsKeyDown(KeyboardKey.F3));
            io.AddKeyEvent(ImGuiKey.F4, Raylib.IsKeyDown(KeyboardKey.F4));
            io.AddKeyEvent(ImGuiKey.F5, Raylib.IsKeyDown(KeyboardKey.F5));
            io.AddKeyEvent(ImGuiKey.F6, Raylib.IsKeyDown(KeyboardKey.F6));
            io.AddKeyEvent(ImGuiKey.F7, Raylib.IsKeyDown(KeyboardKey.F7));
            io.AddKeyEvent(ImGuiKey.F8, Raylib.IsKeyDown(KeyboardKey.F8));
            io.AddKeyEvent(ImGuiKey.F9, Raylib.IsKeyDown(KeyboardKey.F9));
            io.AddKeyEvent(ImGuiKey.F10, Raylib.IsKeyDown(KeyboardKey.F10));
            io.AddKeyEvent(ImGuiKey.F11, Raylib.IsKeyDown(KeyboardKey.F11));
            io.AddKeyEvent(ImGuiKey.F12, Raylib.IsKeyDown(KeyboardKey.F12));
            io.AddKeyEvent(ImGuiKey.LeftShift, Raylib.IsKeyDown(KeyboardKey.LeftShift));
            io.AddKeyEvent(ImGuiKey.LeftCtrl, Raylib.IsKeyDown(KeyboardKey.LeftControl));
            io.AddKeyEvent(ImGuiKey.LeftAlt, Raylib.IsKeyDown(KeyboardKey.LeftAlt));
            io.AddKeyEvent(ImGuiKey.LeftSuper, Raylib.IsKeyDown(KeyboardKey.LeftSuper));
            io.AddKeyEvent(ImGuiKey.RightShift, Raylib.IsKeyDown(KeyboardKey.RightShift));
            io.AddKeyEvent(ImGuiKey.RightCtrl, Raylib.IsKeyDown(KeyboardKey.RightControl));
            io.AddKeyEvent(ImGuiKey.RightAlt, Raylib.IsKeyDown(KeyboardKey.RightAlt));
            io.AddKeyEvent(ImGuiKey.RightSuper, Raylib.IsKeyDown(KeyboardKey.RightSuper));
            io.AddKeyEvent(ImGuiKey.PageDown, Raylib.IsKeyDown(KeyboardKey.PageDown));
            io.AddKeyEvent(ImGuiKey.Menu, Raylib.IsKeyDown(KeyboardKey.KeyboardMenu));
            io.AddKeyEvent(ImGuiKey.LeftBracket, Raylib.IsKeyDown(KeyboardKey.LeftBracket));
            io.AddKeyEvent(ImGuiKey.RightBracket, Raylib.IsKeyDown(KeyboardKey.RightBracket));
            io.AddKeyEvent(ImGuiKey.Backslash, Raylib.IsKeyDown(KeyboardKey.Backslash));
            io.AddKeyEvent(ImGuiKey.GraveAccent, Raylib.IsKeyDown(KeyboardKey.Grave));
            io.AddKeyEvent(ImGuiKey.Keypad0, Raylib.IsKeyDown(KeyboardKey.Kp0));
            io.AddKeyEvent(ImGuiKey.Keypad1, Raylib.IsKeyDown(KeyboardKey.Kp1));
            io.AddKeyEvent(ImGuiKey.Keypad2, Raylib.IsKeyDown(KeyboardKey.Kp2));
            io.AddKeyEvent(ImGuiKey.Keypad3, Raylib.IsKeyDown(KeyboardKey.Kp3));
            io.AddKeyEvent(ImGuiKey.Keypad4, Raylib.IsKeyDown(KeyboardKey.Kp4));
            io.AddKeyEvent(ImGuiKey.Keypad5, Raylib.IsKeyDown(KeyboardKey.Kp5));
            io.AddKeyEvent(ImGuiKey.Keypad6, Raylib.IsKeyDown(KeyboardKey.Kp6));
            io.AddKeyEvent(ImGuiKey.Keypad7, Raylib.IsKeyDown(KeyboardKey.Kp7));
            io.AddKeyEvent(ImGuiKey.Keypad8, Raylib.IsKeyDown(KeyboardKey.Kp8));
            io.AddKeyEvent(ImGuiKey.Keypad9, Raylib.IsKeyDown(KeyboardKey.Kp9));
            io.AddKeyEvent(ImGuiKey.KeypadDecimal, Raylib.IsKeyDown(KeyboardKey.KpDecimal));
            io.AddKeyEvent(ImGuiKey.KeypadDivide, Raylib.IsKeyDown(KeyboardKey.KpDivide));
            io.AddKeyEvent(ImGuiKey.KeypadMultiply, Raylib.IsKeyDown(KeyboardKey.KpMultiply));
            io.AddKeyEvent(ImGuiKey.KeypadSubtract, Raylib.IsKeyDown(KeyboardKey.KpSubtract));
            io.AddKeyEvent(ImGuiKey.KeypadAdd, Raylib.IsKeyDown(KeyboardKey.KpAdd));
            io.AddKeyEvent(ImGuiKey.KeypadEnter, Raylib.IsKeyDown(KeyboardKey.KpEnter));
            io.AddKeyEvent(ImGuiKey.KeypadEqual, Raylib.IsKeyDown(KeyboardKey.KpEqual));
            io.AddKeyEvent(ImGuiKey.Menu, Raylib.IsKeyDown(KeyboardKey.KeyboardMenu));

            int length = 0;
            io.AddInputCharactersUTF8(Raylib.CodepointToUTF8(Raylib.GetCharPressed(), ref length));

            return true;
        }

        void LoadDefaultFontAtlas()
        {
            if (!g_UnloadAtlas)
            {
                var io = ImGui.GetIO();
                byte* pixels;
                int width, height, bpp;
                Image image;

                io.Fonts.GetTexDataAsRGBA32(out pixels, out width, out height, out bpp);
                var size = Raylib.GetPixelDataSize(width, height, PixelFormat.UncompressedR8G8B8A8);
                image.Data = (void*)Marshal.AllocHGlobal(size);
                Buffer.MemoryCopy(pixels, image.Data, size, size);
                image.Width = width;
                image.Height = height;
                image.Mipmaps = 1;
                image.Format = PixelFormat.UncompressedR8G8B8A8;
                var tex = Raylib.LoadTextureFromImage(image);
                g_AtlasTexID = tex.Id;
                io.Fonts.TexID = (IntPtr)g_AtlasTexID;
                Marshal.FreeHGlobal((IntPtr)pixels);
                Marshal.FreeHGlobal((IntPtr)image.Data);
                g_UnloadAtlas = true;
            }
        }

        public void Render(ImDrawDataPtr draw_data)
        {
            Rlgl.DisableBackfaceCulling();
            for (int n = 0; n < draw_data.CmdListsCount; n++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdLists[n];
                uint idx_index = 0;
                for (int i = 0; i < cmd_list.CmdBuffer.Size; i++)
                {
                    var pcmd = cmd_list.CmdBuffer[i];
                    var pos = draw_data.DisplayPos;
                    var rectX = (int)(pcmd.ClipRect.X - pos.X);
                    var rectY = (int)(pcmd.ClipRect.Y - pos.Y);
                    var rectW = (int)(pcmd.ClipRect.Z - rectX);
                    var rectH = (int)(pcmd.ClipRect.W - rectY);
                    Raylib.BeginScissorMode(rectX, rectY, rectW, rectH);
                    {
                        var ti = pcmd.TextureId;
                        for (int j = 0; j <= (pcmd.ElemCount - 3); j += 3)
                        {
                            if (pcmd.ElemCount == 0)
                            {
                                break;
                            }

                            Rlgl.PushMatrix();
                            Rlgl.Begin(DrawMode.Triangles);
                            Rlgl.SetTexture((uint)ti.ToInt32());

                            ImDrawVertPtr vertex;
                            ushort index;

                            index = cmd_list.IdxBuffer[(int)(j + idx_index)];
                            vertex = cmd_list.VtxBuffer[index];
                            DrawTriangleVertex(vertex);

                            index = cmd_list.IdxBuffer[(int)(j + 2 + idx_index)];
                            vertex = cmd_list.VtxBuffer[index];
                            DrawTriangleVertex(vertex);

                            index = cmd_list.IdxBuffer[(int)(j + 1 + idx_index)];
                            vertex = cmd_list.VtxBuffer[index];
                            DrawTriangleVertex(vertex);

                            Rlgl.DisableTexture();
                            Rlgl.End();
                            Rlgl.PopMatrix();
                        }
                    }

                    idx_index += pcmd.ElemCount;
                }
            }

            Raylib.EndScissorMode();
            Rlgl.EnableBackfaceCulling();
        }

        void DrawTriangleVertex(ImDrawVertPtr idx_vert)
        {
            Color c = new Color((byte)(idx_vert.col >> 0), (byte)(idx_vert.col >> 8), (byte)(idx_vert.col >> 16), (byte)(idx_vert.col >> 24));
            Rlgl.Color4ub(c.R, c.G, c.B, c.A);
            Rlgl.TexCoord2f(idx_vert.uv.X, idx_vert.uv.Y);
            Rlgl.Vertex2f(idx_vert.pos.X, idx_vert.pos.Y);
        }

    }
}

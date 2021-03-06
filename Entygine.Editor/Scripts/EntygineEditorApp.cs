﻿using Entygine;
using Entygine.DevTools;
using Entygine.Input;
using Entygine.Rendering;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Entygine_Editor
{
    internal static class EntygineEditorApp
    {
        private static MainEditorWindow mainWindow;
        private static EditorDrawer mainEditorDrawer = new EditorDrawer();
        private static EntyImGui imgui;

        internal static void StartEditor()
        {
            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();

            gameWindowSettings.UpdateFrequency = 60.0d;

            nativeWindowSettings.Title = "Entygine Editor";
            nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(1600, 900);

            mainWindow = new MainEditorWindow(gameWindowSettings, nativeWindowSettings);
            mainWindow.WindowState = WindowState.Maximized;
            mainWindow.CenterWindow();

            mainWindow.Load += LoadEditor;
            mainWindow.UpdateFrame += UpdateEditor;
            mainWindow.RenderFrame += RenderEditor;

            mainWindow.Run();
            mainWindow.Dispose();
        }

        private static void LoadEditor()
        {
            imgui = new EntyImGui(mainWindow.Size.X, mainWindow.Size.Y);
            var style = ImGui.GetStyle();
            style.WindowRounding = 0;
            style.AntiAliasedLines = true;

            mainEditorDrawer.AttachDrawer(new MainMenuBarDrawer());
            mainEditorDrawer.AttachDrawer(new AssetsWindow());
            mainEditorDrawer.AttachDrawer(new ClientWindow());
            mainEditorDrawer.AttachDrawer(new ConsoleWindow());
            mainEditorDrawer.AttachDrawer(new WorldWindow());
            mainEditorDrawer.AttachDrawer(new DetailsWindow());

            EntygineApp.LoadEngine();

            DevConsole.Log(LogType.Info, "Editor started succesfully.");
        }

        private static void RenderEditor(FrameEventArgs e)
        {
            imgui.Update(mainWindow.KeyboardState, mainWindow.MouseState, (float)e.Time);

            Ogl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            Ogl.ClearColor(0.1f, 0.1f, 0.1f, 255);

            EntygineApp.RenderFrame(e);

            Ogl.Viewport(0, 0, mainWindow.Size.X, mainWindow.Size.Y);
            imgui.WindowResized(mainWindow.Size.X, mainWindow.Size.Y);

            mainEditorDrawer.Draw();

            imgui.Render();
        }

        private static void UpdateEditor(FrameEventArgs e)
        {
            AppInput.SetFrameInput(new InputState(mainWindow.KeyboardState, mainWindow.MouseState));
            EntygineApp.UpdateFrame(e);
        }
    }
}
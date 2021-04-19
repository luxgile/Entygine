using Entygine;
using Entygine.DevTools;
using Entygine.Ecs;
using Entygine.Ecs.Components;
using Entygine.Rendering;
using Entygine_Editor.ImGUI;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entygine_Editor
{
    internal static class EntygineEditorApp
    {
        private static ConsoleWindow consoleWindow = new ConsoleWindow();
        private static MainEditorWindow mainWindow;
        private static EntyImGui imgui;
        private static int frameBuffer;
        private static int texture;

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

            DevConsole.AddLogger(consoleWindow);

            EntygineApp.LoadEngine();

            frameBuffer = Ogl.GenFramebuffer("Editor");

            texture = Ogl.GenTexture("Editor Texture");
            Ogl.BindTexture(TextureTarget.Texture2D, texture);

            Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 512, 512, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureMinFilter.Linear);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureMagFilter.Linear);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);
            Ogl.DrawBuffer(DrawBufferMode.ColorAttachment0);
            Ogl.ReadBuffer(ReadBufferMode.ColorAttachment0);
            DevConsole.Log(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));

            DevConsole.Log("Editor started succesfully.");
        }

        private static void RenderEditor(FrameEventArgs e)
        {
            imgui.Update(mainWindow.KeyboardState, mainWindow.MouseState, (float)e.Time);

            Ogl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            Ogl.ClearColor(0.1f, 0.1f, 0.1f, 255);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            EntygineApp.RenderFrame(e);
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            Ogl.Viewport(0, 0, AppScreen.Resolution.x, AppScreen.Resolution.y);
            imgui.WindowResized(AppScreen.Resolution.x, AppScreen.Resolution.y);

            consoleWindow.Draw();

            EntityIterator.PerformIteration(EntityWorld.Active, new RenderCamera(), new EntityQuery().Any(TypeCache.ReadType<C_Camera>()));

            imgui.Render();
        }

        private static void UpdateEditor(FrameEventArgs e)
        {
            EntygineApp.UpdateFrame(e);
        }

        private struct RenderCamera : IQueryEntityIterator
        {
            public void Iteration(ref EntityChunk chunk, int index)
            {
                chunk.TryGetComponent(index, out C_Camera camera);
                ImGui.Begin("Render", ImGuiWindowFlags.NoCollapse);
                ImGui.Image((IntPtr)camera.cameraData.ColorTargetTexture.handle, new Vector2(800, 600), new Vector2(0, 0), new Vector2(1, -1));
                ImGui.End();
            }
        }

        private class ConsoleWindow : IConsoleLogger
        {
            List<string> logs = new List<string>();
            bool showstyle;
            public void Log(object log)
            {
                logs.Add(log.ToString());
            }

            public void Clear()
            {
                logs.Clear();
            }

            internal void Draw()
            {
                ImGui.DockSpaceOverViewport();
                ImGui.BeginMainMenuBar();
                if (ImGui.BeginMenu("Windows"))
                {
                    if (ImGui.MenuItem("Style Editor"))
                        showstyle = !showstyle;

                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();

                if (showstyle)
                    ImGui.ShowStyleEditor();

                ImGui.Begin("Assets");
                ImGui.End();

                ImGui.Begin("World");
                ImGui.End();

                ImGui.Begin("Details");
                ImGui.End();

                ImGui.Begin("Console", ImGuiWindowFlags.NoCollapse);
                for (int i = 0; i < logs.Count; i++)
                {
                    ImGui.Text(logs[i]);
                }
                ImGui.End();
            }
        }
    }
}
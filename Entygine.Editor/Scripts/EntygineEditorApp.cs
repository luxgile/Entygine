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

namespace Entygine_Editor
{
    internal static class EntygineEditorApp
    {
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

            mainWindow.Load += LoadEditor;
            mainWindow.UpdateFrame += UpdateEditor;
            mainWindow.RenderFrame += RenderEditor;

            mainWindow.Run();
            mainWindow.Dispose();
        }

        private static void LoadEditor()
        {
            imgui = new EntyImGui(mainWindow.Size.X, mainWindow.Size.Y);

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
            ImGui.ShowDemoWindow();

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
                ImGui.Begin("Render");
                ImGui.Image((IntPtr)camera.cameraData.ColorTargetTexture.handle, new System.Numerics.Vector2(800, 600));
                ImGui.End();
            }
        }
    }
}
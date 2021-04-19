using Entygine;
using Entygine.Cycles;
using Entygine.Ecs;
using Entygine.Rendering;
using Entygine_Editor.ImGUI;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Entygine_Editor
{
    public class MainEditorWindow : GameWindow
    {

        public MainEditorWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Window = this;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            AppScreen.Resolution = new Entygine.Mathematics.Vec2i(Size.X, Size.Y);

            Ogl.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Close();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            SwapBuffers();
        }

        public static MainEditorWindow Window { get; private set; }
    }
}

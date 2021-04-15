using Entygine.Cycles;
using Entygine.Ecs;
using Entygine.Rendering;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Entygine.Editor
{
    public class MainEditorWindow : GameWindow
    {
        private WorkerCycleCore editorWorker;

        public MainEditorWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Window = this;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Ogl.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //FrameContext.Current = new FrameData(FrameContext.Current.count + 1, (float)e.Time);

            //editorWorker.PerformLogicCycle((float)e.Time);

            //if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
            //    EntityWorld.Active.DEBUG_LOG_INFO();

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Close();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //editorWorker.PerformRenderCycle((float)e.Time);

            SwapBuffers();
        }

        public static MainEditorWindow Window { get; private set; }
    }
}

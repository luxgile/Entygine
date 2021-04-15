using Entygine.Rendering;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Entygine
{
    public class MainDevWindowGL : GameWindow
    {
        public MainDevWindowGL(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Window = this;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            AppScreen.Resolution = new Mathematics.Vec2i(Size.X, Size.Y);

            Ogl.Viewport(0, 0, Size.X, Size.Y);
        }

        public static MainDevWindowGL Window { get; private set; }
    }
}

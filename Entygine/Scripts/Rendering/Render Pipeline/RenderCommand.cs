using OpenTK.Graphics.OpenGL4;
using System;

namespace Entygine.Rendering
{
    public delegate void RenderCommandAction(ref RenderContext context);

    public class RenderCommand
    {
        private string name;
        private RenderCommandAction renderAction;

        public RenderCommand(string name, RenderCommandAction renderAction)
        {
            this.name = name;
            this.renderAction = renderAction;
        }

        internal void Dispatch(ref RenderContext context)
        {
            GL.PushDebugGroup(DebugSourceExternal.DebugSourceApplication, 0, name.Length, name);
            renderAction(ref context);
            renderAction = null;
            GL.PopDebugGroup();
        }
    }
}

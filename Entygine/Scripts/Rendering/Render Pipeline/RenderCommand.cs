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
            renderAction(ref context);
            renderAction = null;
        }
    }
}

using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class RenderCommandBuffer
    {
        private List<RenderCommand> renderCommands = new List<RenderCommand>();

        public void Clear()
        {
            renderCommands.Clear();
        }

        public void QueueCommand(RenderCommand command)
        {
            renderCommands.Add(command);
        }

        public void Dispatch(ref RenderContext context)
        {
            for (int i = 0; i < renderCommands.Count; i++)
                renderCommands[i].Dispatch(ref context);
        }
    }
}

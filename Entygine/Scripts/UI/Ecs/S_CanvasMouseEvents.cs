using Entygine.Cycles;
using Entygine.DevTools;
using Entygine.Ecs;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class S_CanvasMouseEvents : BaseSystem
    {
        private EntityQuerySettings query = new EntityQuerySettings();

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.With(TypeCache.ReadType<C_UICanvas>());

            MouseData mouseData = new MouseData();
            //mouseData.position = MainDevWindowGL.Window.MouseState.Position;
            //mouseData.positionDelta = MainDevWindowGL.Window.MouseState.Delta;
            //mouseData.clicked = MainDevWindowGL.Window.MouseState.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button1);

            //mouseData.position.Y = MainDevWindowGL.Window.Size.Y - mouseData.position.Y;

            Iterator it = new Iterator() { mouseData = mouseData };
            IterateQuery(it, query, false);
        }

        private struct Iterator : IQueryEntityIterator
        {
            public MouseData mouseData;

            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (chunk.TryGetComponent(index, out C_UICanvas canvas))
                    canvas.canvas.TriggerMouseEvent(mouseData);
            }
        }
    }
}

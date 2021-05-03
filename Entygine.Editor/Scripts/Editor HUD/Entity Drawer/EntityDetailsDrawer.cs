using Entygine.Ecs;
using ImGuiNET;
using System.Collections.Generic;

namespace Entygine_Editor
{
    public class EntityDetailsDrawer : DetailsDrawer<Entity>
    {
        private ComponentDrawerCollection drawers = new ComponentDrawerCollection();

        public EntityDetailsDrawer()
        {
            drawers.FindDrawers();
        }

        public override int MatchesObject(object obj)
        {
            return obj is Entity ? 0 : -1;
        }

        protected override void OnDraw()
        {
            ImGui.Text($"ID: {Context.id}");
            ImGui.SameLine();
            ImGui.Text($"Version: {Context.version}");
            ImGui.Separator();
            EntityIterator.PerformIteration(EntityWorld.Active, new DrawComponentsFromEntity() { entity = Context, drawers = drawers }, new EntityQuerySettings());
        }

        private struct DrawComponentsFromEntity : IQueryEntityIterator
        {
            public Entity entity;
            public ComponentDrawerCollection drawers;
            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (!chunk.GetEntity(index).Equals(entity))
                    return;

                chunk.GetComponentsFromEntity(entity, out List<IComponent> components);
                for (int i = 0; i < components.Count; i++)
                {
                    var currComp = components[i];
                    if (ImGui.CollapsingHeader(currComp.GetType().Name, ImGuiTreeNodeFlags.DefaultOpen))
                    {
                        var drawer = drawers.QueryDrawer(currComp);
                        drawer.SetComponentContext(currComp);
                        drawer.Draw();
                    }
                }
            }
        }
    }
}

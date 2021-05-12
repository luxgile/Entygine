using Entygine.Ecs;
using ImGuiNET;
using System.Collections.Generic;

namespace Entygine_Editor
{
    public class EntityDetailsDrawer : DetailsDrawer<Entity>
    {
        private ComponentDrawerCollection drawers = new ComponentDrawerCollection();
        private Entity selectedEntity;
        private EntityQueryScope queryScope;

        public EntityDetailsDrawer()
        {
            drawers.FindDrawers();

            queryScope = new(QuerySettings.Empty, (ref EntityQueryContext context) =>
            {
                context.GetEntity(out Entity entity);
                if (!entity.Equals(selectedEntity))
                    return;

                context.ReadAll(out TypeId[] ids, out IComponent[] components);

                for (int i = 0; i < components.Length; i++)
                {
                    var currComp = components[i];
                    if (ImGui.CollapsingHeader(currComp.GetType().Name, ImGuiTreeNodeFlags.DefaultOpen))
                    {
                        var drawer = drawers.QueryDrawer(currComp);
                        drawer.SetComponentContext(currComp);
                        drawer.Draw();
                    }
                }
            });
        }

        public override int MatchesObject(object obj)
        {
            return obj is Entity ? 0 : -1;
        }

        protected override void OnDraw()
        {
            selectedEntity = Context;
            ImGui.Text($"ID: {selectedEntity.id}");
            ImGui.SameLine();
            ImGui.Text($"Version: {selectedEntity.version}");
            ImGui.Separator();
            queryScope.Perform();
            //EntityIterator.PerformIteration(EntityWorld.Active, new DrawComponentsFromEntity() { entity = Context, drawers = drawers }, new QuerySettings());
        }
    }
}

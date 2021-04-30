using Entygine.Ecs;
using ImGuiNET;
using System.Reflection;

namespace Entygine_Editor
{
    public class DefaultComponentDrawer : ComponentDrawer
    {
        private FieldDrawerCollection drawers = new FieldDrawerCollection();

        public DefaultComponentDrawer()
        {
            drawers.FindDrawers();
        }

        public override bool Draw()
        {
            var fields = Context.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo currField = fields[i];
                var obj = currField.GetValue(Context);
                DrawObject(currField, obj);
            }
            return true;
        }

        private void DrawObject(FieldInfo currField, object obj)
        {
            var drawer = drawers.QueryDrawer(obj);
            if (drawer != null)
            {
                drawer.SetContext(obj);
                drawer.SetField(currField);
                drawer.Draw();
            }
            else
            {
                if (ImGui.TreeNode(currField.Name))
                {
                    var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        FieldInfo currField2 = fields[i];
                        var obj2 = currField2.GetValue(obj);
                        DrawObject(currField2, obj2);
                    }
                    ImGui.TreePop();
                }
            }
        }

        public override int MatchesObject(object obj)
        {
            return obj is IComponent ? 0 : -1;
        }
    }
}

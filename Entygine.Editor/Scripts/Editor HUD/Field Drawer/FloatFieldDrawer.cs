using ImGuiNET;

namespace Entygine_Editor
{
    public class FloatFieldDrawer : FieldDrawer<float>
    {
        public override bool Draw()
        {
            var value = Context;
            ImGui.InputFloat(Field.Name, ref value);

            return true;
        }

        public override int MatchesObject(object obj)
        {
            return obj is float ? 0 : -1;
        }
    }
}

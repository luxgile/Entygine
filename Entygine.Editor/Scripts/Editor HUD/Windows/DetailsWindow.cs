using ImGuiNET;
using System;

namespace Entygine_Editor
{
    public class DetailsWindow : WindowDrawer
    {
        private DetailsDrawerCollection collection = new DetailsDrawerCollection();
        private DetailsDrawer currentDrawer;

        public DetailsWindow()
        {
            collection.FindDrawers();

            ObjectSelections.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object obj)
        {
            currentDrawer = collection.QueryDrawer(obj);
            currentDrawer.SetContext(obj);
        }

        protected override void OnDraw()
        {
            if (currentDrawer != null)
                currentDrawer.Draw();
            else
            {
                ImGui.Text("Select an object.");
            }
        }

        public override string Title => "Details";
    }
}

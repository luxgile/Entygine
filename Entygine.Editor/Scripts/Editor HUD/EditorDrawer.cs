using System.Collections.Generic;

namespace Entygine_Editor
{
    public class EditorDrawer
    {
        public static EditorDrawer Current { get; set; }

        private List<RawDrawer> drawers = new List<RawDrawer>();

        internal void Draw()
        {
            for (int i = 0; i < drawers.Count; i++)
                drawers[i].Draw();
        }

        public void AttachDrawer(RawDrawer drawer)
        {
            drawers.Add(drawer);
        }

        public void DetachDrawer(RawDrawer window)
        {
            drawers.Remove(window);
        }
    }
}

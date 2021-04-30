using Entygine.Mathematics;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Entygine_Editor
{
    public class DetailsDrawerCollection : IDrawerCollection<DetailsDrawer>
    {
        private DetailsDrawer[] drawers;

        public void FindDrawers()
        {
            System.Type[] types = Assembly.GetAssembly(typeof(DetailsDrawer)).GetTypes().Where((x) => !x.IsAbstract && x.IsClass && x.IsSubclassOf(typeof(DetailsDrawer))).ToArray();
            drawers = new DetailsDrawer[types.Length];
            for (int i = 0; i < types.Length; i++)
                drawers[i] = (DetailsDrawer)System.Activator.CreateInstance(types[i]);
        }

        public DetailsDrawer QueryDrawer(object obj)
        {
            List<Vec2i> drawersIndex = new List<Vec2i>();
            for (int i = 0; i < drawers.Length; i++)
            {
                int match = drawers[i].MatchesObject(obj);
                if (match != -1)
                    drawersIndex.Add(new Vec2i(match, i));
            }

            if (drawersIndex.Count == 0)
                return null;

            drawersIndex.Sort((x, y) => x.x.CompareTo(y.x));
            return drawers[drawersIndex[0].y];
        }
    }
}

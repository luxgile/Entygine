using Entygine.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entygine_Editor
{
    public class ComponentDrawerCollection : IDrawerCollection<ComponentDrawer>
    {
        private ComponentDrawer[] drawers;

        public void FindDrawers()
        {
            System.Type[] types = Assembly.GetAssembly(typeof(ComponentDrawer)).GetTypes().Where((x) => !x.IsAbstract && x.IsClass && x.IsSubclassOf(typeof(ComponentDrawer))).ToArray();
            drawers = new ComponentDrawer[types.Length];
            for (int i = 0; i < types.Length; i++)
                drawers[i] = (ComponentDrawer)System.Activator.CreateInstance(types[i]);
        }

        public ComponentDrawer QueryDrawer(object obj)
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


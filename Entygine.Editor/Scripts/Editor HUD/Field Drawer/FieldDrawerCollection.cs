using Entygine.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entygine_Editor
{
    public class FieldDrawerCollection : IDrawerCollection<FieldDrawer>
    {
        private FieldDrawer[] drawers;

        public void FindDrawers()
        {
            System.Type[] types = Assembly.GetAssembly(typeof(FieldDrawer)).GetTypes().Where((x) => !x.IsAbstract && x.IsClass && x.IsSubclassOf(typeof(FieldDrawer))).ToArray();
            drawers = new FieldDrawer[types.Length];
            for (int i = 0; i < types.Length; i++)
                drawers[i] = (FieldDrawer)System.Activator.CreateInstance(types[i]);
        }

        public FieldDrawer QueryDrawer(object obj)
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

    public abstract class FieldDrawer : RawDrawer
    {
        protected object Context { get; private set; }
        protected FieldInfo Field { get; private set; }
        public void SetContext(object obj) => Context = obj;
        public void SetField(FieldInfo field) => Field = field;
        public abstract int MatchesObject(object obj);
    }
    public abstract class FieldDrawer<T0> : FieldDrawer
    {
        protected new T0 Context => (T0)base.Context;
    }
}


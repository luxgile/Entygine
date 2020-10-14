using System.Collections.Generic;

namespace Entygine.DevTools
{
    public abstract class GizmoOrder<T0> where T0 : struct
    {
        private List<T0> gizmoData = new List<T0>();

        public void AddOrder(T0 data)
        {
            gizmoData.Add(data);
        }

        public void Clear()
        {
            gizmoData.Clear();
        }

        public void PerformDraw()
        {
            Draw(gizmoData);
            //Clear();
        }

        protected abstract void Draw(List<T0> gizmoData); 
    }
}

using System;

namespace Entygine_Editor
{
    public static class ObjectSelections
    {
        public static object CurrentObj { get; private set; }

        public static event Action<object> SelectionChanged;

        public static void SelectObject(object obj)
        {
            CurrentObj = obj;
            SelectionChanged?.Invoke(obj);
        }
    }
}

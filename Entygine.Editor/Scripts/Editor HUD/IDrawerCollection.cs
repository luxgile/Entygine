namespace Entygine_Editor
{
    public interface IDrawerCollection<TDrawer> where TDrawer : RawDrawer
    {
        TDrawer QueryDrawer(object obj);

        T0 QueryDrawer<T0>(object obj) where T0 : TDrawer => (T0)QueryDrawer(obj);
    }
}

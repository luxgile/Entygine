namespace Entygine.UI
{
    public interface IMouseClick
    {
        void OnMouseClick(MouseData mouseData);
    }

    public interface IMouseEnter
    {
        void OnMouseEnter(MouseData mouseData);
    }

    public interface IMouseExit
    {
        void OnMouseExit(MouseData mouseData);
    }
}

using OpenToolkit.Mathematics;

namespace Entygine.UI
{
    public abstract class UIPanel : UIElement
    {
        public abstract Matrix4[] GetChildsModels(Matrix4 parentModel);
    }
}

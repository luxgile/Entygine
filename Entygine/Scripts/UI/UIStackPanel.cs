using OpenToolkit.Mathematics;

namespace Entygine.UI
{
    public class UIStackPanel : UIPanel
    {
        public override Matrix4[] GetChildsModels(Matrix4 parentModel)
        {
            Matrix4[] models = new Matrix4[Children.Count];
            Vector2 parentPos = parentModel.ExtractTranslation().Xy;
            Vector2 parentSize = parentModel.ExtractScale().Xy;
            float sizeDelta = parentSize.Y / Children.Count;
            Vector2 currentPos = parentPos;
            for (int i = 0; i < Children.Count; i++)
            {
                Vector2 childSize = new Vector2(parentSize.X, sizeDelta);
                Matrix4 childModel = Matrix4.Identity;
                childModel *= Matrix4.CreateScale(new Vector3(childSize));
                childModel *= Matrix4.CreateTranslation(new Vector3(currentPos));
                models[i] = childModel;
                currentPos.Y += sizeDelta;
            }
            return models;
        }
    }
}

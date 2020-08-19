using OpenToolkit.Mathematics;
using System.Collections.Generic;

namespace Entygine.UI
{
    public class UICanvas
    {
        public UIElement Root { get; set; }

        public UICanvas()
        {
            Root = new UIStackPanel();
            Root.Children.Add(new UIElement());
            Root.Children.Add(new UIElement());
            Root.Children.Add(new UIElement());
        }

        private Matrix4 GetModelMatrix()
        {
            float windowWidth = MainDevWindowGL.Window.Size.X;
            float windowHeight = MainDevWindowGL.Window.Size.Y;
            Vector2 size = new Vector2(windowWidth, windowHeight);

            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateScale(new Vector3(size));
            return model;
        }

        public Matrix4[] CalculateModels()
        {
            List<Matrix4> models = new List<Matrix4>();
            GetModelsFromElement(ref models, GetModelMatrix(), Root);
            return models.ToArray();
        }

        private void GetModelsFromElement(ref List<Matrix4> models, Matrix4 parentModel, UIElement element)
        {
            Matrix4 elementModel = element.GetModelMatrix(parentModel);
            models.Add(elementModel);
            if (element is UIPanel panel)
            {
                Matrix4[] childrenModels = panel.GetChildsModels(elementModel);

                for (int i = 0; i < element.Children.Count; i++)
                    GetModelsFromElement(ref models, childrenModels[i], element.Children[i]);
            }
            else
            {
                for (int i = 0; i < element.Children.Count; i++)
                    GetModelsFromElement(ref models, elementModel, element.Children[i]);
            }
        }
    }
}

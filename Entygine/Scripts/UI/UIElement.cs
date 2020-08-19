using Entygine.Rendering;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Desktop;
using System;
using System.Collections.Generic;

namespace Entygine.UI
{
    public class UIElement
    {
        private IStyle style = new UIStyle();
        private List<UIElement> children = new List<UIElement>();

        public UIElement()
        {
            Random rnd = new Random();
            Color = new Color01((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 0.4f);
        }

        public virtual Matrix4 GetModelMatrix(Matrix4 parentModel)
        {
            Vector2 pos = parentModel.ExtractTranslation().Xy;
            Vector2 size = parentModel.ExtractScale().Xy;
            pos.X += Style.leftPadding;
            pos.Y += Style.bottomPadding;
            size.X -= Style.rightPadding * 2;
            size.Y -= Style.topPadding * 2;

            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateScale(new Vector3(size));
            model *= Matrix4.CreateTranslation(new Vector3(pos));
            return model;
        }

        public IStyle Style => style;

        //TODO: Rendering stuff should be decoupled.
        public Color01 Color { get; }
        public List<UIElement> Children => children;
    }
}

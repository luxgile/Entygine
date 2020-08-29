﻿using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.UI
{
    public class UICanvas
    {
        public UIElement Root { get; set; }

        public UICanvas()
        {
            Root = new UIStackPanel();
            Root.Children.Add(new UIImage());
            Root.Children.Add(new UIImage());
            Root.Children.Add(new UIImage());
        }

        public List<UI_IRenderable> GetRenderables()
        {
            List<UI_IRenderable> renderables = new List<UI_IRenderable>();
            FindRenderables(Root);
            return renderables;

            void FindRenderables(UIElement element)
            {
                List<UIElement> children = element.Children;
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] is UI_IRenderable renderable)
                        renderables.Add(renderable);
                }

                //It's done in a different loop to order them properly
                for (int i = 0; i < children.Count; i++)
                    FindRenderables(children[i]);
            }
        }

        public void UpdateRenderers()
        {
            Rect rootRect = new Rect(Vector2.Zero, new Vector2(MainDevWindowGL.Window.Size.X, MainDevWindowGL.Window.Size.Y));
            UpdateElementRenderers(rootRect, Root);
            static void UpdateElementRenderers(Rect parentRect, UIElement element)
            {
                Rect elementRect = element.GetRect(parentRect);
                if (element is UI_IRenderable renderable)
                    renderable.Rect = elementRect;

                if (element is UIPanel panel)
                {
                    Rect[] childrenRect = panel.GetChildsRect(elementRect);

                    for (int i = 0; i < element.Children.Count; i++)
                        UpdateElementRenderers(childrenRect[i], element.Children[i]);
                }
                else
                {
                    for (int i = 0; i < element.Children.Count; i++)
                        UpdateElementRenderers(elementRect, element.Children[i]);
                }
            }
        }
    }
}
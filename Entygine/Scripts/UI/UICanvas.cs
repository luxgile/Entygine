﻿using Entygine.Rendering;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Entygine.UI
{
    public class UICanvas
    {
        public UIElement Root { get; set; }

        public UIText deltaTimeText;

        private IRaycastable focusedElement;

        public UICanvas()
        {
            Root = new UIElement();
            UIStackPanel stackPanel = new UIStackPanel()
            {
                Orientation = UIStackPanel.EOrientation.Horizontal,
                Padding = new Padding()
                {
                    top = new Padding.PaddingElement(5),
                    bottom = new Padding.PaddingElement(0.95f),
                    left = new Padding.PaddingElement(10),
                    right = new Padding.PaddingElement(10)
                },
            };

            deltaTimeText = new UIText
            {
                Size = 0.3f
            };


            UIImage bgImg = new UIImage()
            {
                Color = Color01.gray,
            };
            bgImg.Children.Add(deltaTimeText);

            UIButton exitButton = new UIButton()
            {
                Padding = new Padding() { left = new Padding.PaddingElement(0.05f), right = new Padding.PaddingElement(0.85f) },
            };
            exitButton.Children.Add(new UIText("X"));


            stackPanel.Children.Add(exitButton);
            stackPanel.Children.Add(bgImg);

            Root.Children.Add(stackPanel);

            //Image to test alignment:
            UIImage img = new UIImage()
            {
                Color = Color01.white,
            };
            UIText t = new UIText("SOME TEXT")
            {
                VerticalAlignment = EVerticalAlign.Center,
                HorizontalAlignment = EHorizontalAlign.Right
            };
            img.Children.Add(t);
            Root.Children.Add(img);
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

        public void TriggerMouseEvent(MouseData mouseData)
        {
            if (RaycastElements(Root, out IRaycastable raycasteable))
            {
                if (focusedElement != raycasteable)
                {
                    if (focusedElement is IMouseExit exit)
                        exit.OnMouseExit(mouseData);

                    if (raycasteable is IMouseEnter enter)
                        enter.OnMouseEnter(mouseData);

                    focusedElement = raycasteable;
                }
            }
            else
            {
                if (focusedElement != null)
                {
                    if (focusedElement is IMouseExit exit)
                        exit.OnMouseExit(mouseData);

                    focusedElement = null;
                }
            }

            if (mouseData.clicked && focusedElement != null && focusedElement is IMouseClick click)
                click.OnMouseClick(mouseData);

            bool RaycastElements(UIElement element, out IRaycastable raycastable)
            {
                for (int i = 0; i < element.Children.Count; i++)
                {
                    if (RaycastElements(element.Children[i], out raycastable))
                        return true;
                }

                if (element is IRaycastable ray && ray.Raycast(mouseData))
                {
                    raycastable = ray;
                    return true;
                }

                raycastable = null;
                return false;
            }
        }
    }
}

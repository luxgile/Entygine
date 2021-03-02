using Entygine.DevTools;
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
            Root = new UIStackPanel();
            deltaTimeText = new UIText();
            UIImage image1 = new UIImage();
            image1.Children.Add(deltaTimeText);

            UIImage image2 = new UIImage();
            image2.Children.Add(new UIText("VAVAV \nTEST \nTEST"));

            UIImage image3 = new UIImage();

            Root.Children.Add(image1);
            Root.Children.Add(image2);
            Root.Children.Add(image3);
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
            if(RaycastElements(Root, out IRaycastable raycasteable))
            {
                if(focusedElement != raycasteable)
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

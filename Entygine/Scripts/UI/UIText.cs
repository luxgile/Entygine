using Entygine.Rendering;
using OpenTK.Mathematics;
using System;

namespace Entygine.UI
{
    public class UIText : UIElement, UI_IRenderable
    {
        public string Text { get; set; }
        public float Size { get; set; } = 0.5f;
        public EVerticalAlign VerticalAlignment { get; set; } = EVerticalAlign.Top;
        public EHorizontalAlign HorizontalAlignment { get; set; } = EHorizontalAlign.Left;
        public Font Font { get; set; }
        public Color01 Color { get; set; }
        public Rect Rect { get; set; }
        public Material Material { get; set; }

        public UIText(string text) : this()
        {
            Text = text;
        }
        public UIText()
        {
            Shader shader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\text.vert")
                , AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\text.frag"));
            Material = new Material(shader, Texture2D.CreateWhiteTexture(1, 1));
            Material.LoadMaterial();

            Font = new Font(AssetBrowser.Utilities.LocalToAbsolutePath(@"Fonts\ArialCE.ttf"));

            Color = new Color01(0, 0, 0, 1f);
        }

        public void DrawUI(Mesh mesh)
        {
            Material.SetColor("color", Color);
            Ogl.ActiveTexture(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);

            Material.UseMaterial();
            mesh.UpdateMeshData(Material);
            Ogl.BindVertexArray(mesh.GetVertexArrayHandle());

            Vector2 position = GetInitialPosition();
            char prevC = char.MinValue;
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                switch (c)
                {
                    case '\n':
                        LineBreak(ref position);
                        break;

                    default:
                        DrawCharacter(mesh, c, prevC, ref position, i > 0);
                        break;
                }

                prevC = c;
            }
        }

        private Vector2 GetInitialPosition()
        {
            float x = 0, y = 0;
            switch (HorizontalAlignment)
            {
                case EHorizontalAlign.Left:
                    x = Rect.pos.X;
                    break;

                case EHorizontalAlign.Center:
                    x = Rect.pos.X + Rect.size.X / 2f;
                    break;

                case EHorizontalAlign.Right:
                    x = Rect.pos.X + Rect.size.X;
                    break;
            }

            switch (VerticalAlignment)
            {
                case EVerticalAlign.Top:
                    y = Rect.pos.Y + Rect.size.Y - Font.LineHeight / 2f;
                    break;

                case EVerticalAlign.Center:
                    y = Rect.pos.Y + Rect.size.Y / 2f;
                    break;

                case EVerticalAlign.Bottom:
                    y = Rect.pos.Y;
                    break;
            }

            return new Vector2(x, y);
        }

        private void LineBreak(ref Vector2 position)
        {
            position.X = Rect.pos.X;
            position.Y -= Font.LineHeight;
        }

        private void DrawCharacter(Mesh mesh, char c, char prevC, ref Vector2 position, bool useKerning)
        {
            FontCharacter fontCharacter = Font.GetCharacter(c);
            Rect rect = fontCharacter.GetRect() * Size;

            //Calculate character rect
            rect.pos.X = position.X + rect.pos.X;
            rect.pos.Y = position.Y - rect.pos.Y;

            //Advance
            position.X += (fontCharacter.Advance >> 6) * Size;

            //Kerning
            if (useKerning)
            {
                float kerning = Font.GetKerning(prevC, c).x >> 6;
                position.X -= kerning;
                rect.pos.X -= kerning;
            }

            //Drawing character
            Matrix4 model = rect.GetModelMatrix();
            Ogl.BindTexture(OpenTK.Graphics.OpenGL4.TextureTarget.Texture2D, fontCharacter.TextureID);
            Material.SetMatrix("model", model);
            Ogl.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, mesh.GetIndiceCount(), OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);
        }
    }
}

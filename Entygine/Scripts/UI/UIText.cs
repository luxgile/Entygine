using Entygine.Rendering;
using OpenTK.Mathematics;
using System;

namespace Entygine.UI
{
    public class UIText : UIElement, UI_IRenderable
    {
        private string text;
        public string Text { get => text; set { text = value; textChanged = true; } }
        public float Size { get; set; } = 0.5f;
        public EVerticalAlign VerticalAlignment { get; set; } = EVerticalAlign.Top;
        public EHorizontalAlign HorizontalAlignment { get; set; } = EHorizontalAlign.Left;
        public Font Font { get; set; }
        public Color01 Color { get; set; }
        public Rect Rect { get; set; }
        public Material Material { get; set; }

        private bool textChanged;
        private Rect[] charsPos;

        public UIText(string text) : this()
        {
            Text = text;
            textChanged = true;
        }
        public UIText()
        {
            Shader shader = Shader.CreateShaderWithPath(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\text.vert")
                , AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\text.frag"), "Text Shader");
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

            if (textChanged)
            {
                UpdateCharPositions();
                PostProcessCharPositions();
                textChanged = false;
            }

            for (int i = 0; i < Text.Length; i++)
                DrawCharacter(mesh, Text[i], charsPos[i]);
        }

        private void UpdateCharPositions()
        {
            charsPos = new Rect[Text.Length];
            Vector2 position = new Vector2(GetHorizontalInitialPos(), GetVerticalInitialPos());
            char prevC = char.MinValue;
            for (int i = 0; i < Text.Length; i++)
            {
                Rect r;

                char c = Text[i];
                switch (c)
                {
                    case '\n':
                        position.X = GetHorizontalInitialPos();
                        position.Y -= Font.LineHeight;

                        r = new Rect()
                        {
                            pos = new Vector2(position.X, position.Y),
                        };
                        break;

                    default:
                        FontCharacter fontCharacter = Font.GetCharacter(c);
                        r = fontCharacter.GetRect() * Size;

                        //Calculate character rect
                        r.pos.X = position.X + r.pos.X;
                        r.pos.Y = position.Y - r.pos.Y;

                        //Advance
                        position.X += (fontCharacter.Advance >> 6) * Size;

                        //Kerning
                        float kerning = Font.GetKerning(prevC, c).x >> 6;
                        position.X -= kerning;
                        r.pos.X -= kerning;

                        break;
                }

                charsPos[i] = r;
                prevC = c;
            }
        }

        private void PostProcessCharPositions()
        {
            switch (HorizontalAlignment)
            {
                case EHorizontalAlign.Center:
                    float halfWidth = GetTotalCharWidth() / 2f;
                    for (int i = 0; i < charsPos.Length; i++)
                        charsPos[i].pos.X -= halfWidth;
                    break;
             
                case EHorizontalAlign.Right:
                    float width = GetTotalCharWidth();
                    for (int i = 0; i < charsPos.Length; i++)
                        charsPos[i].pos.X -= width;
                    break;
            }
        }

        private float GetTotalCharWidth()
        {
            return charsPos[^1].pos.X - charsPos[0].pos.X;
            //Rect prevChar = charsPos[0];
            //float width = prevChar.size.X;
            //for (int i = 1; i < charsPos.Length; i++)
            //{
            //    Rect currPos = charsPos[i];
            //    width += currPos.size.X;
            //    width += currPos.pos.X - prevChar.pos.X;
            //    prevChar = currPos;
            //}
            //return width;
        }

        private float GetHorizontalInitialPos()
        {
            return HorizontalAlignment switch
            {
                EHorizontalAlign.Center => Rect.pos.X + Rect.size.X / 2f,
                EHorizontalAlign.Right => Rect.pos.X + Rect.size.X,
                _ => Rect.pos.X,
            };
        }

        private float GetVerticalInitialPos()
        {
            return VerticalAlignment switch
            {
                EVerticalAlign.Center => Rect.pos.Y + Rect.size.Y / 2f,
                EVerticalAlign.Bottom => Rect.pos.Y,
                _ => Rect.pos.Y + Rect.size.Y - Font.LineHeight / 2f,
            };
        }

        private void DrawCharacter(Mesh mesh, char c, Rect position)
        {
            FontCharacter fontCharacter = Font.GetCharacter(c);

            //Drawing character
            Matrix4 model = position.GetModelMatrix();
            Ogl.BindTexture(OpenTK.Graphics.OpenGL4.TextureTarget.Texture2D, fontCharacter.TextureID);
            Material.SetMatrix("model", model);
            Ogl.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, mesh.GetIndiceCount(), OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);
        }
    }
}

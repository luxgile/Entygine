using Entygine.Rendering;
using OpenTK.Mathematics;

namespace Entygine.UI
{
    public class UIText : UIElement, UI_IRenderable
    {
        public string Text { get; set; }
        public float Size { get; set; } = 12;
        public Font Font { get; set; }
        public Color01 Color { get; set; }
        public Rect Rect { get; set; }
        public Material Material { get; set; }

        public UIText(string text)
        {
            Text = text;

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

            Vector2 position = Rect.pos;

            foreach (char c in Text)
            {
                FontCharacter fontCharacter = Font.GetCharacter(c);
                Rect rect = fontCharacter.GetRect() * Size;
                rect.pos.X = position.X + rect.pos.X;
                rect.pos.Y = position.Y - rect.pos.Y;

                position.X += (fontCharacter.Advance >> 6) * Size;

                Matrix4 model = rect.GetModelMatrix();

                Ogl.BindTexture(OpenTK.Graphics.OpenGL4.TextureTarget.Texture2D, fontCharacter.TextureID);
                Material.SetMatrix("model", model);
                Ogl.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, mesh.GetIndiceCount(), OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}

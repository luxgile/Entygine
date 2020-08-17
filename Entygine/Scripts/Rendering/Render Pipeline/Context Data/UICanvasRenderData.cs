using Entygine.Rendering.Pipeline;
using Entygine.UI;
using OpenToolkit.Mathematics;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class UICanvasRenderData : RenderContextData
    {
        private List<UICanvas> canvases = new List<UICanvas>();

        public UICanvasRenderData()
        {
            Camera = CameraData.CreateOrthographicCamera(MainDevWindowGL.Window.Size.X, MainDevWindowGL.Window.Size.Y, -1, 1);

            Shader uiShader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.vert"), AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.frag"));
            Material = new Material(uiShader, Texture2D.CreatePlainTexture(64, 64, Rgba32.ParseHex("4f97cf")));
            Material.LoadMaterial();

            Mesh = MeshPrimitives.CreatePlaneXY(1);

            Mesh.SetVertexLayout(new VertexBufferLayout[]
            {
                new VertexBufferLayout(VertexAttribute.Position, VertexAttributeFormat.Int32, 2),
                new VertexBufferLayout(VertexAttribute.Uv0, VertexAttributeFormat.Int32, 2),
            });
        }

        public void AddCanvas(UICanvas canvas) => canvases.Add(canvas);
        public List<UICanvas> GetCanvases() => canvases;

        public Mesh Mesh { get; }
        public Material Material { get; }
        public CameraData Camera { get; }
    }
}

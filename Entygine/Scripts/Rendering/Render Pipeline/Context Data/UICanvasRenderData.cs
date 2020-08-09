using Entygine.Rendering.Pipeline;
using Entygine.UI;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class UICanvasRenderData : RenderContextData
    {
        private Mesh quadMesh;
        private Material mat;
        private List<UICanvas> canvases = new List<UICanvas>();

        public UICanvasRenderData()
        {
            Shader uiShader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.vert"), AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.frag"));
            mat = new Material(uiShader, Texture2D.CreateWhiteTexture(1, 1));
            quadMesh = MeshPrimitives.CreatePlane(1);
            quadMesh.SetVertexLayout(new VertexBufferLayout[]
            {
                new VertexBufferLayout(VertexAttribute.Position, VertexAttributeFormat.Int32, 2),
                new VertexBufferLayout(VertexAttribute.Uv0, VertexAttributeFormat.Int32, 2),
            });
        }

        public void AddCanvas(UICanvas canvas) => canvases.Add(canvas);
        public List<UICanvas> GetCanvases() => canvases;

        public Mesh Mesh => quadMesh;
        public Material Material => mat;
    }
}

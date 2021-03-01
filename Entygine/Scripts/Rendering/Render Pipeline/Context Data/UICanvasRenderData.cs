using Entygine.Rendering.Pipeline;
using Entygine.UI;
using OpenTK.Mathematics;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class UICanvasRenderData : RenderContextData
    {
        private CameraData camera;
        private List<UICanvas> canvases = new List<UICanvas>();

        public UICanvasRenderData()
        {
            camera = CameraData.CreateOrthographicCamera(MainDevWindowGL.Window.Size.X / MainDevWindowGL.Window.Size.Y, 1, -1, 1);
            camera.SetOrthoResolution(MainDevWindowGL.Window.Size.X, MainDevWindowGL.Window.Size.Y);

            //Mesh = MeshPrimitives.CreatePlaneXY(1);

            Mesh = new Mesh();
            Mesh.SetVertices(new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 0, 0),
            });
            Mesh.SetUVs(new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
            });
            Mesh.SetIndices(new uint[]
            {
                0, 2, 1, 0, 3, 2
            });
            Mesh.SetVertexLayout(new VertexBufferLayout[]
            {
                new VertexBufferLayout(VertexAttribute.Position, VertexAttributeFormat.Float32, 2),
                new VertexBufferLayout(VertexAttribute.Uv0, VertexAttributeFormat.Float32, 2),
            });
        }

        public void ClearCanvas() => canvases.Clear();
        public void AddCanvas(UICanvas canvas) => canvases.Add(canvas);
        public List<UICanvas> GetCanvases() => canvases;

        public Mesh Mesh { get; }
        public CameraData Camera => camera;
    }
}

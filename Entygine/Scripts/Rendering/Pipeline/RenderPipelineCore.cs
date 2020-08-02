using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entygine.DevTools;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderPipelineCore
    {
        private struct RenderMeshPair
        {
            public Material mat;
            public Mesh mesh;

            public override bool Equals(object obj)
            {
                return obj is RenderMeshPair pair &&
                       EqualityComparer<Material>.Default.Equals(mat, pair.mat) &&
                       EqualityComparer<Mesh>.Default.Equals(mesh, pair.mesh);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(mat, mesh);
            }
        }

        private static Dictionary<RenderMeshPair, List<Matrix4>> meshesToDraw = new Dictionary<RenderMeshPair, List<Matrix4>>();

        public static void QueueMesh(Mesh mesh, Material mat, Matrix4 transform)
        {
            RenderMeshPair pair = new RenderMeshPair() { mat = mat, mesh = mesh };
            if (meshesToDraw.TryGetValue(pair, out List<Matrix4> positions))
            {
                positions.Add(transform);
            }
            else
            {
                meshesToDraw.Add(pair, new List<Matrix4>() { transform });
            }
        }

        public static void Draw(CameraData camera, Matrix4 transform)
        {
            Matrix4 projection = camera.CalculatePerspective();
            foreach (KeyValuePair<RenderMeshPair, List<Matrix4>> item in meshesToDraw)
            {
                RenderMeshPair pair = item.Key;
                List<Matrix4> positions = item.Value;

                pair.mesh.ForceUpdateMeshData();
                GL.BindVertexArray(pair.mesh.GetVertexArrayHandle());
                pair.mat.UseMaterial();

                VertexBufferLayout[] layouts = pair.mesh.GetVertexLayout();
                int layoutSize = pair.mesh.GetVertexLayoutSize();
                int layoutOffset = 0;
                for (int i = 0; i < layouts.Length; i++)
                {
                    VertexBufferLayout layout = layouts[i];
                    int location = layout.Attribute.GetAttributeLocation(pair.mat);

                    int stride = layoutSize * layout.Format.ByteSize();
                    GL.EnableVertexAttribArray(location);
                    GL.VertexAttribPointer(location, layout.Size, layout.Format.ToOpenGlAttribType(), false, stride, layoutOffset);

                    layoutOffset += layout.Size * layout.Format.ByteSize();
                }

                //Update camera pos for material
                pair.mat.SetMatrix("view", transform);
                pair.mat.SetMatrix("projection", projection);
                pair.mat.SetVector3("ambientLight", new Vector3(0.1f, 0.1f, 0.1f));
                pair.mat.SetVector3("directionalLight", new Vector3(1, 1, 1));
                pair.mat.SetVector3("directionalDir", new Vector3(-0.2f, 0.5f, 0.5f));

                for (int i = 0; i < positions.Count; i++)
                {
                    pair.mat.SetMatrix("model", positions[i]);

                    GL.DrawElements(PrimitiveType.Triangles, pair.mesh.GetIndiceCount(), DrawElementsType.UnsignedInt, 0);
                }
            }

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                DevConsole.Log(error);
        }

        public static void ClearPipeline()
        {
            meshesToDraw.Clear();
        }
    }
}

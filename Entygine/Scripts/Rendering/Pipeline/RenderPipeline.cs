using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderPipeline
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

                int vertexLocation = pair.mat.shader.GetAttributeLocation("aPosition");
                int uvsLocation = pair.mat.shader.GetAttributeLocation("aTexCoord");
                int normalsLocation = pair.mat.shader.GetAttributeLocation("aNormal");

                pair.mesh.ForceUpdateMeshData();
                GL.BindVertexArray(pair.mesh.GetVertexArrayHandle());
                pair.mat.UseMaterial();

                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

                GL.EnableVertexAttribArray(uvsLocation);
                GL.VertexAttribPointer(uvsLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

                GL.EnableVertexAttribArray(normalsLocation);
                GL.VertexAttribPointer(normalsLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

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

                GL.BindVertexArray(0);
            }

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                Console.WriteLine(error);
        }

        public static void ClearPipeline()
        {
            meshesToDraw.Clear();
        }
    }
}

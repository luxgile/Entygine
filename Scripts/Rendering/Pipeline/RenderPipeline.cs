using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderPipeline
    {
        private static Dictionary<Material, List<Mesh>> meshesToDraw = new Dictionary<Material, List<Mesh>>();
        private static Dictionary<Material, List<Matrix4>> meshesPosition = new Dictionary<Material, List<Matrix4>>();

        public static void QueueMesh(Mesh mesh, Material mat, Matrix4 transform)
        {
            if (meshesToDraw.TryGetValue(mat, out List<Mesh> meshes))
            {
                meshes.Add(mesh);
                meshesPosition[mat].Add(transform);
            }
            else
            {
                meshesToDraw.Add(mat, new List<Mesh>() { mesh });
                meshesPosition.Add(mat, new List<Matrix4>() { transform });
            }
        }

        public static void Draw(CameraData camera, Matrix4 transform)
        {
            Matrix4 projection = camera.CalculatePerspective();
            foreach (KeyValuePair<Material, List<Mesh>> item in meshesToDraw)
            {
                Material material = item.Key;
                List<Mesh> meshes = item.Value;
                List<Matrix4> positions = meshesPosition[material];

                int vertexLocation = material.shader.GetAttributeLocation("aPosition");
                int uvsLocation = material.shader.GetAttributeLocation("aTexCoord");

                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                GL.EnableVertexAttribArray(uvsLocation);
                GL.VertexAttribPointer(uvsLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                Debug.Assert(meshes.Count == positions.Count);

                //Update camera pos for material
                material.SetMatrix("view", transform);
                material.SetMatrix("projection", projection);

                material.UseMaterial();

                for (int i = 0; i < meshes.Count; i++)
                {
                    Mesh mesh = meshes[i];
                    mesh.UpdateMeshData();

                    material.SetMatrix("model", positions[i]);

                    GL.BindVertexArray(mesh.GetVertexBuffer());

                    GL.DrawElements(PrimitiveType.Triangles, mesh.GetIndiceCount(), DrawElementsType.UnsignedInt, 0);

                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
            }

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                Console.WriteLine(error);
        }

        public static void ClearPipeline()
        {
            meshesToDraw.Clear();
            meshesPosition.Clear();
        }
    }
}

using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using System.Collections.Generic;
using Entygine.DevTools;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderPipelineCore
    {
        private static Dictionary<MeshRender, List<Matrix4>> meshesToDraw = new Dictionary<MeshRender, List<Matrix4>>();

        public static void QueueMesh(Mesh mesh, Material mat, Matrix4 transform)
        {
            MeshRender pair = new MeshRender() { mat = mat, mesh = mesh };
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
            foreach (KeyValuePair<MeshRender, List<Matrix4>> item in meshesToDraw)
            {
                MeshRender pair = item.Key;
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
                //TODO: Use 'Uniform buffer objects' to globally share this data
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

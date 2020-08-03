using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using System.Collections.Generic;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderCommandsLibrary
    {
        public static RenderCommand ClearColorAndDepthBuffer()
        {
            return new RenderCommand("Clear Buffers", (ref RenderContext context) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            });
        }

        public static RenderCommand DrawGeometry(CameraData camera, Matrix4 cameraTransform)
        {
            return new RenderCommand("Draw Geometry", (ref RenderContext context) =>
            {
                Matrix4 projection = camera.CalculatePerspective();
                if (!context.TryGetData(out GeometryRenderData geometryData))
                    return;

                List<MeshRenderGroup> renderGroups = geometryData.GetRenderGroups();
                for (int i = 0; i < renderGroups.Count; i++)
                {
                    MeshRenderGroup renderGroup = renderGroups[i];

                    MeshRender pair = renderGroup.MeshRender;
                    List<Matrix4> positions = renderGroup.Transforms;

                    pair.mesh.ForceUpdateMeshData();
                    GL.BindVertexArray(pair.mesh.GetVertexArrayHandle());
                    pair.mat.UseMaterial();

                    VertexBufferLayout[] layouts = pair.mesh.GetVertexLayout();
                    int layoutSize = pair.mesh.GetVertexLayoutSize();
                    int layoutOffset = 0;
                    for (int l = 0; l < layouts.Length; l++)
                    {
                        VertexBufferLayout layout = layouts[l];
                        int location = layout.Attribute.GetAttributeLocation(pair.mat);

                        int stride = layoutSize * layout.Format.ByteSize();
                        GL.EnableVertexAttribArray(location);
                        GL.VertexAttribPointer(location, layout.Size, layout.Format.ToOpenGlAttribType(), false, stride, layoutOffset);

                        layoutOffset += layout.Size * layout.Format.ByteSize();
                    }

                    //Update camera pos for material
                    //TODO: Use 'Uniform buffer objects' to globally share this data
                    pair.mat.SetMatrix("view", cameraTransform);
                    pair.mat.SetMatrix("projection", projection);
                    pair.mat.SetVector3("ambientLight", new Vector3(0.1f, 0.1f, 0.1f));
                    pair.mat.SetVector3("directionalLight", new Vector3(1, 1, 1));
                    pair.mat.SetVector3("directionalDir", new Vector3(-0.2f, 0.5f, 0.5f));

                    for (int p = 0; p < positions.Count; p++)
                    {
                        pair.mat.SetMatrix("model", positions[p]);

                        GL.DrawElements(PrimitiveType.Triangles, pair.mesh.GetIndiceCount(), DrawElementsType.UnsignedInt, 0);
                    }
                }
            });
        }
    }
}

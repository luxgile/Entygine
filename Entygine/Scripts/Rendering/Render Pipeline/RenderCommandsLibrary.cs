using Entygine.DevTools;
using Entygine.UI;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using SixLabors.ImageSharp;
using System.Collections.Generic;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderCommandsLibrary
    {
        public static RenderCommand ClearColorAndDepthBuffer()
        {
            return new RenderCommand("Clear Buffers", (ref RenderContext context) =>
            {
                Ogl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            });
        }

        public static RenderCommand DrawGeometry(CameraData camera, Matrix4 cameraTransform)
        {
            return new RenderCommand("Draw Geometry", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out GeometryRenderData geometryData))
                    return;

                Matrix4 projection = camera.CalculateProjection();

                List<MeshRenderGroup> renderGroups = geometryData.GetRenderGroups();
                for (int i = 0; i < renderGroups.Count; i++)
                {
                    MeshRenderGroup renderGroup = renderGroups[i];

                    MeshRender pair = renderGroup.MeshRender;
                    List<Matrix4> positions = renderGroup.Transforms;

                    GraphicsAPI.UseMeshMaterial(pair.mesh, pair.mat);

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

                        GraphicsAPI.DrawTriangles(pair.mesh.GetIndiceCount());
                    }

                    GraphicsAPI.FreeMeshMaterial(pair.mesh, pair.mat);
                }
            });
        }

        public static RenderCommand DrawSkybox(CameraData camera, Matrix4 cameraTransform)
        {
            return new RenderCommand("Draw Geometry", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out SkyboxRenderData skyboxRenderData))
                    return;

                Skybox skybox = skyboxRenderData.skybox;

                Ogl.Disable(EnableCap.CullFace);
                Ogl.DepthFunc(DepthFunction.Lequal);

                GraphicsAPI.UseMeshMaterial(skybox.Mesh, skybox.Material);

                skybox.Material.SetMatrix("view", cameraTransform.ClearTranslation());
                skybox.Material.SetMatrix("projection", camera.CalculateProjection());

                GraphicsAPI.DrawTriangles(skybox.Mesh.GetIndiceCount());

                GraphicsAPI.FreeMeshMaterial(skybox.Mesh, skybox.Material);

                Ogl.Enable(EnableCap.CullFace);
                Ogl.DepthFunc(DepthFunction.Less);
                Ogl.BindVertexArray(0);
            });
        }

        static Vector3 m_size = new Vector3(0.00000001f, 0.00000001f, 1);
        static Vector3 m_pos = new Vector3(0, 0, 0);
        public static RenderCommand DrawUI()
        {
            return new RenderCommand("Draw Geometry", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out UICanvasRenderData canvasRenderData))
                    return;

                GraphicsAPI.UseMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);

                Matrix4 projection = canvasRenderData.Camera.CalculateProjection();
                canvasRenderData.Material.SetMatrix("projection", projection);

                List<UICanvas> canvases = canvasRenderData.GetCanvases();
                for (int i = 0; i < canvases.Count; i++)
                {
                    UICanvas canvas = canvases[i];

                    if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.N))
                        m_size.X += 0.0000001f;
                    if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.M))
                        m_size.X -= 0.0000001f;

                    if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.O))
                        m_pos.X += 1000000f;
                    if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.P))
                        m_pos.X -= 1000000f;

                    Matrix4 model = Matrix4.Identity;
                    model *= Matrix4.CreateTranslation(m_pos);
                    model *= Matrix4.CreateScale(m_size);
                    List<Vector3> verts = new List<Vector3>();
                    canvasRenderData.Mesh.GetVertices(ref verts);
                    for (int v = 0; v < verts.Count; v++)
                    {
                        Vector4 result = new Vector4(verts[v], 1) * model * projection;
                        DevConsole.Log(result);
                    }

                    canvasRenderData.Material.SetMatrix("model", model);
                    GraphicsAPI.DrawTriangles(canvasRenderData.Mesh.GetIndiceCount());

                    //DrawElement(canvasRenderData, canvas.Root, Vector2.Zero, Vector2.One * 100);
                }
                GraphicsAPI.FreeMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);
            });

            static void DrawElement(UICanvasRenderData canvasRenderData, UIElement element, Vector2 position, Vector2 size)
            {
                //position.X += element.Style.leftPadding;
                //position.Y += element.Style.bottomPadding;
                //size.X -= element.Style.rightPadding;
                //size.Y -= element.Style.topPadding;

                Matrix4 model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(new Vector3(position));
                model *= Matrix4.CreateScale(new Vector3(size));

                canvasRenderData.Material.SetMatrix("model", model);
                GraphicsAPI.DrawTriangles(canvasRenderData.Mesh.GetIndiceCount());
            }
        }
    }
}

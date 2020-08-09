using Entygine.DevTools;
using Entygine.UI;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

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
                if (!context.TryGetData(out GeometryRenderData geometryData))
                    return;

                Matrix4 projection = camera.CalculatePerspective();

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

                GL.Disable(EnableCap.CullFace);
                GL.DepthFunc(DepthFunction.Lequal);

                GraphicsAPI.UseMeshMaterial(skybox.Mesh, skybox.Material);

                skybox.Material.SetMatrix("view", cameraTransform.ClearTranslation());
                skybox.Material.SetMatrix("projection", camera.CalculatePerspective());

                GraphicsAPI.DrawTriangles(skybox.Mesh.GetIndiceCount());

                GraphicsAPI.FreeMeshMaterial(skybox.Mesh, skybox.Material);

                GL.Enable(EnableCap.CullFace);
                GL.DepthFunc(DepthFunction.Less);
                GL.BindVertexArray(0);
            });
        }

        public static RenderCommand DrawUI()
        {
            return new RenderCommand("Draw Geometry", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out UICanvasRenderData canvasRenderData))
                    return;

                GraphicsAPI.UseMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);
                List<UICanvas> canvases = canvasRenderData.GetCanvases();
                for (int i = 0; i < canvases.Count; i++)
                {
                    UICanvas canvas = canvases[i];
                    DrawElement(canvasRenderData, canvas.Root, Vector2.Zero, Vector2.One);
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

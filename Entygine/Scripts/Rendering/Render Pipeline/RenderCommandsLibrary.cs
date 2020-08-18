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
            return new RenderCommand("Draw Skybox", (ref RenderContext context) =>
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

        static Vector3 m_size = new Vector3(200f, 200f, 1f);
        static Vector3 m_pos = new Vector3(200f, 200f, 0);
        public static RenderCommand DrawUI()
        {
            return new RenderCommand("Draw UI", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out UICanvasRenderData canvasRenderData))
                    return;

                Ogl.Disable(EnableCap.CullFace);
                GraphicsAPI.UseMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);

                Matrix4 projection = canvasRenderData.Camera.CalculateProjection();
                canvasRenderData.Material.SetMatrix("projection", projection);

                List<UICanvas> canvases = canvasRenderData.GetCanvases();
                for (int i = 0; i < canvases.Count; i++)
                {
                    UICanvas canvas = canvases[i];
                    float windowWidth = MainDevWindowGL.Window.Size.X;
                    float windowHeight = MainDevWindowGL.Window.Size.Y;
                    Vector2 center = Vector2.Zero;
                    Vector2 size = new Vector2(windowWidth, windowHeight);
                    DrawElement(canvasRenderData, canvas.Root, center, size);
                }
                GraphicsAPI.FreeMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);
                Ogl.Enable(EnableCap.CullFace);
            });

            static void DrawElement(UICanvasRenderData canvasRenderData, UIElement element, Vector2 position, Vector2 size)
            {
                float windowWidth = MainDevWindowGL.Window.Size.X;
                float windowHeight = MainDevWindowGL.Window.Size.Y;

                position.X += element.Style.leftPadding;
                position.Y += element.Style.bottomPadding;
                size.X -= element.Style.rightPadding * 2;
                size.Y -= element.Style.topPadding * 2;

                Matrix4 model = Matrix4.Identity;
                model *= Matrix4.CreateScale(new Vector3(size));
                model *= Matrix4.CreateTranslation(new Vector3(position));

                canvasRenderData.Material.SetMatrix("model", model);
                GraphicsAPI.DrawTriangles(canvasRenderData.Mesh.GetIndiceCount());
            }
        }
    }
}

using Entygine.DevTools;
using Entygine.UI;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using SixLabors.ImageSharp;
using System;
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
                Ogl.Disable(EnableCap.DepthTest);
                Ogl.Enable(EnableCap.Blend);
                Ogl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GraphicsAPI.UseMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);

                Matrix4 projection = canvasRenderData.Camera.CalculateProjection();
                canvasRenderData.Material.SetMatrix("projection", projection);

                List<UICanvas> canvases = canvasRenderData.GetCanvases();
                for (int i = 0; i < canvases.Count; i++)
                {
                    UICanvas canvas = canvases[i];
                    Matrix4[] models = canvas.CalculateModels();
                    for (int m = 0; m < models.Length; m++)
                    {
                        Color01 color = new Color01(1, 1, 1, 0.4f);
                        canvasRenderData.Material.SetMatrix("model", models[m]);
                        canvasRenderData.Material.SetColor("color", color);
                        GraphicsAPI.DrawTriangles(canvasRenderData.Mesh.GetIndiceCount());
                    }
                    //DrawElement(canvasRenderData, canvas.Root, canvas.GetModelMatrix());
                }
                GraphicsAPI.FreeMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);
                Ogl.Enable(EnableCap.CullFace);
                Ogl.Enable(EnableCap.DepthTest);
                Ogl.Disable(EnableCap.Blend);
            });

            //static void DrawElement(UICanvasRenderData canvasRenderData, UIElement element, Matrix4 parentMatrix)
            //{
            //    Matrix4 model = element.GetModelMatrix(parentMatrix);
            //    canvasRenderData.Material.SetMatrix("model", model);
            //    canvasRenderData.Material.SetColor("color", element.Color);
            //    GraphicsAPI.DrawTriangles(canvasRenderData.Mesh.GetIndiceCount());
            //}
        }
    }
}

using Entygine.DevTools;
using Entygine.UI;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using System.Collections.Generic;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderCommandsLibrary
    {
        public static float yaw = 142;
        public static float pitch = -300;
        public static Vector3 LIGHT_FORWARD => new Vector3(
            (float)MathHelper.Cos(MathHelper.DegreesToRadians(yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(pitch))
            , (float)MathHelper.Sin(MathHelper.DegreesToRadians(pitch))
            , (float)MathHelper.Sin(MathHelper.DegreesToRadians(yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(pitch)));
        public static Vector3 LIGHT_FORWARD_NORMALIZED => LIGHT_FORWARD.Normalized();
        public static Vector3 LIGHT_UP => Vector3.Cross(Vector3.Cross(LIGHT_FORWARD_NORMALIZED, Vector3.UnitY), LIGHT_FORWARD_NORMALIZED).Normalized();
        public static Matrix4 LIGHT_VIEW => Matrix4.LookAt(LIGHT_FORWARD_NORMALIZED * 50, Vector3.Zero, LIGHT_UP);

        public static RenderCommand DrawGeometry(CameraData camera, Matrix4 cameraTransform)
        {
            return new RenderCommand("Draw Geometry", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out GeometryRenderData geometryData))
                    return;

                if (!context.TryGetData(out LightsRenderData lightData))
                    return;

                if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.I))
                    yaw += 0.1f;

                if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.O))
                    yaw -= 0.1f;

                if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.K))
                    pitch += 0.1f;

                if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.L))
                    pitch -= 0.1f;

                Ogl.Viewport(0, 0, MainDevWindowGL.Window.Size.X, MainDevWindowGL.Window.Size.Y);
                Ogl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                Light mainLight = lightData.lights[0];
                Matrix4 projection = camera.CalculateProjection();

                List<MeshRenderGroup> renderGroups = geometryData.GetRenderGroups();
                for (int i = 0; i < renderGroups.Count; i++)
                {
                    MeshRenderGroup renderGroup = renderGroups[i];

                    RenderMesh pair = renderGroup.MeshRender;
                    List<Matrix4> positions = renderGroup.Transforms;

                    pair.mat.SetDepthMap(mainLight.Depthmap);
                    GraphicsAPI.UseMeshMaterial(pair.mesh, pair.mat);


                    //Update camera pos for material
                    //TODO: Use 'Uniform buffer objects' to globally share this data
                    pair.mat.SetMatrix("view", cameraTransform);
                    pair.mat.SetMatrix("projection", projection);

                    pair.mat.SetVector3("directionalLight", new Vector3(1, 1, 1));
                    pair.mat.SetVector3("ambientLight", new Vector3(0.5f, 0.5f, 0.5f));
                    pair.mat.SetVector3("directionalDir", LIGHT_FORWARD_NORMALIZED);
                    pair.mat.SetMatrix("lightSpace", LIGHT_VIEW * mainLight.GetProjection());

                    for (int p = 0; p < positions.Count; p++)
                    {
                        pair.mat.SetMatrix("model", positions[p]);

                        GraphicsAPI.DrawTriangles(pair.mesh.GetIndiceCount());
                    }

                    GraphicsAPI.FreeMeshMaterial(pair.mesh, pair.mat);
                }
            });
        }

        public static RenderCommand GenerateShadowMaps()
        {
            return new RenderCommand("Calculate shadows", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out LightsRenderData lightData))
                    return;

                if (!context.TryGetData(out GeometryRenderData geometryData))
                    return;

                Ogl.Disable(EnableCap.Blend);
                List<Light> lights = lightData.lights;
                for (int i = 0; i < lights.Count; i++)
                {
                    Light light = lights[i];

                    CalculateShadowMap(light);
                }
                Ogl.Enable(EnableCap.Blend);

                Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

                void CalculateShadowMap(Light light)
                {
                    light.BindShadowMap();
                    Ogl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                    Matrix4 projection = light.GetProjection();

                    List<MeshRenderGroup> renderGroups = geometryData.GetRenderGroups();
                    for (int i = 0; i < renderGroups.Count; i++)
                    {
                        MeshRenderGroup renderGroup = renderGroups[i];

                        RenderMesh pair = renderGroup.MeshRender;
                        List<Matrix4> positions = renderGroup.Transforms;

                        GraphicsAPI.UseMeshMaterial(pair.mesh, lightData.depthMat);

                        lightData.depthMat.SetMatrix("view", LIGHT_VIEW);
                        lightData.depthMat.SetMatrix("projection", projection);

                        for (int p = 0; p < positions.Count; p++)
                        {
                            lightData.depthMat.SetMatrix("model", positions[p]);

                            GraphicsAPI.DrawTriangles(pair.mesh.GetIndiceCount());
                        }

                        GraphicsAPI.FreeMeshMaterial(pair.mesh, lightData.depthMat);
                    }
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
                    canvas.UpdateRenderers();
                    var renderables = canvas.GetRenderables();
                    for (int m = 0; m < renderables.Count; m++)
                    {
                        UI_IRenderable currRenderable = renderables[m];
                        GraphicsAPI.UseMeshMaterial(canvasRenderData.Mesh, currRenderable.Material);
                        canvasRenderData.Material.SetMatrix("model", currRenderable.Rect.GetModelMatrix());
                        canvasRenderData.Material.SetColor("color", currRenderable.Color);
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

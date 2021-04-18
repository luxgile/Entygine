using Entygine.DevTools;
using Entygine.UI;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Entygine.Rendering.Pipeline
{
    public static partial class RenderCommandsLibrary
    {
        private static float yaw = 142;
        private static float pitch = -300;
        private static Vector3 LIGHT_FORWARD => new Vector3(
            (float)MathHelper.Cos(MathHelper.DegreesToRadians(yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(pitch))
            , (float)MathHelper.Sin(MathHelper.DegreesToRadians(pitch))
            , (float)MathHelper.Sin(MathHelper.DegreesToRadians(yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(pitch)));
        private static Vector3 LIGHT_FORWARD_NORMALIZED => LIGHT_FORWARD.Normalized();
        private static Vector3 LIGHT_UP => Vector3.Cross(Vector3.Cross(LIGHT_FORWARD_NORMALIZED, Vector3.UnitY), LIGHT_FORWARD_NORMALIZED).Normalized();
        private static Matrix4 LIGHT_VIEW => Matrix4.LookAt(LIGHT_FORWARD_NORMALIZED * 50, Vector3.Zero, LIGHT_UP);

        public static RenderCommand DrawGeometry(CameraData camera, Matrix4 cameraTransform)
        {
            return new RenderCommand("Draw Geometry", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out GeometryRenderData geometryData))
                    return;

                if (!context.TryGetData(out LightsRenderData lightData))
                    return;

                //if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.I))
                //    yaw += 0.1f;

                //if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.O))
                //    yaw -= 0.1f;

                //if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.K))
                //    pitch += 0.1f;

                //if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.L))
                //    pitch -= 0.1f;

                Ogl.Viewport(0, 0, AppScreen.Resolution.x, AppScreen.Resolution.y);
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
    }
}

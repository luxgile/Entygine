﻿using Entygine.Ecs;
using Entygine.Ecs.Components;
using Entygine.Ecs.Systems;
using Entygine.Rendering;
using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Windowing.Common.Input;
using Entygine.DevTools;
using Entygine.Cycles;
using Entygine.Rendering.Pipeline;

namespace Entygine
{
    public class MainDevWindowGL : GameWindow
    {
        private WorkerCycleCore coreWorker;

        public MainDevWindowGL(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Window = this;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            DevConsole.AddLogger(new ConsoleLoggerFile());
            DevConsole.AddLogger(new NativeConsoleLogger());

            DevConsole.Log("Creating Entity World...");

            coreWorker = new WorkerCycleCore();

            EntityWorld world = EntityWorld.CreateWorld();
            world.Runner.AssignToWorker(coreWorker).CreateSystemsAuto(world);
            EntityWorld.SetActive(world);

            RenderPipelineCore.SetPipeline(new DefaultRenderPipeline());

            Cubemap skyboxCubemap = new Cubemap(new string[]
            {
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\right.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\left.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\top.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\bottom.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\front.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\back.png"),
            });

            Shader skyboxShader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\skybox.vert"),
                                                AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\skybox.frag"));
            Skybox skybox = new Skybox(new Material(skyboxShader, skyboxCubemap));
            RenderPipelineCore.SetSkybox(skybox);

            Mesh meshResource = MeshPrimitives.CreateCube(1);
            Shader shaderResource = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\standard.vert"),
                                                AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\standard.frag"));

            Texture2D texture = new Texture2D(AssetBrowser.Utilities.LocalToAbsolutePath(@"Textures\Box.png"));
            Material materialResource = new Material(shaderResource, texture);
            SC_RenderMesh renderMesh = new SC_RenderMesh(meshResource, materialResource);

            EntityArchetype meshArchetype = new EntityArchetype(typeof(SC_RenderMesh), typeof(C_Transform));

            Entity planeEntity = world.EntityManager.CreateEntity(meshArchetype);
            world.EntityManager.SetComponent(planeEntity, new C_Transform() { value = Matrix4.CreateTranslation(new Vector3(0, 0, 0)) });
            Mesh planeMesh = MeshPrimitives.CreatePlane(50);
            Material planeMaterial = new Material(shaderResource, Texture2D.CreateWhiteTexture(64, 64));
            world.EntityManager.SetSharedComponent(planeEntity, new SC_RenderMesh(planeMesh, planeMaterial));

            Entity boxEntity = world.EntityManager.CreateEntity(meshArchetype);
            world.EntityManager.SetComponent(boxEntity, new C_Transform() { value = Matrix4.CreateTranslation(new Vector3(0, 2, 0)) });
            world.EntityManager.SetSharedComponent(boxEntity, renderMesh);

            Entity boxEntity2 = world.EntityManager.CreateEntity(meshArchetype);
            world.EntityManager.SetComponent(boxEntity2, new C_Transform() { value = Matrix4.CreateTranslation(new Vector3(2, 2, 0)) });
            world.EntityManager.SetSharedComponent(boxEntity2, renderMesh);

            Entity boxEntity3 = world.EntityManager.CreateEntity(meshArchetype);
            world.EntityManager.SetComponent(boxEntity3, new C_Transform() { value = Matrix4.CreateTranslation(new Vector3(-2, 2, 0)) });
            world.EntityManager.SetSharedComponent(boxEntity3, renderMesh);

            EntityArchetype editorCameraArchetype = new EntityArchetype(typeof(C_Camera), typeof(C_Transform), typeof(C_EditorCamera));
            Entity cameraEditorEntity = world.EntityManager.CreateEntity(editorCameraArchetype);
            world.EntityManager.SetComponent(cameraEditorEntity, new C_Camera() { cameraData = new CameraData(45f, 800f / 600f, 0.1f, 100f) });

            Vector3 cameraPos = new Vector3(0, 2, 5);
            world.EntityManager.SetComponent(cameraEditorEntity, new C_Transform() { value = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY) });
            world.EntityManager.SetComponent(cameraEditorEntity, new C_EditorCamera() { speed = 0.2f, dir = -Vector3.UnitZ, pos = cameraPos });

            DevConsole.Log("Entity world created.");

            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            coreWorker.PerformLogicCycle();

            if (KeyboardState.IsKeyDown(Key.Space))
                EntityWorld.Active.DEBUG_LOG_INFO();

            if (KeyboardState.IsKeyDown(Key.Escape))
                Close();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            coreWorker.PerformRenderCycle();

            SwapBuffers();
        }

        public static MainDevWindowGL Window { get; private set; }
    }
}

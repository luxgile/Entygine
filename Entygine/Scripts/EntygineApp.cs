using Entygine.Cycles;
using Entygine.DevTools;
using Entygine.Ecs;
using Entygine.Ecs.Components;
using Entygine.Mathematics;
using Entygine.Physics;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;
using Entygine.UI;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Entygine
{
    public static class EntygineApp
    {
        private static WorkerCycleCore coreWorker;
        private static bool loadedEngine;

        internal static void StartEngine()
        {

            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();

            //Everything works but there are some blinking at the start
            //gameWindowSettings.IsMultiThreaded = true;
            gameWindowSettings.UpdateFrequency = 60.0d;
            nativeWindowSettings.Title = "Entygine";
            nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(1600, 900);

            using MainDevWindowGL mainWindow = new MainDevWindowGL(gameWindowSettings, nativeWindowSettings);

            DevConsole.Log("Engine started succesfully.");

            mainWindow.Load += LoadEngine;
            mainWindow.UpdateFrame += UpdateFrame;
            mainWindow.RenderFrame += RenderFrame;

            mainWindow.Run();
        }

        public static void LoadEngine()
        {
            if (loadedEngine)
                return;

            loadedEngine = true;

            Ogl.enableErrorCheck = true;

            InitConsole();

            DevConsole.Log("Creating Entity World...");

            coreWorker = new WorkerCycleCore();
            EntityWorld world = InitEcs();

            RenderPipelineCore.SetPipeline(new ForwardRenderPipeline());

            InitPhysics();
            InitSkybox();
            InitUI(world);
            InitLight();
            InitScene(world);

            DevConsole.Log("Entity world created.");

            Ogl.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            Ogl.Enable(EnableCap.DepthTest);
            Ogl.Enable(EnableCap.CullFace);
            Ogl.Enable(EnableCap.ProgramPointSize);
            Ogl.PointSize(10f);

            EntityWorld InitEcs()
            {
                EntityWorld world = EntityWorld.CreateWorld();
                world.Runner.AssignToWorker(coreWorker).CreateSystemsAuto(world);
                EntityWorld.SetActive(world);
                return world;
            }

            void InitPhysics()
            {
                PhysicsRunner physicsRunner = new PhysicsRunner();
                PhysicsWorld world = new PhysicsWorld();
                PhysicsWorld.SetDefaultWorld(world);
                physicsRunner.AddWorld(world);
                physicsRunner.AssignToWorker(coreWorker);
            }

            void InitScene(EntityWorld world)
            {
                Mesh meshResource = MeshPrimitives.CreateCube(1);
                Shader shaderResource = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\standard.vert"),
                                                    AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\standard.frag"));

                Texture2D texture = new Texture2D(AssetBrowser.Utilities.LocalToAbsolutePath(@"Textures\Box.png"));
                Material materialResource = new Material(shaderResource, texture);
                SC_RenderMesh renderMesh = new SC_RenderMesh(meshResource, materialResource);

                EntityArchetype meshArchetype = new EntityArchetype(typeof(SC_RenderMesh), typeof(C_Transform), typeof(C_Position));

                Entity planeEntity = world.EntityManager.CreateEntity(meshArchetype);
                world.EntityManager.SetComponent(planeEntity, new C_Position() { value = new Vec3f(0, 0, 0) });
                //world.EntityManager.SetComponent(planeEntity, new C_PhysicsBody() { body = new PhysicBody() { isStatic = true } });
                Mesh planeMesh = MeshPrimitives.CreatePlaneXZ(50);
                Material planeMaterial = new Material(shaderResource, Texture2D.CreateWhiteTexture(64, 64));
                world.EntityManager.SetSharedComponent(planeEntity, new SC_RenderMesh(planeMesh, planeMaterial));

                Entity planeEntity2 = world.EntityManager.CreateEntity(meshArchetype);
                world.EntityManager.SetComponent(planeEntity2, new C_Position() { value = new Vec3f(0, 5, -10) });
                //world.EntityManager.SetComponent(planeEntity2, new C_PhysicsBody() { body = new PhysicBody() { isStatic = true } });

                Mesh planeMesh2 = MeshPrimitives.CreatePlaneXY(10);
                Material planeMaterial2 = new Material(shaderResource, texture);
                world.EntityManager.SetSharedComponent(planeEntity2, new SC_RenderMesh(planeMesh2, planeMaterial2));

                Entity boxEntity = world.EntityManager.CreateEntity(meshArchetype);
                world.EntityManager.SetComponent(boxEntity, new C_Position() { value = new Vec3f(0, 2, 0) });
                //world.EntityManager.SetComponent(boxEntity, new C_PhysicsBody() { body = new PhysicBody() });
                world.EntityManager.SetSharedComponent(boxEntity, renderMesh);

                Entity boxEntity2 = world.EntityManager.CreateEntity(meshArchetype);
                world.EntityManager.SetComponent(boxEntity2, new C_Position() { value = new Vec3f(2, 2, 0) });
                //world.EntityManager.SetComponent(boxEntity2, new C_PhysicsBody() { body = new PhysicBody() });
                world.EntityManager.SetSharedComponent(boxEntity2, renderMesh);

                Entity boxEntity3 = world.EntityManager.CreateEntity(meshArchetype);
                world.EntityManager.SetComponent(boxEntity3, new C_Position() { value = new Vec3f(-2, 2, 0) });
                //world.EntityManager.SetComponent(boxEntity3, new C_PhysicsBody() { body = new PhysicBody() });

                world.EntityManager.SetSharedComponent(boxEntity3, renderMesh);

                EntityArchetype editorCameraArchetype = new EntityArchetype(typeof(C_Camera), typeof(C_Transform), typeof(C_EditorCamera));
                Entity cameraEditorEntity = world.EntityManager.CreateEntity(editorCameraArchetype);
                world.EntityManager.SetComponent(cameraEditorEntity, new C_Camera() { cameraData = CameraData.CreatePerpectiveCamera(45f, 800f / 600f, 0.1f, 100f) });

                world.EntityManager.SetComponent(cameraEditorEntity, new C_EditorCamera()
                { speed = 10f, focusPoint = new Vector3(0, 2, 0), focusDistance = 12, pitch = 20, yaw = 115, sensitivity = 20 });
            }

            void InitLight()
            {
                if (RenderPipelineCore.TryGetContext(out LightsRenderData lightData))
                    lightData.lights.Add(new DirectionalLight());
            }

            void InitUI(EntityWorld world)
            {
                EntityArchetype canvasArch = new EntityArchetype(typeof(C_UICanvas));
                var entity = world.EntityManager.CreateEntity(canvasArch);
                world.EntityManager.SetComponent(entity, new C_UICanvas() { canvas = new UICanvas() });
            }

            static void InitSkybox()
            {
                Cubemap skyboxCubemap = new Cubemap(new string[]
                            {
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\right.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\left.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\bottom.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\top.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\front.png"),
                AssetBrowser.Utilities.LocalToAbsolutePath(@"Skybox\back.png"),
                            });

                Shader skyboxShader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\skybox.vert"),
                                                    AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\skybox.frag"));
                Skybox skybox = new Skybox(new Material(skyboxShader, skyboxCubemap));
                RenderPipelineCore.SetSkybox(skybox);
            }

            static void InitConsole()
            {
                DevConsole.AddLogger(new ConsoleLoggerFile());
                DevConsole.AddLogger(new NativeConsoleLogger());
            }
        }

        private static void UpdateFrame(FrameEventArgs e)
        {
            FrameContext.Current = new FrameData(FrameContext.Current.count + 1, (float)e.Time);

            coreWorker.PerformLogicCycle((float)e.Time);

            if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(Keys.Space))
                EntityWorld.Active.DEBUG_LOG_INFO();

            if (MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                MainDevWindowGL.Window.Close();
        }

        private static void RenderFrame(FrameEventArgs e)
        {
            coreWorker.PerformRenderCycle((float)e.Time);

            MainDevWindowGL.Window.SwapBuffers();
        }
    }
}

using Entygine.Ecs;
using Entygine.Ecs.Components;
using Entygine.Ecs.Systems;
using Entygine.Rendering;
using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using System;

namespace Entygine
{
    public class MainDevWindow : GameWindow
    {
        public MainDevWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            EntityWorld world = EntityWorld.CreateWorld();
            EntityWorld.SetActive(world);

            EntityArchetype meshArchetype = new EntityArchetype(typeof(SC_RenderMesh), typeof(C_Transform));
            Entity boxEntity = world.EntityManager.CreateEntity(meshArchetype);
            world.EntityManager.SetComponent(boxEntity, new C_Transform() { value = Matrix4.CreateTranslation(new Vector3(0, 5, 0)) });
            Mesh meshResource = MeshPrimitives.CreateCube(1);
            Shader shaderResource = new Shader
                (@"D:\Development\VS 2019\Entygine\Entygine\Assets\Shaders\standard.vert",
                @"D:\Development\VS 2019\Entygine\Entygine\Assets\Shaders\standard.frag");
            Texture2D texture = new Texture2D
                (@"D:\Development\VS 2019\Entygine\Entygine\Assets\Textures\Box.png");
            Material materialResource = new Material(shaderResource, texture);
            SC_RenderMesh renderMesh = new SC_RenderMesh(meshResource, materialResource);
            world.EntityManager.SetSharedComponent(boxEntity, renderMesh);

            //Entity planeEntity = world.EntityManager.CreateEntity(meshArchetype);
            //world.EntityManager.SetComponent(planeEntity, new C_Transform() { value = Matrix4.CreateTranslation(new Vector3(0, 0, 0)) });
            //Mesh planeMesh = MeshPrimitives.CreatePlane(100);
            //Material planeMaterial = new Material(shaderResource, Texture.CreateWhiteTexture(1, 1));
            //world.EntityManager.SetSharedComponent(planeEntity, new SC_RenderMesh(planeMesh, planeMaterial));

            EntityArchetype editorCameraArchetype = new EntityArchetype(typeof(C_Camera), typeof(C_Transform), typeof(C_EditorCamera));
            Entity cameraEditorEntity = world.EntityManager.CreateEntity(editorCameraArchetype);
            world.EntityManager.SetComponent(cameraEditorEntity, new C_Camera() { cameraData = new CameraData(45f, 800f / 600f, 0.1f, 100f) });
            world.EntityManager.SetComponent(cameraEditorEntity, new C_Transform() { value = Matrix4.CreateTranslation(0, -5, -5) });
            world.EntityManager.SetComponent(cameraEditorEntity, new C_EditorCamera() { speed = 0.5f });

            world.CreateRenderSystem<S_RenderMesh>();
            world.CreateRenderSystem<S_DrawCameras>();
            world.CreateLogicSystem<S_EditorCameraControl>();

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            EntityWorld.Active.PerformLogic();

            //KeyboardState input = Keyboard.GetState();
            //if (input.IsKeyDown(Key.Escape))
            //    Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            EntityWorld.Active.PerformRender();

            SwapBuffers();
        }
    }
}

using Entygine.Ecs.Components;
using OpenToolkit.Mathematics;
using System.Collections.Generic;
using OpenToolkit.Windowing.Common.Input;
using OpenToolkit.Windowing.Desktop;

namespace Entygine.Ecs.Systems
{
    public class S_GameCameraControl : LogicSystem
    {
        private readonly EntityArchetype cameraArchetype = new EntityArchetype(typeof(C_Camera), typeof(C_Transform), typeof(C_EditorCamera));

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            float speedDelta = 0;
            Vector3 posDelta = Vector3.Zero;

            //TODO: Create input system
            KeyboardState input = MainDevWindowGL.Window.KeyboardState;
            if (input.IsKeyDown(Key.LShift))
                speedDelta += 0.01f;

            if (input.IsKeyDown(Key.LControl))
                speedDelta -= 0.01f;

            if (input.IsKeyDown(Key.W))
                posDelta += Vector3.UnitZ;

            if (input.IsKeyDown(Key.S))
                posDelta -= Vector3.UnitZ;

            if (input.IsKeyDown(Key.A))
                posDelta += Vector3.UnitX;

            if (input.IsKeyDown(Key.D))
                posDelta -= Vector3.UnitX;

            if (speedDelta == 0 && posDelta == Vector3.Zero)
                return;

            List<EntityChunk> chunks = World.EntityManager.GetChunksWith(cameraArchetype, false);
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (chunk.HasChanged(LastVersionWorked))
                {
                    chunk.ChunkVersion = World.EntityManager.Version;
                    if (chunk.TryGetComponents<C_Transform>(out ComponentArray transforms) && chunk.TryGetComponents<C_EditorCamera>(out ComponentArray cameras))
                    {
                        for (int c = 0; c < chunk.Count; c++)
                        {
                            C_Transform transform = (C_Transform)transforms[c];
                            C_EditorCamera editorCamera = (C_EditorCamera)cameras[c];

                            editorCamera.speed += speedDelta;
                            editorCamera.speed = MathHelper.Clamp(editorCamera.speed, 0, 100);

                            Vector3 position = transform.value.ExtractTranslation();
                            position += posDelta * editorCamera.speed;
                            transform.value.Row3 = new Vector4(position, 1);

                            transforms[c] = transform;
                            cameras[c] = editorCamera;
                        }
                    }
                }
            }
        }
    }
}

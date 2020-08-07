using Entygine.Ecs.Components;
using OpenToolkit.Mathematics;
using System.Collections.Generic;
using OpenToolkit.Windowing.Common.Input;
using Entygine.DevTools;
using System.Drawing.Drawing2D;

namespace Entygine.Ecs.Systems
{
    public class S_EditorCameraControl : BaseSystem
    {
        private readonly EntityArchetype cameraArchetype = new EntityArchetype(typeof(C_Camera), typeof(C_Transform), typeof(C_EditorCamera));

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            float speedDelta = 0;
            Vector3 posDelta = Vector3.Zero;
            float yRotDelta = 0;

            //TODO: Create input system
            KeyboardState input = MainDevWindowGL.Window.KeyboardState;
            if (input.IsKeyDown(Key.LShift))
                speedDelta += 0.01f;

            if (input.IsKeyDown(Key.LControl))
                speedDelta -= 0.01f;

            if (input.IsKeyDown(Key.W))
                posDelta -= Vector3.UnitZ;

            if (input.IsKeyDown(Key.S))
                posDelta += Vector3.UnitZ;

            if (input.IsKeyDown(Key.A))
                posDelta -= Vector3.UnitX;

            if (input.IsKeyDown(Key.D))
                posDelta += Vector3.UnitX;

            if (input.IsKeyDown(Key.E))
                posDelta += Vector3.UnitY;

            if (input.IsKeyDown(Key.Q))
                posDelta -= Vector3.UnitY;

            if (input.IsKeyDown(Key.Z))
                yRotDelta += 0.01f;

            if (input.IsKeyDown(Key.X))
                yRotDelta -= 0.01f;

            if (speedDelta == 0 && posDelta == Vector3.Zero && yRotDelta == 0)
                return;

            List<int> chunksIndexes = World.EntityManager.GetChunksWith(cameraArchetype, false);
            for (int i = 0; i < chunksIndexes.Count; i++)
            {
                ref EntityChunk chunk = ref World.EntityManager.GetChunk(chunksIndexes[i]);
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

                            editorCamera.pos += posDelta * editorCamera.speed;

                            Vector3 right = Vector3.Normalize(Vector3.Cross(editorCamera.dir, Vector3.UnitY));
                            Vector3 up = Vector3.Normalize(Vector3.Cross(right, editorCamera.dir));
                            transform.value = Matrix4.LookAt(editorCamera.pos, editorCamera.pos + editorCamera.dir, up);

                            transforms[c] = transform;
                            cameras[c] = editorCamera;
                        }
                    }
                }
            }
        }
    }
}

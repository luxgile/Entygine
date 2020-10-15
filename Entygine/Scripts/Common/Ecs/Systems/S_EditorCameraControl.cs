﻿using Entygine.Ecs.Components;
using OpenToolkit.Mathematics;
using System.Collections.Generic;
using OpenToolkit.Windowing.Common.Input;
using Entygine.DevTools;
using Entygine.Mathematics;

namespace Entygine.Ecs.Systems
{
    public class S_EditorCameraControl : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        private float scrollDelta;
        private Vector2 lastCursorPos;

        protected override void OnSystemCreated()
        {
            base.OnSystemCreated();

            MainDevWindowGL.Window.MouseWheel += (x) => scrollDelta = x.OffsetY;
        }

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            float speedDelta = 0;
            Vector3 posDelta = Vector3.Zero;
            Vector2 rotDelta = Vector2.Zero;

            //TODO: Create input system
            KeyboardState input = MainDevWindowGL.Window.KeyboardState;
            if (input.IsKeyDown(Key.LShift))
                speedDelta += 0.01f;

            if (input.IsKeyDown(Key.LControl))
                speedDelta -= 0.01f;

            Vector2 cursorPos = MainDevWindowGL.Window.MouseState.Position;

            if (input.IsKeyDown(Key.AltLeft) && MainDevWindowGL.Window.IsMouseButtonDown(MouseButton.Button1))
                rotDelta = cursorPos - lastCursorPos;

            lastCursorPos = cursorPos;

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

            Iterator iterator = new Iterator()
            {
                deltaTime = dt,
                posDelta = posDelta,
                speedDelta = speedDelta,
                rotDelta = rotDelta,
                distDelta = scrollDelta,
            };

            query.With(TypeCache.WriteType(typeof(C_Transform)), TypeCache.WriteType(typeof(C_EditorCamera)));
            IterateQuery(iterator, query, false);

            scrollDelta = 0;
        }

        private struct Iterator : IQueryEntityIterator
        {
            public float deltaTime;
            public float speedDelta;
            public float distDelta;
            public Vector3 posDelta;
            public Vector2 rotDelta;

            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (!chunk.TryGetComponent(index, out C_EditorCamera editorCamera) || !chunk.TryGetComponent(index, out C_Transform transform))
                    return;

                editorCamera.speed += speedDelta * deltaTime;
                editorCamera.speed = MathHelper.Clamp(editorCamera.speed, 0, 100);

                editorCamera.focusDistance -= distDelta * deltaTime * 100;

                editorCamera.yaw += rotDelta.X * deltaTime * editorCamera.sensitivity;
                editorCamera.pitch += rotDelta.Y * deltaTime * editorCamera.sensitivity;

                Vector3 dir = new Vector3(
                    (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.pitch))
                  , (float)MathHelper.Sin(MathHelper.DegreesToRadians(editorCamera.pitch))
                  , (float)MathHelper.Sin(MathHelper.DegreesToRadians(editorCamera.yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.pitch)));

                Vector3 right = Vector3.Normalize(Vector3.Cross(dir, Vector3.UnitY));
                Vector3 up = Vector3.Normalize(Vector3.Cross(right, dir));

                Vector3 posDeltaRelative = -right * posDelta.X + dir * posDelta.Z + Vector3.UnitY * posDelta.Y;
                posDeltaRelative *= deltaTime;

                editorCamera.focusPoint += posDeltaRelative * editorCamera.speed;
                transform.value = Matrix4.LookAt(editorCamera.focusPoint + dir * editorCamera.focusDistance, editorCamera.focusPoint, up);

                //DevGizmos.DrawPoint((Vec3f)editorCamera.focusPoint);
                DevGizmos.DrawLine((Vec3f)editorCamera.focusPoint, (Vec3f)editorCamera.focusPoint + Vec3f.Right);

                chunk.SetComponent(index, transform);
                chunk.SetComponent(index, editorCamera);
            }
        }
    }
}

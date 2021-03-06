﻿using Entygine.Ecs.Components;
using OpenTK.Mathematics;
using Entygine.DevTools;
using Entygine.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Entygine.Input;

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

            //MainDevWindowGL.Window.MouseWheel += (x) => scrollDelta = x.OffsetY;
        }

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            float speedDelta = 0;
            Vector3 posDelta = Vector3.Zero;
            Vector2 rotDelta = Vector2.Zero;

            //TODO: Create input system
            KeyboardState keyboard = AppInput.CurrentInput.keyboard;
            if (keyboard.IsKeyDown(Keys.LeftShift))
                speedDelta += 0.01f;

            if (keyboard.IsKeyDown(Keys.LeftControl))
                speedDelta -= 0.01f;

            MouseState mouse = AppInput.CurrentInput.mouse;
            Vector2 cursorPos = mouse.Position;

            scrollDelta = mouse.ScrollDelta.Y;

            if (keyboard.IsKeyDown(Keys.LeftAlt) && mouse.IsButtonDown(MouseButton.Button1))
                rotDelta = cursorPos - lastCursorPos;

            lastCursorPos = cursorPos;

            if (keyboard.IsKeyDown(Keys.W))
                posDelta -= Vector3.UnitZ;

            if (keyboard.IsKeyDown(Keys.S))
                posDelta += Vector3.UnitZ;

            if (keyboard.IsKeyDown(Keys.A))
                posDelta -= Vector3.UnitX;

            if (keyboard.IsKeyDown(Keys.D))
                posDelta += Vector3.UnitX;

            if (keyboard.IsKeyDown(Keys.E))
                posDelta += Vector3.UnitY;

            if (keyboard.IsKeyDown(Keys.Q))
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
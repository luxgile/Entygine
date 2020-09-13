﻿using Entygine.Ecs.Components;
using OpenToolkit.Mathematics;
using System.Collections.Generic;
using OpenToolkit.Windowing.Common.Input;

namespace Entygine.Ecs.Systems
{
    public class S_EditorCameraControl : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        private float scrollDelta;

        protected override void OnSystemCreated()
        {
            base.OnSystemCreated();

            MainDevWindowGL.Window.MouseWheel += (x) => scrollDelta = x.OffsetY;
        }

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            float speedDelta = 0;
            Vector3 posDelta = Vector3.Zero;
            Vector2 rotDelta = Vector2.Zero;

            //TODO: Create input system
            KeyboardState input = MainDevWindowGL.Window.KeyboardState;
            if (input.IsKeyDown(Key.LShift))
                speedDelta += 0.01f;

            if (input.IsKeyDown(Key.LControl))
                speedDelta -= 0.01f;

            if(input.IsKeyDown(Key.AltLeft) && MainDevWindowGL.Window.IsMouseButtonDown(MouseButton.Button1))
                rotDelta = MainDevWindowGL.Window.MouseDelta;

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
                posDelta = posDelta,
                speedDelta = speedDelta,
                rotDelta = rotDelta,
                distDelta = scrollDelta,
            };

            query.With(TypeCache.WriteType(typeof(C_Transform)), TypeCache.WriteType(typeof(C_EditorCamera)));
            IterateQuery(iterator, query);
            scrollDelta = 0;
        }

        private struct Iterator : IQueryEntityIterator
        {
            public float speedDelta;
            public float distDelta;
            public Vector3 posDelta;
            public Vector2 rotDelta;

            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (!chunk.TryGetComponent(index, out C_EditorCamera editorCamera) || !chunk.TryGetComponent(index, out C_Transform transform))
                    return;

                editorCamera.speed += speedDelta;
                editorCamera.speed = MathHelper.Clamp(editorCamera.speed, 0, 100);

                editorCamera.focusDistance -= distDelta;

                editorCamera.yaw -= rotDelta.X;
                editorCamera.pitch -= rotDelta.Y;

                Vector3 dir = new Vector3(
                    (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.pitch))
                  , (float)MathHelper.Sin(MathHelper.DegreesToRadians(editorCamera.pitch))
                  , (float)MathHelper.Sin(MathHelper.DegreesToRadians(editorCamera.yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.pitch)));

                Vector3 right = Vector3.Normalize(Vector3.Cross(dir, Vector3.UnitY));
                Vector3 up = Vector3.Normalize(Vector3.Cross(right, dir));

                Vector3 posDeltaRelative = -right * posDelta.X + dir * posDelta.Z + Vector3.UnitY * posDelta.Y;
                editorCamera.focusPoint += posDeltaRelative * editorCamera.speed;
                transform.value = Matrix4.LookAt(editorCamera.focusPoint + dir * editorCamera.focusDistance, editorCamera.focusPoint, up);

                chunk.SetComponent(index, transform);
                chunk.SetComponent(index, editorCamera);
            }
        }
    }
}

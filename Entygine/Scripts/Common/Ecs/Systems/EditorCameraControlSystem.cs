using Entygine.Ecs.Components;
using OpenTK.Mathematics;
using Entygine.DevTools;
using Entygine.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Entygine.Input;

namespace Entygine.Ecs.Systems
{
    internal class EditorCameraControlSystem : QuerySystem
    {
        private Vector2 lastCursorPos;

        protected override bool CheckChanges => false;

        protected override void OnFrame(float dt)
        {
            var speedDelta = 0f;
            var posDelta = Vector3.Zero;
            var rotDelta = Vector2.Zero;

            //TODO: Create input system
            KeyboardState keyboard = AppInput.CurrentInput.keyboard;
            if (keyboard.IsKeyDown(Keys.LeftShift))
                speedDelta += 0.01f;

            if (keyboard.IsKeyDown(Keys.LeftControl))
                speedDelta -= 0.01f;

            MouseState mouse = AppInput.CurrentInput.mouse;
            Vector2 cursorPos = mouse.Position;

            var scrollDelta = mouse.ScrollDelta.Y;

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

            Iterator.SetName("Editor Camera Control").Iterate((ref C_EditorCamera editorCamera, ref C_Transform transform) =>
            {
                editorCamera.speed += speedDelta * dt;
                editorCamera.speed = MathHelper.Clamp(editorCamera.speed, 0, 100);

                editorCamera.focusDistance -= scrollDelta * dt * 100;

                editorCamera.yaw += rotDelta.X * dt * editorCamera.sensitivity;
                editorCamera.pitch += rotDelta.Y * dt * editorCamera.sensitivity;

                Vector3 dir = new(
                    (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.pitch))
                  , (float)MathHelper.Sin(MathHelper.DegreesToRadians(editorCamera.pitch))
                  , (float)MathHelper.Sin(MathHelper.DegreesToRadians(editorCamera.yaw)) * (float)MathHelper.Cos(MathHelper.DegreesToRadians(editorCamera.pitch)));

                Vector3 right = Vector3.Normalize(Vector3.Cross(dir, Vector3.UnitY));
                Vector3 up = Vector3.Normalize(Vector3.Cross(right, dir));

                Vector3 posDeltaRelative = -right * posDelta.X + dir * posDelta.Z + Vector3.UnitY * posDelta.Y;
                posDeltaRelative *= dt;

                editorCamera.focusPoint += posDeltaRelative * editorCamera.speed;
                transform.value = Matrix4.LookAt(editorCamera.focusPoint + dir * editorCamera.focusDistance, editorCamera.focusPoint, up);

                //DevGizmos.DrawPoint((Vec3f)editorCamera.focusPoint);
                DevGizmos.DrawLine((Vec3f)editorCamera.focusPoint, (Vec3f)editorCamera.focusPoint + Vec3f.Right);
            });
        }
    }
}
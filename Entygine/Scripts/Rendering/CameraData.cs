using OpenToolkit.Mathematics;
using SixLabors.ImageSharp;

namespace Entygine.Rendering
{
    public enum RenderingMode { Orthographic, Perspective, }

    public struct CameraData
    {
        public RenderingMode mode;
        public float nearPlane;
        public float farPlane;

        //Perspective
        public float fov;
        public float aspectRatio;

        //Orthographic
        public float orthoSize;

        public static CameraData CreatePerpectiveCamera(float fov, float ratio, float nearPlane, float farPlane)
        {
            return new CameraData
            {
                fov = fov,
                aspectRatio = ratio,
                nearPlane = nearPlane,
                farPlane = farPlane,
                mode = RenderingMode.Perspective
            };
        }

        public static CameraData CreateOrthographicCamera(float aspectRatio, float orthoSize, float nearPlane, float farPlane)
        {
            return new CameraData
            {
                aspectRatio = aspectRatio,
                nearPlane = nearPlane,
                farPlane = farPlane,
                orthoSize = orthoSize,
                mode = RenderingMode.Orthographic
            };
        }

        public void SetOrthoResolution(float width, float height)
        {
            aspectRatio = width / height;
            orthoSize = height / 2;
        }

        public float OrthoHeight => orthoSize * 2f;
        public float OrthoWidth => OrthoHeight * aspectRatio;

        public float RadiansFov => MathHelper.DegreesToRadians(fov);

        public Matrix4 CalculateProjection()
        {
            if (mode == RenderingMode.Perspective)
                return Matrix4.CreatePerspectiveFieldOfView(RadiansFov, aspectRatio, nearPlane, farPlane);
            else
                return Matrix4.CreateOrthographicOffCenter(0.0f, OrthoWidth, 0.0f, OrthoHeight, nearPlane, farPlane);
        }
    }
}

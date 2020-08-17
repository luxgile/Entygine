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
        public float width;
        public float height;

        public static CameraData CreatePerpectiveCamera(float fov, float ratio, float nearPlane, float farPlane)
        {
            CameraData camera = new CameraData();
            camera.fov = fov;
            camera.aspectRatio = ratio;
            camera.nearPlane = nearPlane;
            camera.farPlane = farPlane;
            camera.mode = RenderingMode.Perspective;
            return camera;
        }

        public static CameraData CreateOrthographicCamera(float width, float height, float nearPlane, float farPlane)
        {
            CameraData camera = new CameraData();
            camera.width = width;
            camera.height = height;
            camera.nearPlane = nearPlane;
            camera.farPlane = farPlane;
            camera.mode = RenderingMode.Orthographic;
            return camera;
        }

        public float RadiansFov => MathHelper.DegreesToRadians(fov);

        public Matrix4 CalculateProjection()
        {
            if (mode == RenderingMode.Perspective)
                return Matrix4.CreatePerspectiveFieldOfView(RadiansFov, aspectRatio, nearPlane, farPlane);
            else
                return Matrix4.CreateOrthographicOffCenter(0, width, 0, height, nearPlane, farPlane);
        }
    }
}

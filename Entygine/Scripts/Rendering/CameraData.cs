using OpenToolkit.Mathematics;
using SixLabors.ImageSharp;

namespace Entygine.Rendering
{
    public struct CameraData
    {
        public float fov;
        public float aspectRatio;
        public float nearPlane;
        public float farPlane;

        public CameraData(float fov, float ratio, float nearPlane, float farPlane) : this()
        {
            this.fov = fov;
            this.aspectRatio = ratio;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;
        }

        public float RadiansFov => MathHelper.DegreesToRadians(fov);

        public Matrix4 CalculatePerspective()
        {
            return Matrix4.CreatePerspectiveFieldOfView(RadiansFov, aspectRatio, nearPlane, farPlane);
        }
    }
}

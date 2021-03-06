﻿using OpenTK.Mathematics;
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

        private Framebuffer finalFramebuffer;
        private Framebuffer framebuffer;

        //private Texture2D colorTargetTexture;
        //private DepthTexture depthTargetTexture;

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
                mode = RenderingMode.Perspective,
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
        public static CameraData CreateOffCenterOrthographicCamera(float width, float height, float nearPlane, float farPlane)
        {
            return new CameraData
            {
                aspectRatio = width / height,
                nearPlane = nearPlane,
                farPlane = farPlane,
                orthoSize = height / 2,
                mode = RenderingMode.Orthographic,
            };
        }

        public void SetFramebuffer(Framebuffer framebuffer) => this.framebuffer = framebuffer;
        public void SetFinalFramebuffer(Framebuffer framebuffer) => this.finalFramebuffer = framebuffer;
        //public void SetColorTargetTexture(Texture2D tex) => colorTargetTexture = tex; 
        //public void SetDepthTargetTexture(DepthTexture tex) => depthTargetTexture = tex; 

        public void SetOrthoResolution(float width, float height)
        {
            aspectRatio = width / height;
            orthoSize = height / 2;
        }

        public float OrthoHeight => orthoSize * 2f;
        public float OrthoWidth => OrthoHeight * aspectRatio;

        public float RadiansFov => MathHelper.DegreesToRadians(fov);

        public Framebuffer Framebuffer => framebuffer;
        public Framebuffer FinalFramebuffer => finalFramebuffer;
        //public Texture2D ColorTargetTexture => colorTargetTexture;
        //public DepthTexture DepthTargetTexture => depthTargetTexture;

        public Matrix4 CalculateProjection(bool orthoCentered = false)
        {
            if (mode == RenderingMode.Perspective)
                return Matrix4.CreatePerspectiveFieldOfView(RadiansFov, aspectRatio, nearPlane, farPlane);
            else
            {
                if (!orthoCentered)
                    return Matrix4.CreateOrthographicOffCenter(0.0f, OrthoWidth, 0.0f, OrthoHeight, nearPlane, farPlane);
                else
                    return Matrix4.CreateOrthographic(OrthoWidth, OrthoHeight, nearPlane, farPlane);
            }
        }
    }
}

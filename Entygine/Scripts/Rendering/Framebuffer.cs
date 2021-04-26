using Entygine.DevTools;
using Entygine.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Entygine.Rendering
{
    public class Framebuffer
    {
        private string name;
        private int handle;
        public Vec2i Size { get; private set; }
        private bool multisampledColor;
        private int colorBuffer = -1;
        private bool multisampledDepth;
        private int depthBuffer = -1;

        public Framebuffer(Vec2i size, string name)
        {
            this.name = name;
            handle = Ogl.GenFramebuffer(name);
            Size = size;
        }

        public void AddColorBuffer(bool multisampling)
        {
            if (colorBuffer != -1)
            {
                DevConsole.Log(LogType.Error, "Framebuffer already has color buffer");
                return;
            }

            multisampledColor = multisampling;
            TextureTarget textureTarget = multisampling ? TextureTarget.Texture2DMultisample : TextureTarget.Texture2D;
            colorBuffer = Ogl.GenTexture($"{name} - Color");

            Ogl.BindTexture(textureTarget, colorBuffer);
            if (multisampling)
                Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Rgb, Size.x, Size.y, true);
            else
            {
                Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Size.x, Size.y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);

                Ogl.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, TextureMinFilter.Linear);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, TextureMagFilter.Linear);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureWrapS, TextureWrapMode.Repeat);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureWrapT, TextureWrapMode.Repeat);
            }


            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
            Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, textureTarget, colorBuffer, 0);

            var error = Ogl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (error != FramebufferErrorCode.FramebufferComplete)
                DevConsole.Log(LogType.Warning, error);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void AddDepthBuffer(bool multisampling)
        {
            if (depthBuffer != -1)
            {
                DevConsole.Log(LogType.Error, "Framebuffer already has depth buffer");
                return;
            }

            multisampledDepth = multisampling;
            TextureTarget textureTarget = multisampling ? TextureTarget.Texture2DMultisample : TextureTarget.Texture2D;
            depthBuffer = Ogl.GenTexture($"{name} - Depth");

            Ogl.BindTexture(textureTarget, depthBuffer);

            if (multisampling)
                Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.DepthComponent, Size.x, Size.y, true);
            else
            {
                Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, Size.x, Size.y, 0, PixelFormat.DepthComponent, PixelType.Float, null);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, TextureMinFilter.Nearest);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, TextureMagFilter.Nearest);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureWrapS, TextureWrapMode.ClampToBorder);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureWrapT, TextureWrapMode.ClampToBorder);
                Ogl.TexParameter(textureTarget, TextureParameterName.TextureBorderColor, new float[] { 1, 1, 1, 1 });
            }


            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
            Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, textureTarget, depthBuffer, 0);

            var error = Ogl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (error != FramebufferErrorCode.FramebufferComplete)
                DevConsole.Log(LogType.Warning, error);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void ChangeSize(Vec2i size)
        {
            Size = size;

            if (colorBuffer != -1)
            {
                TextureTarget textureTarget = multisampledColor ? TextureTarget.Texture2DMultisample : TextureTarget.Texture2D;
                Ogl.BindTexture(textureTarget, colorBuffer);
                if (multisampledColor)
                    Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Rgb, Size.x, Size.y, true);
                else
                    Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Size.x, Size.y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
            }

            if (depthBuffer != -1)
            {
                TextureTarget textureTarget = multisampledDepth ? TextureTarget.Texture2DMultisample : TextureTarget.Texture2D;
                Ogl.BindTexture(textureTarget, depthBuffer);
                if (multisampledDepth)
                    Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.DepthComponent, Size.x, Size.y, true);
                else
                    Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, Size.x, Size.y, 0, PixelFormat.DepthComponent, PixelType.Float, null);
            }
        }

        public void Bind()
        {
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
            Ogl.DrawBuffer(DrawBufferMode.ColorAttachment0);
        }

        public void Unbind()
        {
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public int Handle => handle;
        public int ColorBuffer => colorBuffer;
        public int DepthBuffer => depthBuffer;

        public static void Blit(Framebuffer read, Framebuffer write, ClearBufferMask mask)
        {
            Ogl.BindFramebuffer(FramebufferTarget.ReadFramebuffer, read.handle);
            Ogl.BindFramebuffer(FramebufferTarget.DrawFramebuffer, write.handle);
            Ogl.BlitFramebuffer(0, 0, read.Size.x, read.Size.y, 0, 0, write.Size.x, write.Size.y, mask, BlitFramebufferFilter.Nearest);
        }
    }
}

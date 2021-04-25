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
        private int colorBuffer = -1;
        private int depthBuffer = -1;

        public Framebuffer(Vec2i size, string name)
        {
            this.name = name;
            handle = Ogl.GenFramebuffer(name);
            Size = size;
        }

        public void AddColorBuffer()
        {
            if (colorBuffer != -1)
            {
                DevConsole.Log(LogType.Error, "Framebuffer already has color buffer");
                return;
            }

            colorBuffer = Ogl.GenTexture($"{name} - Color");
            Ogl.BindTexture(TextureTarget.Texture2DMultisample, colorBuffer);
            Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Rgb, Size.x, Size.y, true);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
            Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, colorBuffer, 0);

            var error = Ogl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (error != FramebufferErrorCode.FramebufferComplete)
                DevConsole.Log(LogType.Warning, error);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void AddDepthBuffer()
        {
            if (depthBuffer != -1)
            {
                DevConsole.Log(LogType.Error, "Framebuffer already has depth buffer");
                return;
            }

            depthBuffer = Ogl.GenTexture($"{name} - Depth");
            Ogl.BindTexture(TextureTarget.Texture2DMultisample, depthBuffer);
            Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.DepthComponent, Size.x, Size.y, true);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
            Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2DMultisample, depthBuffer, 0);

            var error = Ogl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (error != FramebufferErrorCode.FramebufferComplete)
                DevConsole.Log(LogType.Warning, error);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void ChangeSize(Vec2i size)
        {
            Size = size;

            if(depthBuffer != -1)
            {
                Ogl.BindTexture(TextureTarget.Texture2DMultisample, depthBuffer);
                Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.DepthComponent, Size.x, Size.y, true);
            }

            if(colorBuffer != -1)
            {
                Ogl.BindTexture(TextureTarget.Texture2DMultisample, colorBuffer);
                Ogl.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Rgb, Size.x, Size.y, true);
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

        public static void Blit(Framebuffer read, Framebuffer write)
        {
            Ogl.BindFramebuffer(FramebufferTarget.ReadFramebuffer, read.handle);
            Ogl.BindFramebuffer(FramebufferTarget.DrawFramebuffer, write.handle);
            Ogl.BlitFramebuffer(0, 0, read.Size.x, read.Size.y, 0, 0, write.Size.x, write.Size.y, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
        }
    }
}

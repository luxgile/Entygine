﻿using OpenToolkit.Graphics.OpenGL4;

namespace Entygine.Rendering
{
    public class DepthTexture : BaseTexture
    {
        private int handle;

        private int width;
        private int height;

        public DepthTexture(int width, int height)
        {
            handle = Ogl.GenTexture();

            Ogl.BindTexture(TextureTarget.Texture2D, handle);
            Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, null);

            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureMinFilter.Nearest);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureMagFilter.Nearest);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, TextureWrapMode.ClampToBorder);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, TextureWrapMode.ClampToBorder);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[] { 1, 1, 1, 1 });

            this.width = width;
            this.height = height;
        }

        protected override void CalculatePackedData() { }

        public override int Width => width;
        public override int Height => height;

        protected override TextureTarget TextureType => TextureTarget.Texture2D;

        public override int Handle => handle;

        protected override bool HasChanged { get => true; set => _ = value; }
    }
}
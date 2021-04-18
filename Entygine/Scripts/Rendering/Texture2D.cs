using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;

namespace Entygine.Rendering
{
    public class Texture2D : BaseTexture
    {
        public int handle;

        private int width;
        private int height;
        private Rgba32[] pixels;
        private byte[] packedData;

        private bool hasChanged;

        public Texture2D(string path, string name)
        {
            this.handle = Ogl.GenTexture(name);

            Ogl.BindTexture(TextureTarget.Texture2D, handle);
            Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureMinFilter.Linear);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureMagFilter.Linear);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, TextureWrapMode.Repeat);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, TextureWrapMode.Repeat);

            LoadFromPath(path);
        }

        public Texture2D(int width, int height, string name)
        {
            handle = Ogl.GenTexture(name);

            Ogl.BindTexture(TextureTarget.Texture2D, handle);
            Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureMinFilter.Linear);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureMagFilter.Linear);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, TextureWrapMode.Repeat);
            Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, TextureWrapMode.Repeat);

            this.width = width;
            this.height = height;

            pixels = new Rgba32[width * height];
            packedData = new byte[pixels.Length * 4];
        }

        private void LoadFromPath(string path)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            width = image.Width;
            height = image.Height;
            pixels = new Rgba32[image.Width * image.Height];

            if (image.TryGetSinglePixelSpan(out Span<Rgba32> span))
            {
                for (int i = 0; i < span.Length; i++)
                    pixels[i] = span[i];
            }

            hasChanged = true;
        }

        protected override void CalculatePackedData()
        {
            packedData = new byte[pixels.Length * 4];
            for (int i = 0, packedIndex = 0; i < pixels.Length; i++)
            {
                packedData[packedIndex++] = pixels[i].R;
                packedData[packedIndex++] = pixels[i].G;
                packedData[packedIndex++] = pixels[i].B;
                packedData[packedIndex++] = pixels[i].A;
            }

            Ogl.BindTexture(TextureTarget.Texture2D, handle);
            Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, packedData);
            Ogl.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void SetPixels(Rgba32[] pixels)
        {
            this.pixels = pixels;
            hasChanged = true;
        }

        public void SetRawData(IntPtr data)
        {
            Ogl.BindTexture(TextureTarget.Texture2D, handle);
            Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            Ogl.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public override int Width => width;

        public override int Height => height;

        protected override TextureTarget TextureType => TextureTarget.Texture2D;

        public override int Handle => handle;

        protected override bool HasChanged { get => hasChanged; set => hasChanged = value; }

        public static Texture2D CreateWhiteTexture(int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, "White Texture");
            Rgba32[] pixels = new Rgba32[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = new Rgba32(255, 255, 255, 255);
            texture.SetPixels(pixels);
            return texture;
        }

        public static Texture2D CreatePlainTexture(int width, int height, Rgba32 color)
        {
            Texture2D texture = new Texture2D(width, height, "Plain Texture");
            Rgba32[] pixels = new Rgba32[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;
            texture.SetPixels(pixels);
            return texture;
        }
    }
}

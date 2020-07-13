using OpenToolkit.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class Texture
    {
        public int handle;

        private int width;
        private int height;
        private Rgba32[] pixels;
        private byte[] packedData;

        private bool hasChanged;

        public Texture(string path)
        {
            this.handle = GL.GenTexture();
            LoadFromPath(path);
        }

        public Texture(int width, int height)
        {
            this.handle = GL.GenTexture();
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

        private void CalculatePackedData()
        {
            packedData = new byte[pixels.Length * 4];
            for (int i = 0, packedIndex = 0; i < pixels.Length; i++)
            {
                packedData[packedIndex++] = pixels[i].R;
                packedData[packedIndex++] = pixels[i].G;
                packedData[packedIndex++] = pixels[i].B;
                packedData[packedIndex++] = pixels[i].A;
            }
        }

        public void UseTexture()
        {
            if(hasChanged)
            {
                hasChanged = false;
                CalculatePackedData();
            }

            GL.BindTexture(TextureTarget.Texture2D, handle);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, packedData);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void SetPixels(Rgba32[] pixels)
        {
            this.pixels = pixels;
            hasChanged = true;
        }

        public bool IsValid => handle != 0;

        public static Texture CreateWhiteTexture(int width, int height)
        {
            Texture texture = new Texture(width, height);
            Rgba32[] pixels = new Rgba32[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = new Rgba32(1, 1, 1, 1);
            texture.SetPixels(pixels);
            return texture;
        }
    }
}

using OpenToolkit.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;

namespace Entygine.Rendering
{
    public class Cubemap : BaseTexture
    {
        private int handle;

        private int width;
        private int height;

        private Rgba32[][] facesPixels = new Rgba32[6][];

        private byte[][] packedData = new byte[6][];

        private bool hasChanged;

        public Cubemap(string[] paths)
        {
            if (paths.Length != 6)
                throw new ArgumentException("Cubemap needs 6 path for every texture");

            this.handle = Ogl.GenBuffer();

            LoadFromPath(paths);
        }

        private void LoadFromPath(string[] paths)
        {
            for (int i = 0; i < 6; i++)
            {
                Image<Rgba32> image = Image.Load<Rgba32>(paths[i]);
                image.Mutate(x => x.Flip(FlipMode.Vertical));

                width = image.Width;
                height = image.Height;

                facesPixels[i] = new Rgba32[image.Width * image.Height];

                if (image.TryGetSinglePixelSpan(out Span<Rgba32> span))
                {
                    for (int p = 0; p < span.Length; p++)
                        facesPixels[i][p] = span[p];
                }
            }

            hasChanged = true;
        }

        protected override void CalculatePackedData()
        {
            packedData = new byte[6][];

            Ogl.BindTexture(TextureTarget.TextureCubeMap, handle);

            for (int i = 0; i < facesPixels.Length; i++)
            {
                Rgba32[] pixels = facesPixels[i];
                packedData[i] = new byte[pixels.Length * 4];
                for (int p = 0, packedIndex = 0; p < pixels.Length; p++)
                {
                    packedData[i][packedIndex++] = pixels[p].R;
                    packedData[i][packedIndex++] = pixels[p].G;
                    packedData[i][packedIndex++] = pixels[p].B;
                    packedData[i][packedIndex++] = pixels[p].A;
                }

                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, packedData[i]);
            }

            Ogl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            Ogl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            Ogl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            Ogl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            Ogl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            //GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
        }

        public override int Width => width;

        public override int Height => height;

        protected override TextureTarget TextureType => TextureTarget.TextureCubeMap;

        protected override int Handle => handle;

        protected override bool HasChanged { get => hasChanged; set => hasChanged = value; }
    }
}

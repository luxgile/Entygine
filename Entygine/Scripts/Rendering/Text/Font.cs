using Entygine.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SharpFont;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class Font : IDisposable
    {
        private Library lib;
        private Face font;

        private Dictionary<char, FontCharacter> characters = new Dictionary<char, FontCharacter>();
        private bool hasChanged;

        public Font(string path)
        {
            lib = new Library();
            font = lib.NewFace(path, 0);
        }

        public void SetSize(uint size)
        {
            font.SetPixelSizes(0, size);
            hasChanged = true;
        }

        public FontCharacter GetCharacter(char c) => characters[c];

        private void UpdateTextures()
        {
            ClearCharacters();

            Ogl.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            //For now we load the first 128 ASCII chars
            for (uint i = 0; i < 128; i++)
            {
                font.LoadChar(i, LoadFlags.Render, LoadTarget.Normal);
                int width = font.Glyph.Bitmap.Width;
                int height = font.Glyph.Bitmap.Rows;
                int bearingX = font.Glyph.BitmapLeft;
                int bearingY = font.Glyph.BitmapTop;

                int textureID = Ogl.GenTexture();
                Ogl.BindTexture(TextureTarget.Texture2D, textureID);
                Ogl.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, width, height
                    , 0, PixelFormat.Red, PixelType.UnsignedByte, font.Glyph.Bitmap.BufferData);
                Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, TextureWrapMode.ClampToEdge);
                Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, TextureWrapMode.ClampToEdge);
                Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureMinFilter.Linear);
                Ogl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureMagFilter.Linear);

                FontCharacter character = new FontCharacter(textureID, new Vec2i(width, height), new Vec2i(bearingX, bearingY), font.Glyph.Advance.X.Value);
                characters.Add((char)i, character);
            }
        }

        private void ClearCharacters()
        {
            foreach (KeyValuePair<char, FontCharacter> character in characters)
            {
                Ogl.DeleteTexture(character.Value.TextureID);
            }

            characters.Clear();
        }

        public void Dispose()
        {
            ClearCharacters();
            lib.Dispose();
            font.Dispose();
        }
    }
}

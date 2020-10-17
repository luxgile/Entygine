using Entygine.Mathematics;
using Entygine.UI;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public readonly struct FontCharacter
    {
        private readonly int textureID;
        private readonly Vec2i size;
        private readonly Vec2i offset;
        private readonly int advance;

        public FontCharacter(int textureID, Vec2i size, Vec2i offset, int advance)
        {
            this.textureID = textureID;
            this.size = size;
            this.offset = offset;
            this.advance = advance;
        }

        public Rect GetRect()
        {
            return new Rect()
            {
                pos = new OpenTK.Mathematics.Vector2(Offset.x, Size.y - Offset.y),
                size = new OpenTK.Mathematics.Vector2(Size.x, Size.y)
            };
        }

        public int TextureID => textureID;
        public Vec2i Size => size;
        public Vec2i Offset => offset;
        public int Advance => advance;
    }
}

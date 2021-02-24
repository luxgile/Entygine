using OpenTK.Graphics.OpenGL4;

namespace Entygine.Rendering
{
    public abstract class BaseTexture
    {
        public abstract int Width { get; }
        public abstract int Height { get; }

        protected abstract TextureTarget TextureType { get; }
        public abstract int Handle { get; }
        protected abstract bool HasChanged { get; set; }

        public void UseTexture(TextureUnit unit)
        {
            if (HasChanged)
            {
                HasChanged = false;
                CalculatePackedData();
            }

            Ogl.ActiveTexture(unit);
            Ogl.BindTexture(TextureType, Handle);
        }

        public void FreeTexture()
        {
            Ogl.BindTexture(TextureType, 0);
        }

        protected abstract void CalculatePackedData();
        public bool IsValid => Handle != 0;
    }
}

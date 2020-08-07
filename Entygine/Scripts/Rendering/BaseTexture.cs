using OpenToolkit.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entygine.Rendering
{
    public abstract class BaseTexture
    {
        public abstract int Width { get; }
        public abstract int Height { get; }

        protected abstract TextureTarget TextureType { get; }
        protected abstract int Handle { get; }
        protected abstract bool HasChanged { get; set; }

        public void UseTexture(TextureUnit unit)
        {
            if (HasChanged)
            {
                HasChanged = false;
                CalculatePackedData();
            }

            GL.ActiveTexture(unit);
            GL.BindTexture(TextureType, Handle);
        }

        protected abstract void CalculatePackedData();
        public bool IsValid => Handle != 0;
    }
}

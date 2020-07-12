using Entygine.Rendering;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Entygine.Ecs.Components
{
    public struct SC_RenderMesh : ISharedComponent, IDisposable
    {
        public Mesh mesh;
        public Material material;

        private bool isDisposed;

        public SC_RenderMesh(Mesh mesh, Material material)
        {
            this.mesh = mesh ?? throw new ArgumentNullException(nameof(mesh));
            this.material = material ?? throw new ArgumentNullException(nameof(material));
            isDisposed = false;

            material.LoadMaterial();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                GL.DeleteProgram(material.shader.handle);
                isDisposed = true;
            }
        }
    }
}

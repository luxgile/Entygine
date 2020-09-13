using Entygine.Rendering;
using System;

namespace Entygine.Ecs.Components
{
    public struct SC_RenderMesh : ISharedComponent
    {
        public int id;
        public RenderMesh value;
        public SC_RenderMesh(RenderMesh renderMesh)
        {
            id = -1;
            value = renderMesh;

            value.mat.LoadMaterial();
        }

        public SC_RenderMesh(Mesh mesh, Material mat) : this(new RenderMesh() { mesh = mesh, mat = mat }) { }
    }
}

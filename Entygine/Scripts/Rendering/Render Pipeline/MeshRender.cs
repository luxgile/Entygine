using System;
using System.Collections.Generic;
using System.Text;

namespace Entygine.Rendering
{
    public class MeshRender
    {
        public Material mat;
        public Mesh mesh;

        public override bool Equals(object obj)
        {
            return obj is MeshRender pair &&
                   EqualityComparer<Material>.Default.Equals(mat, pair.mat) &&
                   EqualityComparer<Mesh>.Default.Equals(mesh, pair.mesh);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(mat, mesh);
        }
    }
}

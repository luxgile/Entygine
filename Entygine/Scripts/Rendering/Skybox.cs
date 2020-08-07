using OpenToolkit.Graphics.OpenGL4;
using System.IO.Compression;

namespace Entygine.Rendering
{
    public class Skybox
    {
        private Material material;
        private Mesh cube;

        public Skybox(Material material)
        {
            this.material = material;

            cube = MeshPrimitives.CreateCube(1);
        }

        public Mesh Mesh => cube;
        public Material Material => material;
    }
}

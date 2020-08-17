namespace Entygine.Rendering
{
    public class Skybox
    {
        private Material material;
        private Mesh cube;

        public Skybox(Material material)
        {
            this.material = material;
            this.material.LoadMaterial();

            cube = MeshPrimitives.CreateCube(1);
            cube.SetVertexLayout(new VertexBufferLayout[]
            {
                new VertexBufferLayout(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            });
        }

        public Mesh Mesh => cube;
        public Material Material => material;
    }
}

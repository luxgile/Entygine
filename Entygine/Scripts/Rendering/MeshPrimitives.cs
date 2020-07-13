using OpenToolkit.Mathematics;

namespace Entygine.Rendering
{
    public class MeshPrimitives
    {
        public static Mesh CreateCube(float size)
        {
            float halfSize = size / 2f;
            Vector3 v0 = new Vector3(halfSize, halfSize, -halfSize);
            Vector3 v1 = new Vector3(-halfSize, halfSize, -halfSize);
            Vector3 v2 = new Vector3(-halfSize, -halfSize, -halfSize);
            Vector3 v3 = new Vector3(halfSize, -halfSize, -halfSize);
            Vector3 v4 = new Vector3(halfSize, -halfSize, halfSize);
            Vector3 v5 = new Vector3(halfSize, halfSize, halfSize);
            Vector3 v6 = new Vector3(-halfSize, halfSize, halfSize);
            Vector3 v7 = new Vector3(-halfSize, -halfSize, halfSize);

            Vector3[] verts = new Vector3[]
            {
                //Back Face
                v0, v1, v2, v0, v2, v3,
                //Front face
                v5, v6, v7, v5, v7, v4,
                //Top face
                v5, v1, v0, v5, v6, v1,
                //Bottom face
                v2, v4, v3, v2, v7, v4,
                //Right face
                v5, v3, v4, v5, v0, v4,
                //Left face
                v6, v7, v2, v6, v2, v1
            };

            uint[] triangles = new uint[36];
            for (int i = 0; i < triangles.Length; i++)
                triangles[i] = (uint)i;

            Vector2 uv00 = new Vector2(0, 0);
            Vector2 uv10 = new Vector2(1, 0);
            Vector2 uv11 = new Vector2(1, 1);
            Vector2 uv01 = new Vector2(0, 1);
            Vector2[] uvs = new Vector2[]
            {
                //Back Face
                uv11, uv01, uv00, uv11, uv00, uv10,
                //Front face
                uv01, uv11, uv10, uv01, uv10, uv00,
                //Top face
                uv11, uv00, uv10, uv11, uv01, uv10,
                //Bottom face
                uv00, uv11, uv10, uv00, uv01, uv11,
                //Right face
                uv11, uv00, uv10, uv11, uv01, uv10,
                //Left face
                uv01, uv00, uv10, uv01, uv10, uv11
            };

            Mesh mesh = new Mesh(verts, new Vector3[0], uvs, triangles);
            return mesh;
        }

        public static Mesh CreatePlane(float size)
        {
            float halfSize = size / 2;
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-halfSize, 0, -halfSize),
                new Vector3(-halfSize, 0, halfSize),
                new Vector3(halfSize, 0, halfSize),
                new Vector3(halfSize, 0, -halfSize),
            };
            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            };
            uint[] tris = new uint[]
            {
                0, 1, 2, 0, 2, 3
            };

            return new Mesh(vertices, new Vector3[0], uvs, tris);
        }
    }
}

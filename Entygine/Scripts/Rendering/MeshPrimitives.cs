using OpenToolkit.Mathematics;
using System.Net;

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
                v0, v2, v1, v0, v3, v2,
                //Front face
                v5, v6, v7, v5, v7, v4,
                //Top face
                v5, v0, v1, v5, v1, v6,
                //Bottom face
                v2, v3, v4, v2, v4, v7,
                //Right face
                v5, v3, v0, v5, v4, v3,
                //Left face
                v6, v2, v7, v6, v1, v2
            };

            uint[] triangles = new uint[36];
            for (int i = 0; i < triangles.Length; i++)
                triangles[i] = (uint)i;

            Vector2 uvbl = new Vector2(0, 0);
            Vector2 uvbr = new Vector2(1, 0);
            Vector2 uvtr = new Vector2(1, 1);
            Vector2 uvtl = new Vector2(0, 1);
            Vector2[] uvs = new Vector2[]
            {
                //Back Face
                uvtr, uvbl, uvtl, uvtr, uvbr, uvbl,
                //Front face
                uvtl, uvtr, uvbr, uvtl, uvbr, uvbl,
                //Top face
                uvtr, uvbr, uvbl, uvtr, uvbl, uvtl,
                //Bottom face
                uvtr, uvtl, uvbl, uvtr, uvbl, uvbr,
                //Right face
                uvtr, uvbl, uvtl, uvtr, uvbr, uvbl,
                //Left face
                uvtl, uvbr, uvbl, uvtl, uvtr, uvbr
            };

            Vector3 normalUp = new Vector3(0, 1, 0);
            Vector3 normalRight = new Vector3(1, 0, 0);
            Vector3 normalForward = new Vector3(0, 0, 1);
            Vector3[] normals = new Vector3[]
            {
                -normalForward, -normalForward, -normalForward,-normalForward, -normalForward, -normalForward,
                normalForward, normalForward, normalForward,normalForward, normalForward, normalForward,
                normalUp, normalUp, normalUp,normalUp, normalUp, normalUp,
                -normalUp, -normalUp, -normalUp,-normalUp, -normalUp, -normalUp,
                normalRight, normalRight, normalRight,normalRight, normalRight, normalRight,
                -normalRight, -normalRight, -normalRight,-normalRight, -normalRight, -normalRight,
            };

            Mesh mesh = new Mesh(verts, normals, uvs, triangles);
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
            Vector3[] normals = new Vector3[]
            {
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
            };

            return new Mesh(vertices, normals, uvs, tris);
        }
    }
}

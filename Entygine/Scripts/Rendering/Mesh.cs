using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Entygine.Rendering
{
    public class Mesh
    {
        private int vertexArrayHandle;

        private int vertexBuffer;
        private int trisBuffer;

        private VertexBufferLayout[] layouts;

        private Vector3[] verts;
        private Vector3[] normals;
        private Vector2[] uvs;
        private uint[] tris;

        private float[] packedData;

        private bool meshChanged;

        public Mesh(Vector3[] verts, Vector3[] normals, Vector2[] uvs, uint[] tris)
        {
            this.verts = verts ?? throw new ArgumentNullException(nameof(verts));
            this.normals = normals ?? throw new ArgumentNullException(nameof(normals));
            this.uvs = uvs ?? throw new ArgumentNullException(nameof(uvs));
            this.tris = tris ?? throw new ArgumentNullException(nameof(tris));

            vertexArrayHandle = GL.GenVertexArray();
            vertexBuffer = GL.GenBuffer();
            trisBuffer = GL.GenBuffer();

            layouts = new[]
            {
                new VertexBufferLayout(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexBufferLayout(VertexAttribute.Uv0, VertexAttributeFormat.Float32, 2),
                new VertexBufferLayout(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
            };

            meshChanged = true;
        }

        public void UpdateMeshData()
        {
            if (meshChanged)
            {
                meshChanged = false;
                ForceUpdateMeshData();
            }
        }

        public void ForceUpdateMeshData()
        {
            CalculatePackedData();

            GL.BindVertexArray(vertexArrayHandle);

            //TODO: Dunno if i have to apply this always?
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, packedData.Length * sizeof(float), packedData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, trisBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, tris.Length * sizeof(uint), tris, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }

        private void CalculatePackedData()
        {
            //TODO: Have in mind the buffer layout to pack the data
            packedData = new float[verts.Length * 8];
            for (int i = 0, v = -1; i < verts.Length; i++)
            {
                for (int l = 0; l < layouts.Length; l++)
                {
                    VertexBufferLayout layout = layouts[l];
                    switch (layout.Attribute)
                    {
                        case VertexAttribute.Position:
                            packedData[++v] = verts[i].X;
                            packedData[++v] = verts[i].Y;
                            packedData[++v] = verts[i].Z;
                            break;

                        case VertexAttribute.Normal:
                            packedData[++v] = normals[i].X;
                            packedData[++v] = normals[i].Y;
                            packedData[++v] = normals[i].Z;
                            break;

                        case VertexAttribute.Tangent:
                            break;

                        case VertexAttribute.Color:
                            break;

                        case VertexAttribute.Uv0:
                            packedData[++v] = uvs[i].X;
                            packedData[++v] = uvs[i].Y;
                            break;

                        case VertexAttribute.Uv1:
                            break;

                        case VertexAttribute.Uv2:
                            break;

                        case VertexAttribute.Uv3:
                            break;

                        case VertexAttribute.Uv4:
                            break;

                        case VertexAttribute.Uv5:
                            break;

                        case VertexAttribute.Uv6:
                            break;

                        case VertexAttribute.Uv7:
                            break;
                    }
                }
            }
        }

        public void SetVertices(Vector3[] vertices)
        {
            verts = vertices;
            meshChanged = true;
        }
        public void GetVertices(ref List<Vector3> vertices)
        {
            vertices.Clear();
            for (int i = 0; i < this.verts.Length; i++)
            {
                vertices.Add(this.verts[i]);
            }
        }

        public void SetIndices(uint[] tris)
        {
            this.tris = tris;
            meshChanged = true;
        }
        public void GetIndices(ref List<uint> triangles)
        {
            triangles.Clear();
            for (int i = 0; i < tris.Length; i++)
            {
                triangles.Add(tris[i]);
            }
        }
        public int GetIndiceCount() => tris.Length;

        public void SetUVs(Vector2[] uvs)
        {
            this.uvs = uvs;
            meshChanged = true;
        }
        public void GetUVs(ref List<Vector2> uvs)
        {
            uvs.Clear();
            for (int i = 0; i < this.uvs.Length; i++)
            {
                uvs.Add(this.uvs[i]);
            }
        }

        public int GetVertexArrayHandle()
        {
            return vertexArrayHandle;
        }

        public int GetVertexBuffer()
        {
            return vertexBuffer;
        }
        public int GetTrisBuffer()
        {
            return trisBuffer;
        }
    }
}

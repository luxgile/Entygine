﻿using OpenToolkit.Graphics.OpenGL4;

namespace Entygine.Rendering
{
    public static class GraphicsAPI
    {
        public static void DrawTriangles(int trisCount)
        {
            Ogl.DrawElements(PrimitiveType.Triangles, trisCount, DrawElementsType.UnsignedInt, 0);
        }

        public static void UseMeshMaterial(Mesh mesh, Material mat)
        {
            Ogl.BindVertexArray(mesh.GetVertexArrayHandle());

            mat.UseMaterial();

            mesh.UpdateMeshData(mat);
        }

        public static void FreeMeshMaterial(Mesh mesh, Material mat)
        {
            mat.FreeMaterial();

            Ogl.BindVertexArray(0);
        }
    }
}

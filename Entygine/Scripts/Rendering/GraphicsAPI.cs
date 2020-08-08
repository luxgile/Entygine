using Entygine.DevTools;
using OpenToolkit.Graphics.OpenGL4;

namespace Entygine.Rendering
{
    public static class GraphicsAPI
    {
        public static void DrawTriangles(int trisCount)
        {
            LogErrors();

            GL.DrawElements(PrimitiveType.Triangles, trisCount, DrawElementsType.UnsignedInt, 0);

            LogErrors();
        }

        public static void UseMeshMaterial(Mesh mesh, Material mat)
        {
            LogErrors();

            GL.BindVertexArray(mesh.GetVertexArrayHandle());
            LogErrors();

            mesh.UpdateMeshData(mat);
            LogErrors();

            mat.UseMaterial();
            LogErrors();
        }

        public static void FreeMeshMaterial(Mesh mesh, Material mat)
        {
            mat.FreeMaterial();

            GL.BindVertexArray(0);
        }

        private static void LogErrors()
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                DevConsole.Log(error);
        }
    }
}

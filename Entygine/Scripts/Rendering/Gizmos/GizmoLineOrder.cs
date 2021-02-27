using Entygine.Mathematics;
using Entygine.Rendering;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Entygine.DevTools
{
    public struct GizmoLine
    {
        public GizmoLine(Vec3f pointA, Vec3f pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }

        public Vec3f PointA { get; set; }
        public Vec3f PointB { get; set; }
    }

    public class GizmoLineOrder : GizmoOrder<GizmoLine>
    {
        private float[] data = new float[0];
        private int vbo = 0;
        private int vao = 0;

        public GizmoLineOrder()
        {
            vao = Ogl.GenVertexArray();
            vbo = Ogl.GenBuffer();
        }

        protected override void Draw(List<GizmoLine> gizmoData)
        {
            if (gizmoData.Count == 0)
                return;

            if (data.Length < gizmoData.Count * 6)
                Array.Resize(ref data, gizmoData.Count * 6);

            int dataIndex = 0;
            for (int i = 0; i < gizmoData.Count; i++)
            {
                GizmoLine gizmo = gizmoData[i];
                data[dataIndex++] = gizmo.PointA.x;
                data[dataIndex++] = gizmo.PointA.y;
                data[dataIndex++] = gizmo.PointA.z;

                data[dataIndex++] = gizmo.PointB.x;
                data[dataIndex++] = gizmo.PointB.y;
                data[dataIndex++] = gizmo.PointB.z;
            }

            //VBO config
            Ogl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            Ogl.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * dataIndex, data, BufferUsageHint.StreamDraw);

            //VAO config
            Ogl.BindVertexArray(vao);
            Ogl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            Ogl.EnableVertexAttribArray(0);

            Ogl.DrawArray(PrimitiveType.Lines, 0, gizmoData.Count * 2);
        }
    }
}

using Entygine.Mathematics;
using Entygine.Rendering;
using OpenToolkit.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Entygine.DevTools
{
    public struct GizmoPoint
    {
        public GizmoPoint(Vec3f point)
        {
            Point = point;
        }

        public Vec3f Point { get; set; }
    }

    public class GizmoPointOrder : GizmoOrder<GizmoPoint>
    {
        private float[] data = new float[0];
        private int vbo = 0;
        private int vao = 0;

        public GizmoPointOrder()
        {
            vao = Ogl.GenVertexArray();
            vbo = Ogl.GenBuffer();
        }

        protected override void Draw(List<GizmoPoint> gizmoData)
        {
            if (gizmoData.Count == 0)
                return;

            if (data.Length < gizmoData.Count * 3)
                Array.Resize(ref data, gizmoData.Count * 3);

            int dataIndex = 0;
            for (int i = 0; i < gizmoData.Count; i++)
            {
                GizmoPoint gizmo = gizmoData[i];
                data[dataIndex++] = gizmo.Point.x;
                data[dataIndex++] = gizmo.Point.y;
                data[dataIndex++] = gizmo.Point.z;
            }

            //VAO config
            Ogl.BindVertexArray(vao);
            Ogl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            Ogl.EnableVertexAttribArray(0);

            //VBO config
            Ogl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            Ogl.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * dataIndex, data, BufferUsageHint.StreamDraw);

            //THE ONLY DATA PASSED HERE IS (-5, -5, 0) AND I DON'T KNOW FUCKING WHY?
            Ogl.DrawArray(PrimitiveType.Points, 0, gizmoData.Count);
        }
    }
}

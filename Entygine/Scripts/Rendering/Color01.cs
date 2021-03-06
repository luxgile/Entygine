﻿using OpenTK.Mathematics;
using System.Numerics;

namespace Entygine.Rendering
{
    public struct Color01
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color01(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static explicit operator Color4(Color01 color)
        {
            return new Color4(color.r, color.g, color.b, color.a);
        }

        public static explicit operator System.Numerics.Vector4(Color01 v) => new System.Numerics.Vector4(v.r, v.g, v.b, v.a);

        public static readonly Color01 white = new Color01(1, 1, 1, 1);
        public static readonly Color01 black = new Color01(0, 0, 0, 1);
        public static readonly Color01 gray = new Color01(0.5f, 0.5f, 0.5f, 1);
        public static readonly Color01 red = new Color01(1, 0, 0, 1);
        public static readonly Color01 yellow = new Color01(1, 1, 0, 1);
        public static readonly Color01 green = new Color01(0, 1, 0, 1);
        public static readonly Color01 blue = new Color01(0, 0, 1, 1);
    }
}

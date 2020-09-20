using System;
using System.Diagnostics.CodeAnalysis;

namespace Entygine.Mathematics
{
    public struct Mat3f : IEquatable<Mat3f>
    {
        public float v00, v10, v20;
        public float v01, v11, v21;
        public float v02, v12, v22;

        public static readonly Mat3f Zero = new Mat3f(0, 0, 0, 0, 0, 0, 0, 0, 0);
        public static readonly Mat3f Identity = new Mat3f(1, 0, 0, 0, 1, 0, 0, 0, 1);

        public Mat3f(Vec3f row0, Vec3f row1, Vec3f row2)
        {
            v00 = row0.x;
            v10 = row0.y;
            v20 = row0.z;

            v01 = row1.x;
            v11 = row1.y;
            v21 = row1.z;

            v02 = row2.x;
            v12 = row2.y;
            v22 = row2.z;
        }

        public Mat3f(float v00, float v10, float v20, float v01, float v11, float v21, float v02, float v12, float v22)
        {
            this.v00 = v00;
            this.v10 = v10;
            this.v20 = v20;
            this.v01 = v01;
            this.v11 = v11;
            this.v21 = v21;
            this.v02 = v02;
            this.v12 = v12;
            this.v22 = v22;
        }

        public Mat3f(Mat4f matrix)
        {
            v00 = matrix.v00;
            v10 = matrix.v10;
            v20 = matrix.v20;
            v01 = matrix.v01;
            v11 = matrix.v11;
            v21 = matrix.v21;
            v02 = matrix.v02;
            v12 = matrix.v12;
            v22 = matrix.v22;
        }

        public float GetDeterminant()
        {
            return (v00 * v11 * v22) + (v10 * v21 * v02) + (v20 * v01 * v12)
                   - (v20 * v11 * v02) - (v00 * v21 * v12) - (v10 * v01 * v22);
        }


        public Quatf GetRotation(bool rowNormalize = true)
        {
            var row0 = Row0;
            var row1 = Row1;
            var row2 = Row2;

            if (rowNormalize)
            {
                row0 = row0.Normalized();
                row1 = row1.Normalized();
                row2 = row2.Normalized();
            }

            // code below adapted from Blender
            var q = default(Quatf);
            var trace = 0.25 * (row0[0] + row1[1] + row2[2] + 1.0);

            if (trace > 0)
            {
                var sq = Math.Sqrt(trace);

                q.w = (float)sq;
                sq = 1.0 / (4.0 * sq);
                q.x = (float)((row1[2] - row2[1]) * sq);
                q.y = (float)((row2[0] - row0[2]) * sq);
                q.z = (float)((row0[1] - row1[0]) * sq);
            }
            else if (row0[0] > row1[1] && row0[0] > row2[2])
            {
                var sq = 2.0 * Math.Sqrt(1.0 + row0[0] - row1[1] - row2[2]);

                q.x = (float)(0.25 * sq);
                sq = 1.0 / sq;
                q.w = (float)((row2[1] - row1[2]) * sq);
                q.y = (float)((row1[0] + row0[1]) * sq);
                q.z = (float)((row2[0] + row0[2]) * sq);
            }
            else if (row1[1] > row2[2])
            {
                var sq = 2.0 * Math.Sqrt(1.0 + row1[1] - row0[0] - row2[2]);

                q.y = (float)(0.25 * sq);
                sq = 1.0 / sq;
                q.w = (float)((row2[0] - row0[2]) * sq);
                q.x = (float)((row1[0] + row0[1]) * sq);
                q.z = (float)((row2[1] + row1[2]) * sq);
            }
            else
            {
                var sq = 2.0 * Math.Sqrt(1.0 + row2[2] - row0[0] - row1[1]);

                q.z = (float)(0.25 * sq);
                sq = 1.0 / sq;
                q.w = (float)((row1[0] - row0[1]) * sq);
                q.x = (float)((row2[0] + row0[2]) * sq);
                q.y = (float)((row2[1] + row1[2]) * sq);
            }

            q.Normalize();
            return q;
        }

        public Vec3f Diagonal
        {
            get => new Vec3f(v00, v11, v22);
            set
            {
                v00 = value.x;
                v11 = value.y;
                v22 = value.z;
            }
        }

        public float Trace => v00 + v11 + v22;

        public void Normalize()
        {
            float det = GetDeterminant();
            v00 /= det;
            v01 /= det;
            v02 /= det;

            v10 /= det;
            v11 /= det;
            v12 /= det;

            v20 /= det;
            v21 /= det;
            v22 /= det;
        }

        public static Mat3f operator -(Mat3f v) => Substract(v);
        public static Mat3f Substract(in Mat3f left)
        {
            return new Mat3f
                (
                    -left.v00, -left.v10, -left.v20,
                    -left.v01, -left.v11, -left.v21,
                    -left.v02, -left.v12, -left.v22
                );
        }

        public static Mat3f operator -(Mat3f l, Mat3f r) => Substract(l, r);
        public static Mat3f Substract(in Mat3f left, in Mat3f right)
        {
            return new Mat3f
                (
                    left.v00 - right.v00, left.v10 - right.v10, left.v20 - right.v20,
                    left.v01 - right.v01, left.v11 - right.v11, left.v21 - right.v21,
                    left.v02 - right.v02, left.v12 - right.v12, left.v22 - right.v22
                );
        }

        public static Mat3f operator +(Mat3f l, Mat3f r) => Add(l, r);
        public static Mat3f Add(in Mat3f left, in Mat3f right)
        {
            return new Mat3f
                (
                    left.v00 + right.v00, left.v10 + right.v10, left.v20 + right.v20,
                    left.v01 + right.v01, left.v11 + right.v11, left.v21 + right.v21,
                    left.v02 + right.v02, left.v12 + right.v12, left.v22 + right.v22
                );
        }

        public static Mat3f operator *(Mat3f m, float v) => Multiply(m, v);
        public static Mat3f operator *(float v, Mat3f m) => Multiply(m, v);
        public static Mat3f Multiply(in Mat3f left, in float right)
        {
            return new Mat3f
                (
                    left.v00 * right, left.v10 * right, left.v20 * right,
                    left.v01 * right, left.v11 * right, left.v21 * right,
                    left.v02 * right, left.v12 * right, left.v22 * right
                );
        }


        public static Mat3f operator *(Mat3f l, Mat3f r) => Multiply(l, r);
        public static Mat3f Multiply(in Mat3f left, in Mat3f right)
        {
            float leftM11 = left.v00;
            float leftM12 = left.v01;
            float leftM13 = left.v02;
            float leftM21 = left.v10;
            float leftM22 = left.v11;
            float leftM23 = left.v12;
            float leftM31 = left.v20;
            float leftM32 = left.v21;
            float leftM33 = left.v22;
            float rightM11 = right.v00;
            float rightM12 = right.v01;
            float rightM13 = right.v02;
            float rightM21 = right.v10;
            float rightM22 = right.v11;
            float rightM23 = right.v12;
            float rightM31 = right.v20;
            float rightM32 = right.v21;
            float rightM33 = right.v22;

            return new Mat3f
                (
                    (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31),
                    (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32),
                    (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33),
                    (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31),
                    (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32),
                    (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33),
                    (leftM31 * rightM11) + (leftM32 * rightM21) + (leftM33 * rightM31),
                    (leftM31 * rightM12) + (leftM32 * rightM22) + (leftM33 * rightM32),
                    (leftM31 * rightM13) + (leftM32 * rightM23) + (leftM33 * rightM33)
                );
        }

        public Mat3f Inverted()
        {
            Mat3f matCopy = this;
            float det = matCopy.GetDeterminant();
            if (det != 0)
                matCopy.Invert();

            return matCopy;
        }

        public void Invert()
        {
            this = Invert(this);
        }

        public static Mat3f Invert(in Mat3f mat)
        {
            int[] colIdx = { 0, 0, 0 };
            int[] rowIdx = { 0, 0, 0 };
            int[] pivotIdx = { -1, -1, -1 };

            float[,] inverse =
            {
                { mat.v00, mat.v10, mat.v20 },
                { mat.v01, mat.v11, mat.v21 },
                { mat.v02, mat.v12, mat.v22 }
            };

            var icol = 0;
            var irow = 0;
            for (var i = 0; i < 3; i++)
            {
                var maxPivot = 0.0f;
                for (var j = 0; j < 3; j++)
                {
                    if (pivotIdx[j] != 0)
                    {
                        for (var k = 0; k < 3; ++k)
                        {
                            if (pivotIdx[k] == -1)
                            {
                                var absVal = MathUtils.Absolute(inverse[j, k]);
                                if (absVal > maxPivot)
                                {
                                    maxPivot = absVal;
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (pivotIdx[k] > 0)
                                return mat;
                        }
                    }
                }

                ++pivotIdx[icol];

                if (irow != icol)
                {
                    for (var k = 0; k < 3; ++k)
                    {
                        var f = inverse[irow, k];
                        inverse[irow, k] = inverse[icol, k];
                        inverse[icol, k] = f;
                    }
                }

                rowIdx[i] = irow;
                colIdx[i] = icol;

                var pivot = inverse[icol, icol];

                if (pivot == 0.0f)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                }

                var oneOverPivot = 1.0f / pivot;
                inverse[icol, icol] = 1.0f;
                for (var k = 0; k < 3; ++k)
                {
                    inverse[icol, k] *= oneOverPivot;
                }

                for (var j = 0; j < 3; ++j)
                {
                    if (icol != j)
                    {
                        var f = inverse[j, icol];
                        inverse[j, icol] = 0.0f;
                        for (var k = 0; k < 3; ++k)
                        {
                            inverse[j, k] -= inverse[icol, k] * f;
                        }
                    }
                }
            }

            for (var j = 2; j >= 0; --j)
            {
                var ir = rowIdx[j];
                var ic = colIdx[j];
                for (var k = 0; k < 3; ++k)
                {
                    var f = inverse[k, ir];
                    inverse[k, ir] = inverse[k, ic];
                    inverse[k, ic] = f;
                }
            }

            return new Mat3f(inverse[0, 0], inverse[0, 1], inverse[0, 2], inverse[1, 0], inverse[1, 1], inverse[1, 2], inverse[2, 0], inverse[2, 1], inverse[2, 2]);
        }

        public static Mat3f CreateRotation(in Quatf quaternion)
        {
            quaternion.GetAngleAxis(out Vec3f axis, out float angle);
            return CreateRotation(axis, angle);
        }
        public static Mat3f CreateRotation(in Vec3f axis, in float angle)
        {
            axis.Normalize();
            float axisX = axis.x, axisY = axis.y, axisZ = axis.z;

            var cos = (float)Math.Cos(-angle);
            var sin = (float)Math.Sin(-angle);
            var t = 1.0f - cos;

            float tXX = t * axisX * axisX;
            float tXY = t * axisX * axisY;
            float tXZ = t * axisX * axisZ;
            float tYY = t * axisY * axisY;
            float tYZ = t * axisY * axisZ;
            float tZZ = t * axisZ * axisZ;

            float sinX = sin * axisX;
            float sinY = sin * axisY;
            float sinZ = sin * axisZ;

            return new Mat3f(tXX + cos, tXY - sinZ, tXZ + sinY,
                             tXY + sinZ, tYY + cos, tYZ - sinX,
                             tXZ - sinY, tYZ + sinX, tZZ + cos);
        }

        public static Mat3f OuterProduct(in Vec3f left, in Vec3f right)
        {
            return new Mat3f(
                                left.x * right.x, left.x * right.y, left.x * right.z,
                                left.y * right.x, left.y * right.y, left.y * right.z,
                                left.z * right.x, left.z * right.y, left.z * right.z
                            );
        }

        public override string ToString()
        {
            return $"{v00}, {v01}, {v02}\n{v10}, {v11}, {v12}\n{v20}, {v21}, {v22}";
        }

        public bool Equals([AllowNull] Mat3f other)
        {
            return
                v00 == other.v00 &&
                v01 == other.v01 &&
                v02 == other.v02 &&

                v10 == other.v10 &&
                v11 == other.v11 &&
                v12 == other.v12 &&

                v20 == other.v20 &&
                v21 == other.v21 &&
                v22 == other.v22;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(v00);
            hash.Add(v10);
            hash.Add(v20);
            hash.Add(v01);
            hash.Add(v11);
            hash.Add(v21);
            hash.Add(v02);
            hash.Add(v12);
            hash.Add(v22);
            return hash.ToHashCode();
        }

        public Vec3f Row0 => new Vec3f(v00, v01, v02);
        public Vec3f Row1 => new Vec3f(v10, v11, v12);
        public Vec3f Row2 => new Vec3f(v20, v21, v22);

        public Vec3f Column0 => new Vec3f(v00, v10, v20);
        public Vec3f Column1 => new Vec3f(v01, v11, v21);
        public Vec3f Column2 => new Vec3f(v02, v12, v22);
    }
}

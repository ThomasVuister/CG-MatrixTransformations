using System;
using System.Text;

namespace MatrixTransformations
{
    public class Matrix
    {
        float[,] mat = new float[4, 4];

        public Matrix()
        {
            mat[0, 0] = 1; mat[0, 1] = 0; mat[0, 2] = 0; mat[0, 3] = 0;
            mat[1, 0] = 0; mat[1, 1] = 1; mat[1, 2] = 0; mat[1, 3] = 0;
            mat[2, 0] = 0; mat[2, 1] = 0; mat[2, 2] = 1; mat[2, 3] = 0;
            mat[3, 0] = 0; mat[3, 1] = 0; mat[3, 2] = 0; mat[3, 3] = 1;
        }

        public Matrix(float m11, float m12, float m13,
                      float m21, float m22, float m23,
                      float m31, float m32, float m33)
        {
            mat[0, 0] = m11; mat[0, 1] = m12; mat[0, 2] = m13; mat[0, 3] = 0;
            mat[1, 0] = m21; mat[1, 1] = m22; mat[1, 2] = m23; mat[1, 3] = 0;
            mat[2, 0] = m31; mat[2, 1] = m32; mat[2, 2] = m33; mat[2, 3] = 0;
            mat[3, 0] = 0;   mat[3, 1] = 0;   mat[3, 2] = 0;   mat[3, 3] = 1;
        }

        public Matrix(Vector v)
        {
            mat[0, 0] = v.x; mat[0, 1] = 0; mat[0, 2] = 0; mat[0, 3] = 0;
            mat[1, 0] = v.y; mat[1, 1] = 0; mat[1, 2] = 0; mat[1, 3] = 0;
            mat[2, 0] = v.z; mat[2, 1] = 0; mat[2, 2] = 0; mat[2, 3] = 0;
            mat[3, 0] = 1;   mat[3, 1] = 0; mat[3, 2] = 0; mat[3, 3] = 0;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix matrix = new Matrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix.mat[i, j] = m1.mat[i, j] + m2.mat[i, j];
                }
            }
            return matrix;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix matrix = new Matrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix.mat[i, j] = m1.mat[i, j] - m2.mat[i, j];
                }
            }
            return matrix;
        }

        public static Matrix operator *(Matrix m1, float f)
        {
            Matrix matrix = new Matrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix.mat[i, j] = m1.mat[i, j] * f;
                }
            }
            return matrix;
        }

        public static Matrix operator *(float f, Matrix m1)
        {
            Matrix matrix = new Matrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix.mat[i, j] =  f * m1.mat[i, j];
                }
            }
            return matrix;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix matrix = new Matrix();
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    float sum = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        sum += m1.mat[r, i] * m2.mat[i, c];
                    }
                    matrix.mat[r, c] = sum;
                }
            }
            return matrix;
        }

        public static Vector operator *(Matrix m1, Vector v)
        {
            Matrix matrix = m1 * new Matrix(v);
            return new Vector(matrix.mat[0, 0], matrix.mat[1, 0], matrix.mat[2, 0]);
        }

        public static Matrix Identity()
        {
            return new Matrix();
        }

        public static Matrix ScaleMatrix(float s)
        {
            Matrix matrix = new Matrix();
            return s * matrix;
        }

        public static Matrix RotateMatrix(float degrees)
        {
            decimal dec = new decimal(degrees);
            double doubleDegrees = Convert.ToDouble(dec);

            Matrix matrix = new Matrix();
            matrix.mat[0, 0] = (float)Math.Cos(doubleDegrees); matrix.mat[0, 1] = (float)-Math.Sin(doubleDegrees);
            matrix.mat[1, 0] = (float)Math.Sin(doubleDegrees); matrix.mat[1, 1] = (float)Math.Cos(doubleDegrees);

            return matrix;
        }

        public static Matrix TranslateMatrix(Vector t)
        {
            Matrix matrix = new Matrix();
            matrix.mat[0, 3] += t.x;
            matrix.mat[1, 3] += t.y;
            matrix.mat[2, 3] += t.z;
            return matrix;
        }

        // ProjectionMatrix needs the z and d


        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    str += mat[i, j] + " ";
                }
                str += "\n";
            }
            return str;
        }
    }
}

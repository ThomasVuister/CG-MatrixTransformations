using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixTransformations
{
    public class Vector
    {
        public float x, y, z, w;

        public Vector()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.w = 0;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
            this.w = 1;
        }

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            float x, y, z;

            x = v1.x + v2.x;
            y = v1.y + v2.y;
            z = v1.z + v2.z;

            return new Vector(x, y, z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            float x, y, z;

            x = v1.x - v2.x;
            y = v1.y - v2.y;
            z = v1.z - v2.z;

            return new Vector(x, y, z);
        }

        public static Vector operator *(Vector v, float f)
        {
            float x, y, z;

            x = v.x * f;
            y = v.y * f;
            z = v.z * f;

            return new Vector(x, y, z);
        }

        public static Vector operator /(float f, Vector v)
        {
            float x, y, z;

            x = v.x / f;
            y = v.y / f;
            z = v.z / f;

            return new Vector(x, y, z);
        }

        public override string ToString()
        {
            return "x = " + x + " y = " + y + " z = " + z;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Timers;

namespace MatrixTransformations
{
    public partial class Form1 : Form
    {
        // Axes
        AxisX x_axis;
        AxisY y_axis;
        AxisZ z_axis;

        // Cube
        Cube cube;

        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        // Transformation variables
        float scale = 1F;
        float degreesZ = 0;
        float degreesX = 0;
        float degreesY = 0;
        float xValue = 0;
        float yValue = 0;
        float zValue = 0;

        // Starting parameters
        float d = 800;
        float r = 10;
        float theta = -100;
        float phi = -10;

        // Matrices for transformations
        Matrix S;
        Matrix R;
        Matrix T;

        public Form1()
        {
            InitializeComponent();

            this.Width = WIDTH;
            this.Height = HEIGHT;
            this.DoubleBuffered = true;

            Vector v1 = new Vector();
            Console.WriteLine(v1);
            Vector v2 = new Vector(1, 2);
            Console.WriteLine(v2);
            Vector v3 = new Vector(2, 6);
            Console.WriteLine(v3);
            Vector v4 = v2 + v3;
            Console.WriteLine(v4); // 3, 8

            Matrix m1 = new Matrix();
            Console.WriteLine(m1); // 1, 0, 0, 1
            Matrix m2 = new Matrix(
                2, 4, 6,
                -1, 3, -3,
                -2, -4, -6);
            Console.WriteLine(m2); // 2, 4, -1, 3
            Console.WriteLine(m1 + m2); // 3, 4, -1, 4
            Console.WriteLine(m1 - m2); // -1, -4, 1, -2
            Console.WriteLine(m2 * m2); // 0, 20, -5, 5

            Console.WriteLine(m2 * v3); // 28, 16

            // Define axes
            x_axis = new AxisX(2);
            y_axis = new AxisY(2);
            z_axis = new AxisZ(2);

            // Create object
            cube = new Cube(Color.Purple);

            // Timer tick += new eventhandler()
            // interval = 50;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            List<Vector> vb;

            base.OnPaint(e);

            // Draw axes
            vb = ViewingPipeline(x_axis.vb, d, r, theta, phi);
            x_axis.Draw(e.Graphics, vb);
            vb = ViewingPipeline(y_axis.vb, d, r, theta, phi);
            y_axis.Draw(e.Graphics, vb);
            vb = ViewingPipeline(z_axis.vb, d, r, theta, phi);
            z_axis.Draw(e.Graphics, vb);

            // Draw cube
            vb.Clear();

            S = Matrix.ScaleMatrix(scale);
            R = Matrix.RotateXMatrix(degreesX) * Matrix.RotateYMatrix(degreesY) * Matrix.RotateZMatrix(degreesZ);
            T = Matrix.TranslateMatrix(new Vector(xValue, yValue, zValue));
            Matrix total = S * R * T;

            foreach (Vector v in cube.vertexbuffer)
                vb.Add(total * v);

            vb = ViewingPipeline(vb, d, r, theta, phi);
            cube.Draw(e.Graphics, vb);

            // Show info
            ShowInfo(e.Graphics);
        }

        public static List<Vector> ViewportTransformation(List<Vector> vb)
        {
            List<Vector> result = new List<Vector>();

            float delta_x = WIDTH / 2;
            float delta_y = HEIGHT / 2;

            foreach (Vector v in vb)
                result.Add(new Vector(v.x + delta_x, delta_y - v.y, v.z));

            return result;
        }

        public static List<Vector> ViewingPipeline(List<Vector> vb, float d, float r, float theta, float phi)
        {
            List<Vector> result = new List<Vector>();
            Vector vp = new Vector();
            Matrix view = Matrix.ViewTransformation(r, theta, phi);

            foreach (Vector v in vb)
            {
                vp = view * v;

                Matrix proj = Matrix.ProjectionTransformation(d, vp.z);
                vp = proj * vp;

                result.Add(vp);
            }
            return ViewportTransformation(result);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();

            // Scale
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.S)
                scale += .1F;
            else if (e.KeyCode == Keys.S)
                scale -= .1F;

            // Translate
            if (e.KeyCode == Keys.Right)
                xValue += 1F;
            if (e.KeyCode == Keys.Left)
                xValue -= 1F;
            if (e.KeyCode == Keys.Up)
                yValue += 1F;
            if (e.KeyCode == Keys.Down)
                yValue -= 1F;
            if (e.KeyCode == Keys.PageUp)
                zValue += 1F;
            if (e.KeyCode == Keys.PageDown)
                zValue -= 1F;

            // RotateX
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.X)
                degreesX += .1F;
            else if (e.KeyCode == Keys.X)
                degreesX -= .1F;

            // RotateY
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Y)
                degreesY += .1F;
            else if (e.KeyCode == Keys.Y)
                degreesY -= .1F;

            // RotateZ
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Z)
                degreesZ += .1F;
            else if (e.KeyCode == Keys.Z)
                degreesZ -= .1F;

            // Change d
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.D)
                d += .1F;
            else if (e.KeyCode == Keys.D)
                d -= .1F;

            // Change r
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.R)
                r += .1F;
            else if (e.KeyCode == Keys.R)
                r -= .1F;

            // Change theta
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.T)
                theta += .1F;
            else if (e.KeyCode == Keys.T)
                theta -= .1F;

            // Change phi
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.P)
                phi += .1F;
            else if (e.KeyCode == Keys.P)
                phi -= .1F;

            // Animation
            if (e.KeyCode == Keys.A)
            {
                // TO DO
            }

            // Reset
            if (e.KeyCode == Keys.C)
            {
                ResetVariables();
            }

            // repaint
            Invalidate();
        }

        private void ResetVariables()
        {
            scale = 1F;
            degreesZ = 0;
            degreesX = 0;
            degreesY = 0;
            xValue = 0;
            yValue = 0;
            zValue = 0;
        }

        private void ShowInfo(Graphics g)
        {
            string s = "";
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            s += String.Format(nfi, "Scale:" + "\t\t" + scale + "\t" + "S / s" + "\n");
            s += String.Format(nfi, "TranslateX:" + "\t" + xValue + "\t" + "Left / Right" + "\n");
            s += String.Format(nfi, "TranslateY:" + "\t" + yValue + "\t" + "Up / Down" + "\n");
            s += String.Format(nfi, "TranslateZ:" + "\t" + zValue + "\t" + "PgUp / PgDn" + "\n");
            s += String.Format(nfi, "RotateX:" + "\t" + degreesX + "\t" + "X / x" + "\n");
            s += String.Format(nfi, "RotateY:" + "\t\t" + degreesY + "\t" + "Y / y" + "\n");
            s += String.Format(nfi, "RotateZ:" + "\t\t" + degreesZ + "\t" + "Z / z" + "\n\n");

            s += String.Format(nfi, "d:" + "\t" + d + "\t" + "D / d" + "\n");
            s += String.Format(nfi, "r:" + "\t" + r + "\t" + "R / r" + "\n");
            s += String.Format(nfi, "theta:" + "\t" + theta + "\t" + "T / t" + "\n");
            s += String.Format(nfi, "phi:" + "\t" + phi + "\t" + "P / p" + "\n\n");

            s += String.Format(nfi, "Phase:");

            PointF p = new PointF(0, 0);
            Font font = new Font("Arial", 10);
            g.DrawString(s, font, Brushes.Black, p);
        }
    }
}

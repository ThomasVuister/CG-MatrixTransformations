﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MatrixTransformations
{
    public partial class Form1 : Form
    {
        // Axes
        AxisX x_axis;
        AxisY y_axis;

        // Objects
        Square square;
        Square cyan_square;
        Square orange_square;
        Square green_square;

        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        // Transformation variables
        float scale = 1.5F;
        float degrees = 20F;
        float xValue = 40F;
        float yValue = 20F;
        float zValue = 0;

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
            x_axis = new AxisX(200);
            y_axis = new AxisY(200);

            // Create object
            square = new Square(Color.Purple);
            cyan_square = new Square(Color.Cyan);
            orange_square = new Square(Color.Orange);
            green_square = new Square(Color.Green);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            List<Vector> vb;

            base.OnPaint(e);

            // Draw axes
            vb = ViewportTransformation(x_axis.vb);
            x_axis.Draw(e.Graphics, vb);
            vb = ViewportTransformation(y_axis.vb);
            y_axis.Draw(e.Graphics, vb);

            // Draw square
            vb = ViewportTransformation(square.vb);
            square.Draw(e.Graphics, vb);
            vb = ScaleTransformation(cyan_square.vb, scale);
            vb = ViewportTransformation(vb);
            cyan_square.Draw(e.Graphics, vb);
            vb = RotateTransformation(orange_square.vb, degrees);
            vb = ViewportTransformation(vb);
            orange_square.Draw(e.Graphics, vb);
            vb = TranslateTransformation(green_square.vb, new Vector(xValue, yValue, zValue));
            vb = ViewportTransformation(vb);
            green_square.Draw(e.Graphics, vb);

            // Better way of doing the transformations, but not sure what to do next
            S = Matrix.ScaleMatrix(scale);
            R = Matrix.RotateMatrix(degrees);
            T = Matrix.TranslateMatrix(new Vector(xValue, yValue, zValue));
            Matrix total = S * R * T;

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

        public static List<Vector> ScaleTransformation(List<Vector> vb, float scale)
        {
            List<Vector> result = new List<Vector>();
            Matrix matrix = Matrix.ScaleMatrix(scale);

            foreach (Vector v in vb)
                result.Add(matrix * v);

            return result;
        }

        public static List<Vector> RotateTransformation(List<Vector> vb, float degrees)
        {
            List<Vector> result = new List<Vector>();
            Matrix matrix = Matrix.RotateMatrix(degrees);

            foreach (Vector v in vb)
                result.Add(matrix * v);

            return result;
        }

        public static List<Vector> TranslateTransformation(List<Vector> vb, Vector vector)
        {
            List<Vector> result = new List<Vector>();
            Matrix matrix = Matrix.TranslateMatrix(vector);

            foreach (Vector v in vb)
                result.Add(matrix * v);

            return result;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();

            // Scale
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.S)
                scale += 0.01F;
            else if (e.KeyCode == Keys.S)
                scale -= 0.01F;

            // Rotate
            if (e.KeyCode == Keys.Oemplus)
                degrees += 0.01F;
            if (e.KeyCode == Keys.OemMinus)
                degrees -= 0.01F;

            // Translate
            if (e.KeyCode == Keys.Up)
                yValue++;
            if (e.KeyCode == Keys.Down)
                yValue--;
            if (e.KeyCode == Keys.Right)
                xValue++;
            if (e.KeyCode == Keys.Left)
                xValue--;

            // repaint
            Invalidate();
        }

        private void ShowInfo(Graphics g)
        {
            string s = "";
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            s += String.Format(nfi, "Scale:" + "\t\t" + scale + "\t" + "S / s" + "\n");
            s += String.Format(nfi, "RotateY:" + "\t\t" + degrees + "\t" + "+ / -" + "\n");
            s += String.Format(nfi, "TranslateX:" + "\t" + xValue + "\t" + "Left / Right" + "\n");
            s += String.Format(nfi, "TranslateY:" + "\t" + yValue + "\t" + "Up / Down" + "\n");

            PointF p = new PointF(0, 0);
            Font font = new Font("Arial", 10);
            g.DrawString(s, font, Brushes.Black, p);
        }
    }
}
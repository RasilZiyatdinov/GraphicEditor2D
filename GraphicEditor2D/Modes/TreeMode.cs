using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;

namespace GraphicEditor2D
{
    public class TreeMode
    {
        public double angle = Math.PI / 2;
        public double ang1 = Math.PI / 4;
        public double ang2 = Math.PI / 6;
        private double lengthScale = 0.75;
        private double deltaTheta = Math.PI / 5;
        Canvas canvasParent;

        public TreeMode(Canvas c)
        {
            canvasParent = c;
            DrawBinaryTree(10, new Point(800 / 2, 0.83 * 800), 0.2 * 800, -Math.PI / 2);
        }
        private void DrawBinaryTree(int depth, Point pt, double length, double theta)
        {
            double x1 = pt.X + length * Math.Cos(theta);
            double y1 = pt.Y + length * Math.Sin(theta);
            Line line = new Line();
            line.Stroke = Brushes.Green;
            line.Tag = "group_tree";
            line.X1 = pt.X;
            line.Y1 = pt.Y;
            line.X2 = x1;
            line.Y2 = y1;
            canvasParent.Children.Add(line);
            if (depth > 1)
            {
                depth--;
                DrawBinaryTree(depth, new Point(x1, y1), length * lengthScale, theta + deltaTheta);
                DrawBinaryTree(depth, new Point(x1, y1), length * lengthScale, theta - deltaTheta);
            }
            else
                return;
        }
    }
}

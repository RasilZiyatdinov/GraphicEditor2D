using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public class Spline
    {
        static int i;
        Polygon polygon;
        Canvas c;
        public Polygon spline;
        public Spline(Polygon obj, Canvas canvas)
        {
            polygon = obj;
            spline = new Polygon();

            spline.Name = "spline_" + obj.Name;
            spline.Tag = "spline";
            spline.Stroke = Brushes.Red;
            spline.StrokeThickness = 3;
            MainWindow.listShapes.Add(spline);
            MainWindow.listPolygones.Add(new MyPolygon(spline));

            CalculateSpline(polygon, ref spline);
            c = canvas;
            c.Children.Add(spline);        
        }


        public static void CalculateSpline(Polygon polygon, ref Polygon spl)
        {
            PointCollection splineCollection  = new PointCollection();

            for (int i = 0; i < polygon.Points.Count; i++)
            {
                Point prev = polygon.Points[i - 1 < 0 ? polygon.Points.Count - 1 : i - 1];
                Point n0 = polygon.Points[i];
                Point n1 = polygon.Points[(i + 1) % polygon.Points.Count];
                Point n2 = polygon.Points[(i + 2) % polygon.Points.Count];

                double a0 = (prev.X + 4 * n0.X + n1.X) / 6;
                double a1 = (-prev.X + n1.X) / 2;
                double a2 = (prev.X - 2 * n0.X + n1.X) / 2;
                double a3 = (-prev.X + 3 * n0.X - 3 * n1.X + n2.X) / 6;

                double b0 = (prev.Y + 4 * n0.Y + n1.Y) / 6;
                double b1 = (-prev.Y + n1.Y) / 2;
                double b2 = (prev.Y - 2 * n0.Y + n1.Y) / 2;
                double b3 = (-prev.Y + 3 * n0.Y - 3 * n1.Y + n2.Y) / 6;

                for (double j = 0; j < 1; j += 0.1)
                {
                    double x = a0 + a1 * j + a2 * j * j + a3 * Math.Pow(j, 3);
                    double y = b0 + b1 * j + b2 * j * j + b3 * Math.Pow(j, 3);

                    splineCollection.Add(new Point(x, y));
                }
            }

            spl.Points.Clear(); spl.Points = splineCollection;
        }
    }
}

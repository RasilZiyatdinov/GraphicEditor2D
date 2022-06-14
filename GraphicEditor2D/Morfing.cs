using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public class Morfing
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        public static PointCollection firstFigure;
        public static PointCollection secondFigure;
        public Canvas canvas;
        public static Polygon newPolygon;
        public static Line newLine;
        public Morfing(ref Canvas c, PointCollection first, PointCollection second)
        {
            ClearMorfing(ref c);

            if (first.Count == second.Count)
            {
                canvas = c;

                T = 0.5;
                if (first.Count == 2)
                {
                    newLine = new Line();
                    newLine.Stroke = Brushes.Red;
                    newLine.StrokeThickness = 3;
                   // newLine.StrokeDashArray = new DoubleCollection();
                   // newLine.StrokeDashArray.Add(5);
                    newLine.Name = "morfing" + newLine.GetHashCode().ToString();

                    newLine.Tag = "morfing" + newLine.GetHashCode().ToString();
                    MyLine ml = new MyLine(newLine);
                    MainWindow.listLines.Add(ml);
                    canvas.Children.Add(newLine);
                }
                else
                {
                    newPolygon = new Polygon();
                    newPolygon.Stroke = Brushes.Red;
                    newPolygon.StrokeThickness = 3;
                   // newPolygon.StrokeDashArray = new DoubleCollection();
                    //newPolygon.StrokeDashArray.Add(5);
                    newPolygon.Name = "morfing" + newPolygon.GetHashCode().ToString();

                    newPolygon.Tag = "morfing" + newPolygon.GetHashCode().ToString();
                    MyPolygon mp = new MyPolygon(newPolygon);
                    MainWindow.listPolygones.Add(mp);
                    canvas.Children.Add(newPolygon);
                }
                firstFigure = first;
                secondFigure = second;
                CalculateMorfing(firstFigure, secondFigure, first.Count != 2);
            }
            else
                MessageBox.Show("Морфинг не возможен, разное количество точек", "Ошибка морфинга", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ClearMorfing(ref Canvas cnv)
        {
            var children = cnv.Children.OfType<UIElement>().ToList();
            foreach (var item in children)
            {
                if ((item as Shape).Tag.ToString().Contains("morfing"))
                {
                    cnv.Children.Remove(item);

                }
            }
        }
        private static double t;
        public static double T
        {
            get => t;
            set
            {
                t = value;
                if (firstFigure != null && secondFigure != null)
                {
                    
                    CalculateMorfing(firstFigure, secondFigure, firstFigure.Count != 2);
                }
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(T)));
            }
        }

        public static void CalculateMorfing(PointCollection start, PointCollection end, bool isPolygon)
        {
            PointCollection newCollection = new PointCollection();
            for (int i = 0; i < start.Count; i++)
            {
                double x = (1 - T) * start[i].X + T * end[i].X;
                double y = (1 - T) * start[i].Y + T * end[i].Y;
                newCollection.Add(new Point(x, y));
            }
            if (isPolygon) newPolygon.Points = newCollection;
            else
            {
                newLine.X1 = newCollection[0].X;
                newLine.X2 = newCollection[1].X;
                newLine.Y1 = newCollection[0].Y;
                newLine.Y2 = newCollection[1].Y;
            }
        }

    }
}

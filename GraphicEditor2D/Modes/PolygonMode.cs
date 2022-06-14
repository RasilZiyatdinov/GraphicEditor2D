using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public class PolygonMode: IMouseOperations
    {
        private Polyline NewPolyline = null;
        Polygon newPolygon;
        int i = 0;
        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (NewPolyline != null)
                {
                    if (NewPolyline.Points.Count > 3)
                    {
                        NewPolyline.Points.RemoveAt(NewPolyline.Points.Count - 1);

                        newPolygon = new Polygon();
                     
                        MyPolygon myPolygon = new MyPolygon(newPolygon);
                        myPolygon.ZCoordinates = default;

                        MainWindow.listShapes.Add(myPolygon);
                        MainWindow.listPolygones.Add(myPolygon);

                        newPolygon.Stroke = new SolidColorBrush(MainWindow.brushes.ColorStroke);
                        newPolygon.StrokeThickness = MainWindow.brushes.Thickness;
                        newPolygon.Points = NewPolyline.Points;

                        canvas.Children.Add(newPolygon);
                    }
                    canvas.Children.Remove(NewPolyline);
                    NewPolyline = null;
                }
                return;
            }

            if (NewPolyline == null)
            {
                NewPolyline = new Polyline();
                NewPolyline.Stroke = new SolidColorBrush(MainWindow.brushes.ColorStroke);
                NewPolyline.StrokeThickness = MainWindow.brushes.Thickness;
                NewPolyline.StrokeDashArray = new DoubleCollection();
                NewPolyline.StrokeDashArray.Add(5);
                NewPolyline.Points.Add(e.GetPosition(canvas));
                canvas.Children.Add(NewPolyline);
            }
            NewPolyline.Points.Add(e.GetPosition(canvas));
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;
            if (NewPolyline == null) return;
            NewPolyline.Points[NewPolyline.Points.Count - 1] = e.GetPosition(canvas);
        }
        public void MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        public void MouseEnter(object sender, MouseEventArgs e)
        {
            if (MainWindow.selector != null)
            {
                for (int i = 0; i < newPolygon.Points.Count; i++)
                {
                    if ((Math.Abs(e.GetPosition(sender as Polygon).X - newPolygon.Points[i].X) < 5) && (Math.Abs(e.GetPosition(sender as Polygon).Y - newPolygon.Points[i].Y) < 5))
                    {
                        (sender as Polygon).Cursor = Cursors.Cross;
                        return;
                    }
                }
            }
        }

        public void MouseLeave(object sender, MouseEventArgs e)
        {
            if (MainWindow.selector != null)
            {
                (sender as Polygon).Cursor = Cursors.Hand;
            }
        }
    }
}

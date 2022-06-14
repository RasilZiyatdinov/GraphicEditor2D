using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace GraphicEditor2D
{
    public class PolygonSelector
    {
        bool isStretch, isMove;
        private Point EditLastPoint;
        Nullable<Point> dragStart = null;
        private int stretchPoint;
        public MyPolygon selectedPolygon;

        public void PolygonMouseDown(IInputElement targetElement, object sender, MouseButtonEventArgs e)
        {
            Canvas c = sender as Canvas;
            ClearBinding(Polygon.StrokeProperty);
            ClearBinding(Polygon.StrokeThicknessProperty);

            selectedPolygon = MainWindow.listPolygones.First(s => (s as MyPolygon).polygon.Name == (targetElement as Shape).Name);
            selectedPolygon.ZCoordinates = default;
     

            StaticClass.ShapeName = selectedPolygon.polygon.Name;
            //StaticClass.ShapeVisible = Visibility.Visible;

            MatrixOperations.myPolygon = selectedPolygon;

            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            myDropShadowEffect.Color = Colors.Green;
            myDropShadowEffect.Direction = 0;
            myDropShadowEffect.ShadowDepth = 7;
            myDropShadowEffect.Softness = .3;
            selectedPolygon.polygon.BitmapEffect = myDropShadowEffect;

            Brush startBrush = selectedPolygon.polygon.Stroke;
            int startThick = (int)selectedPolygon.polygon.StrokeThickness;
            string startName = selectedPolygon.polygon.Name;

            dragStart = e.GetPosition(selectedPolygon.polygon);
            EditLastPoint = e.GetPosition(sender as Canvas);
            isMove = true;
            c.Cursor = Cursors.Hand;

            for (int i = 0; i < selectedPolygon.polygon.Points.Count; i++)
            {
                if ((Math.Abs(dragStart.Value.X - selectedPolygon.polygon.Points[i].X) < 5) && (Math.Abs(dragStart.Value.Y - selectedPolygon.polygon.Points[i].Y) < 5))
                {
                    stretchPoint = i;
                    isStretch = true;
                    isMove = false;
                    c.Cursor = Cursors.Cross;
                }
            }
            if (!isStretch) selectedPolygon.polygon.CaptureMouse();

            SetBinding("BrushStroke", Polygon.StrokeProperty);
            selectedPolygon.polygon.Stroke = startBrush;

            SetBinding("Thickness", Polygon.StrokeThicknessProperty);
            selectedPolygon.polygon.StrokeThickness = startThick;
        }
        public void PolygonMouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart != null && e.LeftButton == MouseButtonState.Pressed && isMove)
            {
                Point p = e.GetPosition(sender as Canvas);
                double dx = p.X - EditLastPoint.X;
                double dy = p.Y - EditLastPoint.Y;
                for (int i = 0; i < selectedPolygon.polygon.Points.Count; i++)
                {
                    Point new_point = new Point(
                           selectedPolygon.polygon.Points[i].X + dx,
                           selectedPolygon.polygon.Points[i].Y + dy);
                    selectedPolygon.polygon.Points[i] = new_point;
                    if (!(selectedPolygon.spline is null)) Spline.CalculateSpline(selectedPolygon.polygon, ref selectedPolygon.spline);
                }
                EditLastPoint = p;
            }
            if (dragStart != null && e.LeftButton == MouseButtonState.Pressed && isStretch)
            {
                Point p2 = e.GetPosition(sender as Canvas);

                double dx = p2.X - EditLastPoint.X;
                double dy = p2.Y - EditLastPoint.Y;
                Point new_point = new Point(
                    selectedPolygon.polygon.Points[stretchPoint].X + dx,
                    selectedPolygon.polygon.Points[stretchPoint].Y + dy);
                selectedPolygon.polygon.Points[stretchPoint] = new_point;
                EditLastPoint = p2;
                if (!(selectedPolygon.spline is null)) Spline.CalculateSpline(selectedPolygon.polygon, ref selectedPolygon.spline);
            }
        }
        public void PolygonMouseUp(object sender, MouseEventArgs e)
        {
            Canvas c = sender as Canvas;
            dragStart = null;
            selectedPolygon.polygon.ReleaseMouseCapture();
            isStretch = false;
            isMove = false;
            c.Cursor = Cursors.Hand;
        }

        public void SetBinding(string path, DependencyProperty dep)
        {
            Binding bind = new Binding();
            bind.Source = MainWindow.brushes;
            bind.Path = new PropertyPath(path);
            bind.Mode = BindingMode.TwoWay;
            selectedPolygon.polygon.SetBinding(dep, bind);

            
        }
        public void ClearBinding(DependencyProperty dep)
        {
            if (selectedPolygon != null)
            {
                Brush b = selectedPolygon.polygon.Stroke;
                int t = (int)selectedPolygon.polygon.StrokeThickness;
                BindingOperations.ClearBinding(selectedPolygon.polygon, dep);
                selectedPolygon.polygon.Stroke = b;
                selectedPolygon.polygon.StrokeThickness = t;
                selectedPolygon.polygon.BitmapEffect = null;
                MatrixOperations.myPolygon = null;
                //StaticClass.ShapeVisible = Visibility.Hidden;
            }
        }
    }
}
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public class SplineSelector
    {
        public Polygon selectedSpline;
        public void SplineMouseDown(IInputElement targetElement, object sender, MouseButtonEventArgs e)
        {
            Canvas c = sender as Canvas;
            ClearBinding(Polygon.StrokeProperty);
            ClearBinding(Polygon.StrokeThicknessProperty);

            //selected = (Polygon)MainWindow.listShapes.First(s => s.Name == (targetElement as Shape).Name);
            selectedSpline = (Polygon)MainWindow.listShapes.First(s => s.Name == (targetElement as Shape).Name);
            StaticClass.ShapeName = selectedSpline.Name;

            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            myDropShadowEffect.Color = Colors.Green;
            myDropShadowEffect.Direction = 0;
            myDropShadowEffect.ShadowDepth = 7;
            myDropShadowEffect.Softness = .3;
            selectedSpline.BitmapEffect = myDropShadowEffect;

            Brush startBrush = selectedSpline.Stroke;
            int startThick = (int)selectedSpline.StrokeThickness;

            SetBinding("BrushStroke", Polygon.StrokeProperty);
            selectedSpline.Stroke = startBrush;

            SetBinding("Thickness", Polygon.StrokeThicknessProperty);
            selectedSpline.StrokeThickness = startThick;
        }


        public void SetBinding(string path, DependencyProperty dep)
        {
            Binding bind = new Binding();
            bind.Source = MainWindow.brushes;
            bind.Path = new PropertyPath(path);
            bind.Mode = BindingMode.TwoWay;
            selectedSpline.SetBinding(dep, bind);
        }
        public void ClearBinding(DependencyProperty dep)
        {
            if (selectedSpline != null)
            {
                Brush b = selectedSpline.Stroke;
                int t = (int)selectedSpline.StrokeThickness;
                BindingOperations.ClearBinding(selectedSpline, dep);
                selectedSpline.Stroke = b;
                selectedSpline.StrokeThickness = t;
                selectedSpline.BitmapEffect = null;
                MatrixOperations.myPolygon = null;
            }
        }
    }
}

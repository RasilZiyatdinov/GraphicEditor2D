using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
   
    public class RectangleMode : IMouseOperations
    {
        private Point startPoint;
        private Rectangle rect;
        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas c = sender as Canvas;
            startPoint = e.GetPosition(sender as Canvas);

            rect = new Rectangle
            {
                Stroke = new SolidColorBrush(MainWindow.brushes.ColorStroke),
                StrokeThickness = MainWindow.brushes.Thickness
            };
            
            Canvas.SetLeft(rect, startPoint.X);
            Canvas.SetTop(rect, startPoint.Y);
            c.Children.Add(rect);
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || rect == null)
                return;

            var pos = e.GetPosition(sender as Canvas);

            var x = Math.Min(pos.X, startPoint.X);
            var y = Math.Min(pos.Y, startPoint.Y);

            var w = Math.Max(pos.X, startPoint.X) - x;
            var h = Math.Max(pos.Y, startPoint.Y) - y;

            rect.Width = w;
            rect.Height = h;

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }

        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.listShapes.Add(rect);
        }
    }
}

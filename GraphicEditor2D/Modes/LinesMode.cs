using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public class LinesMode : IMouseOperations
    {
        public Point? LocalPosition { get; set; }
        Line line;
        MyLine myLine;
        public MyLine lineObject { get; set; }

        private bool IsNew;

        static int i = 0;
        public LinesMode()
        {

        }
        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;

            if (e.ChangedButton == MouseButton.Left)
            {
                LocalPosition = Mouse.GetPosition(canvas);
                IsNew = true;
            }
        }
        public void MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;

            if (e.LeftButton == MouseButtonState.Pressed && LocalPosition.HasValue)
            {
                Point point = e.GetPosition(canvas);

                if (IsNew)
                {
                    line = new Line();
                    //line.Name = "line" + myLine.line.GetHashCode().ToString();
                    //line.Tag = "line" + myLine.line.GetHashCode().ToString();
                    myLine = new MyLine(line);
                    myLine.ZCoordinates = default;
                  
                    myLine.line.Stroke = new SolidColorBrush(MainWindow.brushes.ColorStroke);
                    myLine.line.StrokeThickness = MainWindow.brushes.Thickness;

                    myLine.line.X1 = point.X;
                    myLine.line.X2 = point.X;
                    myLine.line.Y1 = point.Y;
                    myLine.line.Y1 = point.Y;

                    myLine.line.Name = "line" + myLine.line.GetHashCode().ToString();
                    myLine.line.Tag = "line" + myLine.line.GetHashCode().ToString();

                    canvas.Children.Add(line);
                    IsNew = false;
                }
                myLine.line.X2 = point.X; myLine.line.Y2 = point.Y;
            }
        }
        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.listShapes.Add(myLine);
            MainWindow.listLines.Add(myLine);
        }
    }
}

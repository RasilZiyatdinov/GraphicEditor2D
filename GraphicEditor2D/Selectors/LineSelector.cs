using System;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace GraphicEditor2D
{
    public class LineSelector: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private MyLine selectedMyLine;
        private Point local;

        private bool isMove;
        private int stretchPoint = -1;
        public void LineMouseDown(IInputElement targetElement, object sender, MouseButtonEventArgs e)
        {
            ClearBinding(Line.StrokeProperty);
            ClearBinding(Line.StrokeThicknessProperty);

            selectedMyLine = MainWindow.listLines.First(s => (s as MyLine).line.Name == (targetElement as Shape).Name);
            selectedMyLine.ZCoordinates = default;
            StaticClass.ShapeName = selectedMyLine.line.Name;
            MatrixOperations.myLine = selectedMyLine;

            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            myDropShadowEffect.Color = Colors.Green;
            myDropShadowEffect.Direction = 80;
            myDropShadowEffect.ShadowDepth = 7;
            myDropShadowEffect.Softness = .3;
            selectedMyLine.line.BitmapEffect = myDropShadowEffect;

            Brush startBrush = selectedMyLine.line.Stroke;
            int startThick = (int)selectedMyLine.line.StrokeThickness;
            local = e.GetPosition(sender as Canvas);
            (sender as Canvas).Cursor = Cursors.Hand;
            isMove = true;
            if (Math.Abs(local.X - selectedMyLine.line.X1) < 10 && Math.Abs(local.Y - selectedMyLine.line.Y1) < 10)
            {
                stretchPoint = 0;
                isMove = false;
                (sender as Canvas).Cursor = Cursors.Cross;
            }
            else if (Math.Abs(local.X - selectedMyLine.line.X2) < 10 && Math.Abs(local.Y - selectedMyLine.line.Y2) < 10)
            {
                stretchPoint = 1;
                isMove = false;
                (sender as Canvas).Cursor = Cursors.Cross;
            }
            SetBinding("BrushStroke", Line.StrokeProperty);
            selectedMyLine.line.Stroke = startBrush;

            SetBinding("Thickness", Line.StrokeThicknessProperty);
            selectedMyLine.line.StrokeThickness = startThick;
        }
        public void LineMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isMove)
            {
                Point point = e.GetPosition(sender as Canvas);

                selectedMyLine.line.X1 = selectedMyLine.line.X1 + (point.X - local.X);
                selectedMyLine.line.X2 = selectedMyLine.line.X2 + (point.X - local.X);
                selectedMyLine.line.Y1 = selectedMyLine.line.Y1 + (point.Y - local.Y);
                selectedMyLine.line.Y2 = selectedMyLine.line.Y2 + (point.Y - local.Y);

                local = point;
            }
            if (e.LeftButton == MouseButtonState.Pressed && stretchPoint > -1)
            {
                Point point = e.GetPosition(sender as Canvas);
                if (stretchPoint == 0)
                {
                    selectedMyLine.line.X1 = point.X;
                    selectedMyLine.line.Y1 = point.Y;
                }
                else
                {
                    selectedMyLine.line.X2 = point.X;
                    selectedMyLine.line.Y2 = point.Y;
                }
            }
        }

        public void LineMouseUp(object sender, MouseButtonEventArgs e)
        {
            Canvas c = sender as Canvas;
            isMove = false;
            stretchPoint = -1;
            c.Cursor = Cursors.Hand;
        }

        public void SetBinding(string path, DependencyProperty dep)
        {
            Binding bind = new Binding();
            bind.Source = MainWindow.brushes;
            bind.Path = new PropertyPath(path);
            bind.Mode = BindingMode.TwoWay;
            selectedMyLine.line.SetBinding(dep, bind);
        }
        public void ClearBinding(DependencyProperty dep)
        {
            if (selectedMyLine != null)
            {
                Brush b = selectedMyLine.line.Stroke;
                int t = (int)selectedMyLine.line.StrokeThickness;
                BindingOperations.ClearBinding(selectedMyLine.line, dep);
                selectedMyLine.line.Stroke = b;
                selectedMyLine.line.StrokeThickness = t;
                selectedMyLine.line.BitmapEffect = null;
                MatrixOperations.myLine = null;
            }
        }

    }
}

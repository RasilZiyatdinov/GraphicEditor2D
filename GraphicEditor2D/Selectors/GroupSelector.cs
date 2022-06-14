using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class GroupSelector
    {
        public ObservableCollection<string> groupList = new ObservableCollection<string>();
        Shape source;
        Canvas c;
        MyPolygon selectedPolygon;
        MyLine selectedLine;
        Nullable<Point> dragStart = null;
        public void GroupMouseDown(IInputElement targetElement, object sender, MouseButtonEventArgs e)
        {
            c = sender as Canvas;
            source = (Shape)targetElement;
            MatrixOperations.myLine = null;
            MatrixOperations.myPolygon = null;
            if (e.ClickCount == 2)
            {
                DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
                myDropShadowEffect.Color = Colors.Green;
                myDropShadowEffect.Direction = 0;
                myDropShadowEffect.ShadowDepth = 7;
                myDropShadowEffect.Softness = .3;
                source.BitmapEffect = myDropShadowEffect;

                StaticClass.ButtonDeleteVisible = Visibility.Visible;

                if (source is Polygon)
                {
                    selectedPolygon = MainWindow.listPolygones.First(s => (s as MyPolygon).polygon.Name == source.Name);
                    selectedPolygon.ZCoordinates = default;
                    StaticClass.ShapeName = selectedPolygon.polygon.Name;
                    MatrixOperations.myPolygon = selectedPolygon;
                }
                else
                {
                    //selectedLine = MainWindow.listLines.First(s => (s as MyLine).line.Name == source.Name);
                    //selectedLine.ZCoordinates = default;
                    //StaticClass.ShapeName = selectedLine.line.Name;
                    //MatrixOperations.myLine = selectedLine;
                }
                StaticClass.DeletedGroupShape = source;
                return;
            }
            foreach (var item in c.Children)
            {
                if ((item as Shape).Tag.ToString() == source.Tag.ToString())
                {

                    DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
                    myDropShadowEffect.Color = Colors.Green;
                    myDropShadowEffect.Direction = 0;
                    myDropShadowEffect.ShadowDepth = 7;
                    myDropShadowEffect.Softness = .3;
                    (item as Shape).BitmapEffect = myDropShadowEffect;                  
                }
            }

            dragStart = e.GetPosition(source);
            source.CaptureMouse();
        }
        public void GroupMouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart != null)
            {
                var p2 = e.GetPosition(c);
                Canvas.SetLeft(source, p2.X - dragStart.Value.X);
                Canvas.SetTop(source, p2.Y - dragStart.Value.Y);
                foreach (var item in c.Children)
                {   
                    if ((item as Shape).Tag.ToString() == source.Tag.ToString())
                    {
                        Canvas.SetLeft(item as Shape, p2.X - dragStart.Value.X);
                        Canvas.SetTop(item as Shape, p2.Y - dragStart.Value.Y);
                    }
                }
            }
        }
        public void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragStart = null;
            source.ReleaseMouseCapture();
        }

        public void ClearBinding()
        {
            if (source != null)
            {
                groupList.Clear();
                StaticClass.GrList = groupList;
                StaticClass.GroupName = "";

                foreach (var item in c.Children)
                {
                    if ((item as Shape).Tag.ToString() == source.Tag.ToString())
                    {
                        (item as Shape).BitmapEffect = null;
                    }
                }
                StaticClass.ButtonDeleteVisible = Visibility.Hidden;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class SelectMode : IMouseOperations
    {
        private IInputElement targetElement;
        public LineSelector lineSelector;
        public PolygonSelector polygonSelector;
        public SplineSelector splineSelector;
        public GroupSelector groupSelector;

        bool isLine, isGroup, isPolygon; Brush b; int t;
        public SelectMode()
        {
            b = MainWindow.brushes.BrushStroke;
            t = MainWindow.brushes.Thickness;
        }

        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ClearSelected();
                targetElement = e.Source as IInputElement;
                if (targetElement.GetType().Equals(typeof(Line)) && (targetElement as Line).Name != null)
                {
                    if ((targetElement as Line).Tag.ToString().Contains("group"))
                    {
                        groupSelector = new GroupSelector();
                        groupSelector.GroupMouseDown(targetElement, sender, e);
                        isGroup = true;
                        return;
                    }
                    lineSelector = new LineSelector();
                    lineSelector.LineMouseDown(targetElement, sender, e);
                    isLine = true;
                }
                else if (targetElement.GetType().Equals(typeof(Polygon)))
                {
                    if ((targetElement as Polygon).Tag.ToString().Contains("group"))
                    {
                        groupSelector = new GroupSelector();
                        groupSelector.GroupMouseDown(targetElement, sender, e);
                        isGroup = true;
                        return;
                    }
                    if ((targetElement as Polygon).Tag == "spline")
                    {
                        splineSelector = new SplineSelector();
                        splineSelector.SplineMouseDown(targetElement, sender, e);
                        isPolygon = false;
                        return;
                    }
                    polygonSelector = new PolygonSelector();
                    polygonSelector.PolygonMouseDown(targetElement, sender, e);
                    isPolygon = true;
                }
                else
                {
                    MainWindow.brushes.BrushStroke = b;
                    MainWindow.brushes.Thickness = t;
                    StaticClass.ShapeName = "";
                }
            }
        }
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (isLine)
                lineSelector.LineMouseMove(sender, e);
            if (isPolygon)
                polygonSelector.PolygonMouseMove(sender, e);
            if (isGroup)
                groupSelector.GroupMouseMove(sender, e);
        }
        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isLine)
            {
                lineSelector.LineMouseUp(sender, e);
                isLine = false;
            }
            if (isPolygon)
            {
                polygonSelector.PolygonMouseUp(sender, e);
                isPolygon = false;
            }
            if (isGroup)
            {
                groupSelector.MouseButtonUp(sender, e);
                isGroup = false;
            }
        }

        public void ClearSelected()
        {
            if (lineSelector != null)
            {
                lineSelector.ClearBinding(Line.StrokeProperty);
                lineSelector.ClearBinding(Line.StrokeThicknessProperty);
                lineSelector = null;
            }
            if (polygonSelector != null)
            {
                polygonSelector.ClearBinding(Polygon.StrokeProperty);
                polygonSelector.ClearBinding(Polygon.StrokeThicknessProperty);
                polygonSelector = null;
            }
            if (splineSelector != null)
            {
                splineSelector.ClearBinding(Polygon.StrokeProperty);
                splineSelector.ClearBinding(Polygon.StrokeThicknessProperty);
                splineSelector = null;
            }
            if (groupSelector != null)
            {
                groupSelector.ClearBinding();
                groupSelector = null;
            }
        }
        public void DeleteShape(Canvas canv)
        {
            if (targetElement != null)
            {
                if (targetElement is Canvas)
                {
                    canv.Children.Remove(targetElement as UIElement);
                    return;
                }
                var children = canv.Children.OfType<UIElement>().ToList();

                foreach (var item in children)
                {
                    if ((item as Shape).BitmapEffect != null)
                        canv.Children.Remove(item as UIElement);
                }
                MainWindow.brushes.BrushStroke = b;
                MainWindow.brushes.Thickness = t;
            }
        }
        public bool IsPolygonSelected()
        {
            if (polygonSelector == null) return false;
            return true;
        }
        public void SetName(string name)
        {
            if (targetElement is Shape)
            {
                try
                {
                    (targetElement as Shape).Name = name;
                    return;
                }
                catch(Exception e)
                {
                    MessageBox.Show("Недопустимое имя объекта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (targetElement is Canvas)
            {
                (targetElement as Canvas).Name = name;
                return;
            }
        }
    }
}

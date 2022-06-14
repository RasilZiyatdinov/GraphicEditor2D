using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public class MorfingMode
    {
        Morfing m;
        private IInputElement targetElement;
        public List<Shape> morfList = new List<Shape>();
        PointCollection first;
        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Canvas c = sender as Canvas;
                targetElement = e.Source as IInputElement;
               
                if (targetElement is Shape)
                {
                    if (morfList.Count == 2)
                    {
                        return;
                    }
                    Shape selected = targetElement as Shape;
                    DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
                    myDropShadowEffect.Color = Colors.Green;
                    myDropShadowEffect.Direction = 0;
                    myDropShadowEffect.ShadowDepth = 7;
                    myDropShadowEffect.Softness = .3;
                    selected.BitmapEffect = myDropShadowEffect;
                    morfList.Add(selected);
                    if (morfList.Count == 2)
                    {                     
                        m = new Morfing(ref c, ToCollect(morfList[0]), ToCollect(morfList[1]));                       
                    }
                }
                else
                {
                    if (morfList.Count != 0)
                    {
                        foreach (var item in morfList)
                        {
                            item.BitmapEffect = null;
                        }
                        morfList.Clear();
                        Morfing.firstFigure = null;
                        Morfing.secondFigure = null;
                        if (m != null) m.ClearMorfing(ref c);
                    }
                }
            }
        }
        public PointCollection ToCollect(Shape item)
        {
            first = new PointCollection();
            if (item is Line)
            {
                Point p1 = new Point((item as Line).X1, (item as Line).Y1);
                Point p2 = new Point((item as Line).X2, (item as Line).Y2);
                first.Add(p1);
                first.Add(p2);                
            }
            if (item is Polygon)
            {
                first = (item as Polygon).Points;
            }
            return first;
        }
        public void ClearSelected(Canvas cnv)
        {
            var children = cnv.Children.OfType<UIElement>().ToList();
            foreach (var item in children)
            {
                if ((item as Shape).BitmapEffect != null)
                {
                    (item as Shape).BitmapEffect = null;
                }
            }
        }
    }
}

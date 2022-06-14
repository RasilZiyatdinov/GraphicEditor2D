using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class Group
    {
        private IInputElement targetElement;
        public static ObservableCollection<string> groupList;
        int i;
        public Group()
        {
            groupList = new ObservableCollection<string>();
        }
        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Canvas c = sender as Canvas;
                targetElement = e.Source as IInputElement;
                if (targetElement is Shape)
                {
                    Shape selected = targetElement as Shape;
                    if (selected.Tag.ToString().Contains("spline")) return;
                    if (selected is Polygon)
                    {
                        MyPolygon selectedPolygon = MainWindow.listPolygones.First(s => (s as MyPolygon).polygon.Name == (targetElement as Shape).Name);
                        if (selectedPolygon.spline != null)
                        {
                            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
                            myDropShadowEffect.Color = Colors.Green;
                            myDropShadowEffect.Direction = 0;
                            myDropShadowEffect.ShadowDepth = 7;
                            myDropShadowEffect.Softness = .3;
                            selectedPolygon.spline.BitmapEffect = myDropShadowEffect;
                            groupList.Add(selectedPolygon.spline.Name);
                            StaticClass.GrList = groupList;
                        }
                    }

                    StaticClass.ButtonVisible = System.Windows.Visibility.Visible;

                    foreach (var item in c.Children)
                    {
                        if ((item as Shape).Tag.ToString() == selected.Tag.ToString())
                        {                            
                            if (!groupList.Contains((item as Shape).Name))
                            {
                                groupList.Add((item as Shape).Name);
                                StaticClass.GrList = groupList;
                                DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
                                myDropShadowEffect.Color = Colors.Green;
                                myDropShadowEffect.Direction = 0;
                                myDropShadowEffect.ShadowDepth = 7;
                                myDropShadowEffect.Softness = .3;
                                (item as Shape).BitmapEffect = myDropShadowEffect;

                            } 
                            else
                            {
                                groupList.Remove((item as Shape).Name);
                                StaticClass.GrList = groupList;
                                (item as Shape).BitmapEffect = null;
                            }
                        }
                    }
                }
                else
                {
                    ClearSelected(sender as Canvas);
                    groupList.Clear();
                    StaticClass.GrList = groupList;
                    StaticClass.GroupName = "";
                    StaticClass.ButtonVisible = System.Windows.Visibility.Hidden;
                }
            }
        }
        public void ClearSelected(Canvas cnv)
        {
            var children = cnv.Children.OfType<UIElement>().ToList();
            foreach (var item in children)
            {
                if (item is Shape)
                    (item as Shape).BitmapEffect = null;
            }
        }
    }
}

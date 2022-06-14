using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public sealed class MyPolygon: Shape
    {
        public Polygon polygon;
        public Polygon spline;
        public Morfing morfing;

        public string Name;
        private List<double> zCoordinates;
        public List<double> ZCoordinates
        {
            get { return zCoordinates; }
            set
            {
                zCoordinates = new List<double>();
                for (int i = 0; i < polygon.Points.Count; i++)
                {
                    zCoordinates.Add(0);
                }
            }
        }

        protected override Geometry DefiningGeometry => throw new NotImplementedException();
        public MyPolygon(Polygon obj)
        {
            polygon = obj;
            polygon.Name = "polygone" + polygon.GetHashCode().ToString();
            polygon.Tag = "polygone" + polygon.GetHashCode().ToString();
            polygon.MouseEnter += MouseEnter;
            polygon.MouseLeave += MouseLeave;
            Name = polygon.Name;
        }
        public MyPolygon(Polygon obj, string name, object tag)
        {
            polygon = obj;
            polygon.Name = name;
            polygon.Tag = tag;
            polygon.MouseEnter += MouseEnter;
            polygon.MouseLeave += MouseLeave;
            Name = polygon.Name;
        }

        public void MouseEnter(object sender, MouseEventArgs e)
        {
            if (MainWindow.selector != null)
            {
                for (int i = 0; i < polygon.Points.Count; i++)
                {
                    if ((Math.Abs(e.GetPosition(sender as Polygon).X - polygon.Points[i].X) < 5) && (Math.Abs(e.GetPosition(sender as Polygon).Y - polygon.Points[i].Y) < 5))
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

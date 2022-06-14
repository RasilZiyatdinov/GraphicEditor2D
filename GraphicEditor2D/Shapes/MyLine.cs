using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public sealed class MyLine: Shape
    {
        public Line line;
        public string Name;
        private List<double> zCoordinates;
        public List<double> ZCoordinates
        {
            get { return zCoordinates; }
            set
            {
                zCoordinates = new List<double>();
                for (int i = 0; i < 2; i++)
                {
                    zCoordinates.Add(0);
                }
            }
        }

        public PointCollection Points
        {
            get
            {
                PointCollection points = new PointCollection();
                points.Add(new Point(line.X1, line.Y1));
                points.Add(new Point(line.X2, line.Y2));
                return points;
            }
        }

        protected override Geometry DefiningGeometry => throw new NotImplementedException();

        public MyLine(Line obj)
        {
            line = obj;
            line.Name = "line" + line.GetHashCode().ToString();
            line.Tag = "line" + line.GetHashCode().ToString();
            line.MouseEnter += MouseEnter;
            line.MouseLeave += MouseLeave;
            Name = line.Name;
        }
        public MyLine(Line obj, string name, object tag)
        {
            line = obj;
            line.Name = name;
            line.Tag = tag;
            line.MouseEnter += MouseEnter;
            line.MouseLeave += MouseLeave;
            Name = line.Name;
        }

        public void MouseEnter(object sender, MouseEventArgs e)
        {
            if (MainWindow.selector != null)
            {
                if (Math.Abs(e.GetPosition(sender as Line).X - (sender as Line).X1) < 5 && Math.Abs(e.GetPosition(sender as Line).Y - (sender as Line).Y1) < 5)
                {
                    (sender as Line).Cursor = Cursors.Cross;
                    return;
                }
                if (Math.Abs(e.GetPosition(sender as Line).X - (sender as Line).X2) < 5 && Math.Abs(e.GetPosition(sender as Line).Y - (sender as Line).Y2) < 5)
                {
                    (sender as Line).Cursor = Cursors.Cross;
                    return;

                }
            }
        }
        public void MouseLeave(object sender, MouseEventArgs e)
        {
            if (MainWindow.selector != null)
            {
                (sender as Line).Cursor = Cursors.Hand;
            }
        }
    }
}

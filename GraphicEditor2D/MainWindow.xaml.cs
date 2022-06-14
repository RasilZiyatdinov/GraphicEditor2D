using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BrushSettings brushes;
        public Color curentBrush;
        public int currentThickness;
        public static List<Shape> listShapes;
        public static List<MyPolygon> listPolygones;
        public static List<MyLine> listLines;
        public MorfingMode mf;
        Group g;
        public static Spline spl;
        int i;

        public static IMouseOperations shapeLine = new LinesMode(), shapePolygone = new PolygonMode(), shapeRectangle = new RectangleMode(), selector;
        public MainWindow()
        {
            InitializeComponent();
            brushes = new BrushSettings(2, Colors.Black);
            myColorPicker.DataContext = brushes;

            MatrixOperations matrixOperations = new MatrixOperations();
            thickness.DataContext = brushes;
            angleY.DataContext = matrixOperations;
            angleZ.DataContext = matrixOperations;         
            angleX.DataContext = matrixOperations;
            scale.DataContext = matrixOperations;
            i = 0;
            List<string> mirrorList = new List<string> { "X = 0", "Y = 0"};
            mirror.ItemsSource = mirrorList;
            mirror.DataContext = matrixOperations;

            listShapes = new List<Shape>();
            listPolygones = new List<MyPolygon>();
            listLines = new List<MyLine>();

            StaticClass.ButtonVisible = Visibility.Hidden;
            StaticClass.ButtonDeleteVisible = Visibility.Hidden;
        }

        //......Operations..................//
        private void LineCreateClick(object sender, RoutedEventArgs e)
        {
            statusLine.Clear();
            Remove();    
            myCanvas.Cursor = Cursors.Pen;
            if (selector != null)
                (selector as SelectMode).ClearSelected();
            if (mf != null)
                mf.ClearSelected(myCanvas);
            if (g != null)
                g.ClearSelected(myCanvas);
            Mouse.AddMouseDownHandler(myCanvas, shapeLine.MouseDown);
            Mouse.AddMouseMoveHandler(myCanvas, shapeLine.MouseMove);
            Mouse.AddMouseUpHandler(myCanvas, shapeLine.MouseUp);
        }
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            //statusLine.Clear();
            Remove();
            myCanvas.Cursor = Cursors.Hand;
            if (selector == null)
                selector = new SelectMode();
            if (mf != null)
                mf.ClearSelected(myCanvas);
            if (g != null)
                g.ClearSelected(myCanvas);
            Mouse.AddMouseDownHandler(myCanvas, selector.MouseDown);
            Mouse.AddMouseMoveHandler(myCanvas, selector.MouseMove);
            Mouse.AddMouseUpHandler(myCanvas, selector.MouseUp);
        }
        private void CreateCanvasClick(object sender, RoutedEventArgs e)
        {
            statusLine.Clear();
            MessageBoxResult result = MessageBox.Show("Сохранить файл?", "Сохранение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveToXaml(default, default);
                    myCanvas.Children.Clear();
                    break;
                case MessageBoxResult.No:
                    myCanvas.Children.Clear();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        private void Remove()
        {        
            if (shapeLine != null)
            {
                Mouse.RemoveMouseDownHandler(myCanvas, shapeLine.MouseDown);
                Mouse.RemoveMouseMoveHandler(myCanvas, shapeLine.MouseMove);
                Mouse.RemoveMouseUpHandler(myCanvas, shapeLine.MouseUp);               
            }
            if (shapePolygone != null)
            {
                Mouse.RemoveMouseDownHandler(myCanvas, shapePolygone.MouseDown);
                Mouse.RemoveMouseMoveHandler(myCanvas, shapePolygone.MouseMove);
                Mouse.RemoveMouseUpHandler(myCanvas, shapePolygone.MouseUp);
            }
            if (shapeRectangle!= null)
            {
                Mouse.RemoveMouseDownHandler(myCanvas, shapeRectangle.MouseDown);
                Mouse.RemoveMouseMoveHandler(myCanvas, shapeRectangle.MouseMove);
                Mouse.RemoveMouseUpHandler(myCanvas, shapeRectangle.MouseUp);
            }
            if (selector != null)
            {
                
                Mouse.RemoveMouseDownHandler(myCanvas, selector.MouseDown);
                Mouse.RemoveMouseMoveHandler(myCanvas, selector.MouseMove);
                Mouse.RemoveMouseUpHandler(myCanvas, selector.MouseUp);
            }
            if (mf != null)
            {
                Mouse.RemoveMouseDownHandler(myCanvas, mf.MouseDown);
            }
            if (g != null)
            {
                Mouse.RemoveMouseDownHandler(myCanvas, g.MouseDown);
                StaticClass.ButtonVisible = System.Windows.Visibility.Hidden;
            }
        }      
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            statusLine.Clear();
            if (selector != null) (selector as SelectMode).DeleteShape(myCanvas);        
        }
        private void RectangleCreateButtonClick(object sender, RoutedEventArgs e)
        {
            statusLine.Clear();
            Remove();
            myCanvas.Cursor = Cursors.Pen;
            if (selector != null)
                (selector as SelectMode).ClearSelected();
            Mouse.AddMouseDownHandler(myCanvas, shapeRectangle.MouseDown);
            Mouse.AddMouseMoveHandler(myCanvas, shapeRectangle.MouseMove);
            Mouse.AddMouseUpHandler(myCanvas, shapeRectangle.MouseUp);
        }
        private void MirrorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mirror.SelectedIndex = -1;
            return;
        }
        private void ShapeNameChanged(object sender, TextChangedEventArgs e)
        {
            if (selector != null)
            {          
                if (statusLine.Text.Length != 0) (selector as SelectMode).SetName(statusLine.Text);
            }
        }
        private void TreeCreateButtonClick(object sender, RoutedEventArgs e)
        {
            TreeMode tree = new TreeMode(myCanvas);
        }
        private void MorfingButtonClick(object sender, RoutedEventArgs e)
        {
            statusLine.Clear();
            Remove();
            myCanvas.Cursor = Cursors.Hand;
            if (selector != null)
            {
                (selector as SelectMode).ClearSelected();
            }
            if (mf != null)
            {
               mf.ClearSelected(myCanvas);
            }
            if (g != null)
                g.ClearSelected(myCanvas);
            mf = new MorfingMode();
            Mouse.AddMouseDownHandler(myCanvas, mf.MouseDown);
        }
        private void SplineButtonClick(object sender, RoutedEventArgs e)
        {
            if (selector == null) return;
            if (!(selector as SelectMode).IsPolygonSelected()) return;

            if ((selector as SelectMode).polygonSelector.selectedPolygon.polygon != null)
            {
                spl = new Spline((selector as SelectMode).polygonSelector.selectedPolygon.polygon, myCanvas);
                (selector as SelectMode).polygonSelector.selectedPolygon.spline = spl.spline;
            }
        }
        private void PolygonButtonClick(object sender, RoutedEventArgs e)
        {
            statusLine.Clear();
            Remove();
            myCanvas.Cursor = Cursors.Pen;
            if (selector != null)
                (selector as SelectMode).ClearSelected();
            if (mf != null)
                mf.ClearSelected(myCanvas);
            if (g != null)
                g.ClearSelected(myCanvas);
            Mouse.AddMouseDownHandler(myCanvas, shapePolygone.MouseDown);
            Mouse.AddMouseMoveHandler(myCanvas, shapePolygone.MouseMove);
            Mouse.AddMouseUpHandler(myCanvas, shapePolygone.MouseUp);
        }
        private void groupButtonClick(object sender, RoutedEventArgs e)
        {
            statusLine.Clear();
            Remove();

            myCanvas.Cursor = Cursors.Hand;
            if (selector != null)
                (selector as SelectMode).ClearSelected();
            if (mf != null)
                mf.ClearSelected(myCanvas);
            if (g != null)
                g.ClearSelected(myCanvas);
            i++;
            g = new Group();

            Mouse.AddMouseDownHandler(myCanvas, g.MouseDown);
        }
        private void GroupListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void AddGroupButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in StaticClass.GrList)
            {
                foreach (Shape chil in myCanvas.Children)
                {
                    if (chil.Name == item)
                        chil.Tag = "group" + i;
                }
            }
            g.ClearSelected(myCanvas);
            StaticClass.ButtonVisible = Visibility.Hidden;
            StaticClass.GrList.Clear();
            i++;
        }
        private void DeleteFromGroup(object sender, RoutedEventArgs e)
        {
            StaticClass.DeletedGroupShape.Tag = StaticClass.DeletedGroupShape.Name;
            StaticClass.ButtonDeleteVisible = Visibility.Hidden;
            if (selector != null)
                (selector as SelectMode).ClearSelected();
        }

        //.......Save to .png / .jpg / .xaml........//
        private void SaveToImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|All(*.*)|*",
                FileName = "Nameless",
                DefaultExt = "png",
            };
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            myCanvas.Measure(new Size((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight));
            myCanvas.Arrange(new Rect(new Size((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight)));
            renderBitmap.Render(myCanvas);
            InvalidateVisual();
            if (saveFileDialog.ShowDialog() == true)
            {
                var extension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                using (FileStream file = File.Create(saveFileDialog.FileName))
                {
                    BitmapEncoder encoder = null;
                    switch (extension.ToLower())
                    {
                        case ".jpg":
                            encoder = new JpegBitmapEncoder();
                            break;
                        case ".png":
                            encoder = new PngBitmapEncoder();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(extension);
                    }
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(file);
                }
            }
        }
        private void SaveToXaml(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                FileName = "nameless",
                DefaultExt = "xaml",
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fs = File.Create(saveFileDialog.FileName))
                {
                    XamlWriter.Save(myCanvas, fs);
                }
            }
        }

        //.......Open from file.....................//
        private void OpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "XAML (*.xaml)|*.xaml",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream f = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                Canvas savedCanvas = XamlReader.Load(f) as Canvas;
                listLines.Clear();
                listPolygones.Clear();
                listShapes.Clear();
                myCanvas.Children.Clear();
                while (savedCanvas.Children.Count > 0)
                {
                    UIElement uie = savedCanvas.Children[0];
                    listShapes.Add(uie as Shape);
                    if (uie is Line)
                        listLines.Add(new MyLine(uie as Line, (uie as Line).Name, (uie as Line).Tag));
                    if (uie is Polygon)
                        listPolygones.Add(new MyPolygon(uie as Polygon, (uie as Polygon).Name, (uie as Polygon).Tag));
                    savedCanvas.Children.Remove(uie);
                    myCanvas.Children.Add(uie);
                }
            }
        }

    }
}

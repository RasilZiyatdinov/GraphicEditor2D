using System;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Controls;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;

namespace GraphicEditor2D
{
    public class MatrixOperations: INotifyPropertyChanged
    {
        private double angleX, angleY, angleZ;
        private double scale;
        private string mirror;
        public static Canvas c;
        public static MyPolygon myPolygon;
        public static MyLine myLine;

        public event PropertyChangedEventHandler PropertyChanged;
        public string Mirror
        {
            get => mirror;
            set
            {
                mirror = value;
                if (myPolygon != null)
                {
                    Point centr = FindCentroid(myPolygon.polygon.Points);
                    Matrix<double> matr = MatrixTransfer4D(-centr.X, -centr.Y, PointsToMatrix4D(myPolygon.polygon.Points, myPolygon.ZCoordinates));
                    if (mirror == "X = 0")
                    {
                        matr = MatrixMirror4D("X", matr);
                    }
                    if (mirror == "Y = 0")
                    {
                        matr = MatrixMirror4D("Y", matr);
                    }
                    matr = MatrixTransfer4D(centr.X, centr.Y, matr);
                    for (int i = 0; i < myPolygon.polygon.Points.Count; i++)
                    {
                        myPolygon.polygon.Points[i] = new Point(matr[i, 0], matr[i, 1]);
                        myPolygon.ZCoordinates[i] = matr[i, 2];
                    }
                    if (!(myPolygon.spline is null)) Spline.CalculateSpline(myPolygon.polygon, ref myPolygon.spline);
                }
                if (myLine != null)
                {
                    Matrix<double> matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / -2, (myLine.line.Y1 + myLine.line.Y2) / -2, PointsToMatrix4D(myLine.Points, myLine.ZCoordinates));
                    if (value == "X = 0")
                    {
                        matr = MatrixMirror4D("X", matr);
                    }
                    if (value == "Y = 0")
                    {
                        matr = MatrixMirror4D("Y", matr);
                    }
                    matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / 2, (myLine.line.Y1 + myLine.line.Y2) / 2, matr);

                    myLine.line.X1 = matr[0, 0];
                    myLine.line.Y1 = matr[0, 1];
                    myLine.line.X2 = matr[1, 0];
                    myLine.line.Y2 = matr[1, 1];

                    myLine.ZCoordinates[0] = matr[0, 2];
                    myLine.ZCoordinates[1] = matr[1, 2];
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mirror"));
            }
        }
        public double Scale
        {
            get => scale;
            set
            {
                double s = scale;
                scale = value;
                if (myPolygon != null)
                {
                    Point centr = FindCentroid(myPolygon.polygon.Points);
                    Matrix<double> matr = MatrixTransfer4D(-centr.X, -centr.Y, PointsToMatrix4D(myPolygon.polygon.Points, myPolygon.ZCoordinates));
                    matr = MatrixScale4D(scale - s + 1, matr);
                    matr = MatrixTransfer4D(centr.X, centr.Y, matr);

                    for (int i = 0; i < myPolygon.polygon.Points.Count; i++)
                    {
                        myPolygon.polygon.Points[i] = new Point(matr[i, 0], matr[i, 1]);
                        myPolygon.ZCoordinates[i] = matr[i, 2];
                    }
                    //if (!(MainWindow.spl is null)) MainWindow.spl.CalculateSpline();
                    if (!(myPolygon.spline is null)) Spline.CalculateSpline(myPolygon.polygon, ref myPolygon.spline);
                }
                if (myLine != null)
                {
                    Matrix<double> matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / -2, (myLine.line.Y1 + myLine.line.Y2) / -2, PointsToMatrix4D(myLine.Points, myLine.ZCoordinates));
                    matr = MatrixScale4D(scale - s + 1, matr);
                    matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / 2, (myLine.line.Y1 + myLine.line.Y2) / 2, matr);
                    
                    myLine.line.X1 = matr[0, 0];
                    myLine.line.Y1 = matr[0, 1];
                    myLine.line.X2 = matr[1, 0];
                    myLine.line.Y2 = matr[1, 1];

                    myLine.ZCoordinates[0] = matr[0, 2];
                    myLine.ZCoordinates[1] = matr[1, 2];
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Scale"));
            }
        }

        public double AngleX
        {
            get => angleX;
            set
            {
                double a = angleX - value;

                angleX = value;


                if (myPolygon != null)
                {
                    Point centr = FindCentroid(myPolygon.polygon.Points);
                    Matrix<double> matr = MatrixTransfer4D(-centr.X, -centr.Y, PointsToMatrix4D(myPolygon.polygon.Points, myPolygon.ZCoordinates));
                    matr = MatrixRotate4DX((a), matr);
                    matr = MatrixTransfer4D(centr.X, centr.Y, matr);


                    for (int i = 0; i < myPolygon.polygon.Points.Count; i++)
                    {
                        myPolygon.polygon.Points[i] = new Point((matr[i, 0]), (matr[i, 1]));
                        myPolygon.ZCoordinates[i] = matr[i, 2];
                    }
                    //if (!(MainWindow.spl is null)) MainWindow.spl.CalculateSpline();
                    if (!(myPolygon.spline is null)) Spline.CalculateSpline(myPolygon.polygon, ref myPolygon.spline);

                }

                if (myLine != null)
                {
                    Matrix<double> matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / -2, (myLine.line.Y1 + myLine.line.Y2) / -2, PointsToMatrix4D(myLine.Points, myLine.ZCoordinates));
                    matr = MatrixRotate4DX((a), matr);
                    matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / 2, (myLine.line.Y1 + myLine.line.Y2) / 2, matr);


                    myLine.line.X1 = matr[0, 0];
                    myLine.line.Y1 = matr[0, 1];
                    myLine.line.X2 = matr[1, 0];
                    myLine.line.Y2 = matr[1, 1];

                    myLine.ZCoordinates[0] = matr[0, 2];
                    myLine.ZCoordinates[1] = matr[1, 2];
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Angle"));
            }
        }
        public double AngleY
        {
            get => angleY;
            set
            {
                double a = angleY - value;
                angleY = value;

                if (myPolygon != null)
                {
                    Point centr = FindCentroid(myPolygon.polygon.Points);
                    Matrix<double> matr = MatrixTransfer4D(-centr.X, -centr.Y, PointsToMatrix4D(myPolygon.polygon.Points, myPolygon.ZCoordinates));
                    matr = MatrixRotate4DY((a), matr);
                    matr = MatrixTransfer4D(centr.X, centr.Y, matr);


                    for (int i = 0; i < myPolygon.polygon.Points.Count; i++)
                    {
                        myPolygon.polygon.Points[i] = new Point((matr[i, 0]), (matr[i, 1]));
                        myPolygon.ZCoordinates[i] = matr[i, 2];
                    }
                    //if (!(MainWindow.spl is null)) MainWindow.spl.CalculateSpline();
                    if (!(myPolygon.spline is null)) Spline.CalculateSpline(myPolygon.polygon, ref myPolygon.spline);
                }
                if (myLine != null)
                {
                    Matrix<double> matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2)/-2, (myLine.line.Y1 + myLine.line.Y2) / -2, PointsToMatrix4D(myLine.Points, myLine.ZCoordinates));
                    matr = MatrixRotate4DY((a), matr);
                    matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / 2, (myLine.line.Y1 + myLine.line.Y2) / 2, matr);


                    myLine.line.X1 = matr[0, 0];
                    myLine.line.Y1 = matr[0, 1];
                    myLine.line.X2 = matr[1, 0];
                    myLine.line.Y2 = matr[1, 1];

                    myLine.ZCoordinates[0] = matr[0, 2];
                    myLine.ZCoordinates[1] = matr[1, 2];
                }
            }
        }
        public double AngleZ
        {
            get => angleZ;
            set
            {
                double a = angleZ - value;

                angleZ = value;


                if (myPolygon != null)
                {
                    Point centr = FindCentroid(myPolygon.polygon.Points);
                    Matrix<double> matr = MatrixTransfer4D(-centr.X, -centr.Y, PointsToMatrix4D(myPolygon.polygon.Points, myPolygon.ZCoordinates));
                    matr = MatrixRotate4DZ((a), matr);
                    matr = MatrixTransfer4D(centr.X, centr.Y, matr);


                    for (int i = 0; i < myPolygon.polygon.Points.Count; i++)
                    {
                        myPolygon.polygon.Points[i] = new Point((matr[i, 0]), (matr[i, 1]));
                        myPolygon.ZCoordinates[i] = matr[i, 2];
                    }
                    if (!(myPolygon.spline is null)) Spline.CalculateSpline(myPolygon.polygon, ref myPolygon.spline);
                }
                if (myLine != null)
                {
                    Matrix<double> matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / -2, (myLine.line.Y1 + myLine.line.Y2) / -2, PointsToMatrix4D(myLine.Points, myLine.ZCoordinates));
                    matr = MatrixRotate4DZ((a), matr);
                    matr = MatrixTransfer4D((myLine.line.X1 + myLine.line.X2) / 2, (myLine.line.Y1 + myLine.line.Y2) / 2, matr);


                    myLine.line.X1 = matr[0, 0];
                    myLine.line.Y1 = matr[0, 1];
                    myLine.line.X2 = matr[1, 0];
                    myLine.line.Y2 = matr[1, 1];

                    myLine.ZCoordinates[0] = matr[0, 2];
                    myLine.ZCoordinates[1] = matr[1, 2];
                }
            }
        }

        public Matrix<double> MatrixRotate4DX(double a, Matrix<double> start)
        {
            Matrix<double> rotate = DenseMatrix.OfArray(new double[,] {
                                {1, 0, 0, 0},
                                {0, Math.Cos(a*Math.PI/180), -Math.Sin(a*Math.PI/180), 0},

                                {0, Math.Sin(a*Math.PI/180), Math.Cos(a*Math.PI/180), 0},
                                {0, 0, 0, 1 } });
            return start * rotate;
        }
        public Matrix<double> MatrixRotate4DY(double a, Matrix<double> start)
        {
            Matrix<double> rotate = DenseMatrix.OfArray(new double[,] {
                                {Math.Cos(a*Math.PI/180), 0, Math.Sin(a*Math.PI/180), 0},
                                {0, 1, 0, 0},

                                {-Math.Sin(a*Math.PI/180), 0, Math.Cos(a*Math.PI/180), 0},
                                {0, 0, 0, 1 } });
            return start * rotate;
        }
        public Matrix<double> MatrixRotate4DZ(double a, Matrix<double> start)
        {
            Matrix<double> rotate = DenseMatrix.OfArray(new double[,] {
                                {Math.Cos(a*Math.PI/180), -Math.Sin(a*Math.PI/180), 0, 0},
                                {Math.Sin(a*Math.PI/180), Math.Cos(a*Math.PI/180), 0, 0},

                                {0, 0, 1, 0},
                                {0, 0, 0, 1 } });
            return start * rotate;
        }
        public Matrix<double> MatrixScale4D(double s, Matrix<double> start)
        {
            Matrix<double> scale = DenseMatrix.OfArray(new double[,] {
                                {s, 0, 0, 0},
                                {0, s, 0, 0},
                                {0, 0, s, 0},
                                {0, 0, 0, 1} });
            return start * scale;
        }
        public Matrix<double> MatrixMirror4D(string type, Matrix<double> start)
        {
            if (type == "X")
            {
                Matrix<double> mirror = DenseMatrix.OfArray(new double[,] {
                                {-1, 0, 0, 0},
                                {0, 1, 0, 0},
                                {0, 0, 1, 0},
                                {0, 0, 0, 1} });
                return start * mirror;

            }
            if (type == "Y")
            {
                Matrix<double> mirror = DenseMatrix.OfArray(new double[,] {
                                {1, 0, 0, 0},
                                {0, -1, 0, 0},
                                {0, 0, 1, 0},
                                {0, 0, 0, 1} });
                return start * mirror;

            }
            return null;
        }
        public Matrix<double> MatrixTransfer4D(double dx, double dy, Matrix<double> Points)
        {
            Matrix<double> transfer = DenseMatrix.OfArray(new double[,] {
                                {1, 0, 0, 0},
                                {0, 1, 0, 0},
                                {0, 0, 1, 0},
                                {dx, dy, 0, 1 } });
            return Points * transfer;
        }

        public double[,] PointsToArray(PointCollection col)
        {
            double[,] arr = new double[col.Count, 2];
            for (int i = 0; i < col.Count; i++)
            {
                arr[i, 0] = col[i].X;
                arr[i, 1] = col[i].Y;
            }
            return arr;
        }
        public Matrix<double> PointsToMatrix(PointCollection col)
        {
            double[,] arr = new double[col.Count, 3];
            for (int i = 0; i < col.Count; i++)
            {
                arr[i, 0] = col[i].X;
                arr[i, 1] = col[i].Y;
                arr[i, 2] = 1;
            }
            Matrix<double> start = DenseMatrix.OfArray(arr);

            return start;
        }
        public Matrix<double> PointsToMatrix4D(PointCollection col, List<double> zet)
        {
            double[,] arr = new double[col.Count, 4];
            for (int i = 0; i < col.Count; i++)
            {
                arr[i, 0] = col[i].X;
                arr[i, 1] = col[i].Y;
                arr[i, 2] = zet[i];
                arr[i, 3] = 1;
            }
            Matrix<double> start = DenseMatrix.OfArray(arr);

            return start;
        }
        static Point FindCentroid(PointCollection points)
        {
            double minX = 9999999;
            double minY = 9999999;

            double maxX = -9999999;
            double maxY = -9999999;

            foreach (var item in points)
            {
                if (item.X < minX) minX = item.X;
                if (item.Y < minY) minY = item.Y;

                if (item.X > maxX) maxX = item.X;
                if (item.Y > maxY) maxY = item.Y;

            }

            double centrX = (minX + maxX) / 2;
            double centrY = (minY + maxY) / 2;
            return new Point(centrX, centrY);
            //double[] ans = new double[2];

            //int n = v.GetLength(0);
            //double signedArea = 0;

            //for (int i = 0; i < n; i++)
            //{
            //    double x0 = v[i, 0], y0 = v[i, 1];
            //    double x1 = v[(i + 1) % n, 0],
            //            y1 = v[(i + 1) % n, 1];

            //    double A = (x0 * y1) - (x1 * y0);
            //    signedArea += A;

            //    ans[0] += (x0 + x1) * A;
            //    ans[1] += (y0 + y1) * A;
            //}
            //signedArea *= 0.5;
            //ans[0] = (ans[0]) / (6 * signedArea);
            //ans[1] = (ans[1]) / (6 * signedArea);

           // return ans;
        }
    }
}

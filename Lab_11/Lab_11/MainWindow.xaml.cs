using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_11
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;
        private UIElement currentShape;
        private string currentTool = "Line";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                displayImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void DrawRectanbleButton_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = 150,
                Height = 100,
                Fill = Brushes.Blue,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(rectangle, 100);
            Canvas.SetTop(rectangle, 100);
            shapesCanvas.Children.Add(rectangle);
        }

        private void DrawEllipseButton_Click(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 150,
                Height = 100,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetRight(ellipse, 200);
            Canvas.SetTop(ellipse, 100);
            shapesCanvas.Children.Add(ellipse);
        }

        private void DrawPolygonButton_Click(object sender, RoutedEventArgs e)
        {
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(new Point(10, 10));
            myPointCollection.Add(new Point(110, 10));
            myPointCollection.Add(new Point(-50, -80));

            Polygon polygon = new Polygon
            {
                Points = myPointCollection,
                Width = 150,
                Height = 100,
                Fill = Brushes.Green,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(polygon, 320);
            Canvas.SetTop(polygon, 200);
            shapesCanvas.Children.Add(polygon);
        }

        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(drawingCanvas);
            switch(currentTool)
            {
                case "Линия":
                    currentShape = new Line()
                    {
                        Stroke = Brushes.Black,
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = startPoint.X,
                        Y2 = startPoint.Y
                    };
                    break;
                case "Прямоугольник":
                    currentShape = new Rectangle()
                    {
                        StrokeThickness = 2,
                        Stroke = Brushes.Gray,
                        Fill = Brushes.Red
                    };
                    break;
                case "Эллипс":
                    currentShape = new Ellipse()
                    {
                        Fill = Brushes.Green,
                        StrokeThickness = 2,
                        Stroke = Brushes.Black,
                    };
                    break;
            }
            drawingCanvas.Children.Add(currentShape);
        }

        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentShape = null;
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && currentShape != null)
            {
                Point currentPoint = e.GetPosition(drawingCanvas);
                switch (currentTool)
                {
                    case "Линия":
                        Line line = (Line)currentShape;
                        line.X2 = currentPoint.X;
                        line.Y2 = currentPoint.Y;
                        break;
                    case "Прямоугольник":
                    case "Эллипс":
                        double width = Math.Abs(currentPoint.X - startPoint.X);
                        double height = Math.Abs(currentPoint.Y - startPoint.Y);
                        double left = Math.Min(startPoint.X, currentPoint.X);
                        double top = Math.Min(startPoint.Y, currentPoint.Y);
                        Shape shape = (Shape)currentShape;
                        shape.Width = width;
                        shape.Height = height;
                        Canvas.SetLeft(shape, left);
                        Canvas.SetTop(shape, top);
                        break;
                }
            }
        }

        private void Tool_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                currentTool = radioButton.Content.ToString();
            }
        }
    }
}

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

namespace Lab_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(XnTextBox.Text, out double xn) ||
                !double.TryParse(XkTextBox.Text, out double xk) ||
                !int.TryParse(NTextBox.Text, out int n))
            {
                MessageBox.Show("Invalid input. Please enter valid numbers.");
                return;
            }

            double h = (xk - xn) / n;
            var results = new List<Result>();

            for (double x = xn; x <= xk; x += h)
            {
                double yx = CalculateY(x);
                double sx = CalculateS(x, n);
                results.Add(new Result { X = x, Yx = yx, Sx = sx });
            }

            ResultsDataGrid.ItemsSource = results; // Cвязываем наш список результатов с DataGrid
        }

        private double CalculateY(double x)
        {
            return (1 + 2 * Math.Pow(x, 2)) * Math.Exp(Math.Pow(x, 2));
        }

        private double CalculateS(double x, int n)
        {
            double sum = 1; // Первый член ряда
            double factorial = 1;
            double power = 1;

            for (int k = 1; k <= n; k++)
            {
                factorial *= k;
                power *= x * x;
                sum += (2 * k + 1) * power / factorial;
            }

            return sum;
        }
    }
    public class Result
    {
        public double X { get; set; }
        public double Yx { get; set; }
        public double Sx { get; set; }
    }
}

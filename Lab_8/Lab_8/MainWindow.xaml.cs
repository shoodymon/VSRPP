using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Lab_8
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

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double valueX = double.Parse(xTextBox.Text);
                double valueY = double.Parse(yTextBox.Text);
                double valueZ = double.Parse(zTextBox.Text);
                double valueH = double.Parse(hTextBox.Text);

                CreateCombinedGraph(valueX, valueY, valueZ, valueH);
                GenerateDataTable(valueX, valueY, valueZ, valueH);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CreateCombinedGraph(double x, double y, double z, double h)
        {
            var plotModel = new PlotModel { Title = "Комбинированный график" };

            // График 1: e^(x-y)
            var expSeries = new FunctionSeries(t => Math.Exp(t - y), x, x + 5 * h, h, "e^(x-y)");
            expSeries.Color = OxyColors.Red;
            plotModel.Series.Add(expSeries);

            // График 2: tg^2(z)
            var tanSquaredSeries = new FunctionSeries(t => Math.Pow(Math.Tan(t), 2), z, z + 5 * h, h, "tg^2(z)");
            tanSquaredSeries.Color = OxyColors.Blue;
            plotModel.Series.Add(tanSquaredSeries);

            // График 3: sin(x) + cos(y)
            var sinCosSeries = new FunctionSeries(t => Math.Sin(t) + Math.Cos(y), x, x + 5 * h, h, "sin(x) + cos(y)");
            sinCosSeries.Color = OxyColors.Green;
            plotModel.Series.Add(sinCosSeries);

            // График 4: x^2 + y^2
            var sumOfSquaresSeries = new FunctionSeries(t => Math.Pow(t, 2) + Math.Pow(y, 2), x, x + 5 * h, h, "x^2 + y^2");
            sumOfSquaresSeries.Color = OxyColors.Purple;
            plotModel.Series.Add(sumOfSquaresSeries);

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "y" });

            // Добавляем легенду
            plotModel.Legends.Add(new OxyPlot.Legends.Legend
            {
                LegendPosition = OxyPlot.Legends.LegendPosition.RightTop,
                LegendPlacement = OxyPlot.Legends.LegendPlacement.Inside
            });

            GraphicPlotView.Model = plotModel;
        }

        private void GenerateDataTable(double x, double y, double z, double h)
        {
            var result = new StringBuilder();
            result.AppendLine($"Таблица данных (шаг h = {h}):");
            result.AppendLine($"{"X",-10}{"e^(x-y)",-15}{"tg^2(z)",-15}{"sin(x)+cos(y)",-20}{"x^2+y^2",-15}");

            for (int i = 0; i < 6; i++)  // Генерируем 6 строк данных
            {
                double currentX = x + i * h;
                double currentZ = z + i * h;
                double expValue = Math.Exp(currentX - y);
                double tanSquaredValue = Math.Pow(Math.Tan(currentZ), 2);
                double sinCosValue = Math.Sin(currentX) + Math.Cos(y);
                double sumOfSquaresValue = Math.Pow(currentX, 2) + Math.Pow(y, 2);
                
                result.AppendLine($"{currentX,-10:F2}{expValue,-15:F4}{tanSquaredValue,-15:F4}{sinCosValue,-20:F4}{sumOfSquaresValue,-15:F2}");
            }

            ResultTextBox.Text = result.ToString();
        }
    }
}

using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
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
using Wpf.Ui.Controls;


namespace Lab_14
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <sumary>
        /// Метод, который объединяет парсинг всех текстовых полей, возвращая значения через out параметры. 
        /// Если парсинг не удается, выводится сообщение об ошибке.
        /// </sumary>
        private bool TryParseInputs(out double x, out double y, out double z, out double h)
        {
            x = y = z = h = 0;
            try
            {
                x = double.Parse(xTextBox.Text);
                y = double.Parse(yTextBox.Text);
                z = double.Parse(zTextBox.Text);
                h = double.Parse(hTextBox.Text);
                return true;
            }
            catch (FormatException)
            {
                System.Windows.MessageBox.Show("Ошибка: Введите корректные числовые значения.", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        /// <sumary>
        /// Вспомогательный метод для обработки ввода и передачи значений в действия, 
        /// такие как построение графика или генерация таблицы.
        /// </sumary>
        private void HandleResultButtonClick(Action<double, double, double, double> action)
        {
            if (TryParseInputs(out double valueX, out double valueY, out double valueZ, out double valueH))
            {
                action(valueX, valueY, valueZ, valueH);     // Вызовет CreateCombinedGraph (делегат ссылается на метод)
            }
        }

        private void PlotGraphButton_Click(object sender, RoutedEventArgs e)
        {
            HandleResultButtonClick(CreateCombinedGraph);
        }

        private void GenerateTableButton_Click(object sender, RoutedEventArgs e)
        {
            HandleResultButtonClick(GenerateDataTable);
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            HandleResultButtonClick((x, y, z, h) =>
            {
                CreateCombinedGraph(x, y, z, h);
                GenerateDataTable(x, y, z, h);
            });
        }

        private PlotModel CreatePlotModel(double x, double y, double z, double h)
        {
            var plotModel = new PlotModel
            {
                Title = "Комбинированный график",
                Legends =
                {
                    new OxyPlot.Legends.Legend
                    {
                        LegendPosition = OxyPlot.Legends.LegendPosition.RightTop,
                        LegendPlacement = OxyPlot.Legends.LegendPlacement.Inside
                    }
                },
                Axes =
                {
                    new LinearAxis { Position = AxisPosition.Bottom, Title = "x" },
                    new LinearAxis { Position = AxisPosition.Left, Title = "y" }
                }
            };

            var expSeries = new FunctionSeries(t => Math.Exp(t - y), x, x + 5 * h, h, "e^(x-y)");
            expSeries.Color = OxyColors.Red;
            plotModel.Series.Add(expSeries);

            var tanSquaredSeries = new FunctionSeries(t => Math.Pow(Math.Tan(t), 2), z, z + 5 * h, h, "tg^2(z)");
            tanSquaredSeries.Color = OxyColors.Blue;
            plotModel.Series.Add(tanSquaredSeries);

            var sinCosSeries = new FunctionSeries(t => Math.Sin(t) + Math.Cos(y), x, x + 5 * h, h, "sin(x) + cos(y)");
            sinCosSeries.Color = OxyColors.Green;
            plotModel.Series.Add(sinCosSeries);

            var sumOfSquaresSeries = new FunctionSeries(t => Math.Pow(t, 2) + Math.Pow(y, 2), x, x + 5 * h, h, "x^2 + y^2");
            sumOfSquaresSeries.Color = OxyColors.Purple;
            plotModel.Series.Add(sumOfSquaresSeries);

            return plotModel;
        }

        /// <summary>
        /// Один и тот же экземпляр PlotModel не может одновременно использоваться
        /// в нескольких элементах управления PlotView (клонировать нельзя)
        /// GraphicPlotView.Model = plotModel;
        /// GraphicPlotView2.Model = plotModel;
        /// Создание двух независимых PlotModel также привело к ошибке
        /// plotModel1.Series.Add(tanSquaredSeries);
        /// plotModel2.Series.Add(tanSquaredSeries);
        /// GraphicPlotView.Model = plotModel1;
        /// GraphicPlotView2.Model = plotModel2;
        /// System.InvalidOperationException: "The element cannot be added, it already belongs to a PlotModel."
        /// Решение - создать два независимых графика, но с одинаковым содержимым
        /// </summary>

        private void CreateCombinedGraph(double x, double y, double z, double h)
        {
            GraphicPlotView.Model = CreatePlotModel(x, y, z, h);
            GraphicPlotView2.Model = CreatePlotModel(x, y, z, h);
        }

        private void GenerateDataTable(double x, double y, double z, double h)
        {
            var result = new StringBuilder();
            result.AppendLine($"Таблица данных (шаг h = {h}):");
            result.AppendLine($"{"X",-10}{"e^(x-y)",-15}{"tg^2(z)",-15}{"sin(x)+cos(y)",-20}{"x^2+y^2",-15}");

            for (int i = 0; i < 6; i++)
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
            TableTextBox.Text = result.ToString();
        }

        /// <summary>
        /// Методы очистки теперь могут использоваться во всех подходящих сценариях без дублирования.
        /// </summary>
        private void ClearGraph()
        {
            GraphicPlotView.Model = new PlotModel();
            GraphicPlotView2.Model = new PlotModel();
        }

        private void ClearTable()
        {
            ResultTextBox.Clear();
            TableTextBox.Clear();
        }

        private void ClearInputs()
        {
            xTextBox.Clear();
            yTextBox.Clear();
            zTextBox.Clear();
            hTextBox.Clear();
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            ClearGraph();
            ClearTable();
            ClearInputs();
        }

        private void ClearGraphButton_Click(object sender, RoutedEventArgs e)
        {
            ClearGraph();
        }

        private void ClearTableButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTable();
        }

        private void ClearTextBoxButton_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
        }
    }
}
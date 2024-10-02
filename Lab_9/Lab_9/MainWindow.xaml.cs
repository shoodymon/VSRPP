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

namespace Lab_9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate double MathFunction(double x);

        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBoxItems();
        }

        private void InitializeComboBoxItems()
        {
            FunctionComboBox.Items.Add("(1 + 2x^2) * e^(x^2)");
            FunctionComboBox.Items.Add("sin(x) + cos(x)");
            FunctionComboBox.Items.Add("ln(x+1) + sqrt(x)");
            FunctionComboBox.Items.Add("sqrt(x) + sin(x)");
            FunctionComboBox.Items.Add("x^3 * e^2");
            FunctionComboBox.Items.Add("4 * pi * x^2");
            FunctionComboBox.SelectedIndex = 0;
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

            MathFunction selectedFunction = GetSelectedFunction();
            double h = (xk - xn) / n;
            var results = new List<Result>();
            StepTextBlock.Text = h.ToString();

            for (double x = xn; x <= xk; x += h)
            {
                double yx = selectedFunction(x);
                double sx = CalculateS(x, n, selectedFunction);
                results.Add(new Result { X = x, Yx = yx, Sx = sx });
            }

            ResultsDataGrid.ItemsSource = results;
        }

        private MathFunction GetSelectedFunction()
        {
            switch (FunctionComboBox.SelectedIndex)
            {
                case 0: return Functions.Function1;
                case 1: return Functions.Function2;
                case 2: return Functions.Function3;
                case 3: return Functions.Function4;
                case 4: return Functions.Function5;
                case 5: return Functions.Function6;
                default: return Functions.DefaultFunction;
            }
        }

        private double CalculateS(double x, int n, MathFunction function)
        {
            double sum = function(0);
            double factorial = 1;
            double power = 1;
            for (int k = 1; k <= n; k++)
            {
                factorial *= k;
                power *= x * x;
                sum += function(k) * power / factorial;
            }
            return sum;
        }

        private void OpenColorMixerModal_Click(object sender, RoutedEventArgs e)
        {
            var colorMixer = new ColorMixerWindow();
            colorMixer.Owner = this; // Устанавливаем владельца для модального окна
            colorMixer.ColorSelected += ColorMixer_ColorSelected; // Подписываемся на событие

            if (colorMixer.ShowDialog() == true) // Блокирует выполнение, пока окно открыто
            {
                ApplySelectedColor(colorMixer.SelectedColor);
            }

            colorMixer.ColorSelected -= ColorMixer_ColorSelected; // Отписываемся от события
        }

        private void OpenColorMixerNonModal_Click(object sender, RoutedEventArgs e)
        {
            var colorMixer = new ColorMixerWindow();
            colorMixer.ColorSelected += ColorMixer_ColorSelected; // Подписываемся на событие
            colorMixer.Closed += (s, args) => colorMixer.ColorSelected -= ColorMixer_ColorSelected; // Отписываемся при закрытии
            colorMixer.Show(); // Не блокирует выполнение
        }

        private void ColorMixer_ColorSelected(object sender, Color color)
        {
            ApplySelectedColor(color);
        }

        private void ApplySelectedColor(Color color)
        {
            ResultsDataGrid.Background = new SolidColorBrush(color);
        }
    }

    public class Result
    {
        public double X { get; set; }
        public double Yx { get; set; }
        public double Sx { get; set; }
    }
}

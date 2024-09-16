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

namespace Lab_2
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

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double valueX = double.Parse(blockValueX.Text);
                double valueY = double.Parse(blockValueY.Text);
                double valueZ = double.Parse(blockValueZ.Text);

                double result = calculateResult(valueX, valueY);
                blockResult.Text = $"\n\tЛабораторная работа №2, ст.гр. 10702423\n\tЖивоглод Никита, Павлоский Никита\n" +
                                   $"\n\tX = {valueX}\n" +
                                   $"\n\tY = {valueY}\n" +
                                   $"\n\tZ = {valueZ}\n" +
                                   $"\n\tРезультат = {result}\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private double calculateResult(double valueX, double valueY)
        {
            double result = 0;
            double funcValue = calculateFunction(valueX);

            if (valueX > 0) 
            {
                if (valueY % 2 == 1)
                {
                    result = valueY * Math.Sqrt(funcValue);
                }
            }
            else if (valueX < 0)
            {
                if (valueY % 2 == 0)
                {
                    result = valueY / 2 * Math.Sqrt(Math.Abs(funcValue));
                }
            }
            else { result = Math.Sqrt(Math.Abs(valueY) * Math.Abs(funcValue));  }
            return result;
        }

        private double calculateFunction(double x)
        {
            if (buttonSH.IsChecked == true)
            {
                return Math.Sinh(x);
            }
            else if (buttonSquare.IsChecked == true)
            {
                return Math.Pow(x, 2);
            }
            else if (buttonExp.IsChecked == true)
            {
                return Math.Exp(x);
            }
            else
            {
                throw new InvalidOperationException("Функция не выбрана");
            }
        }
    }
}

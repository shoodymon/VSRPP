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

namespace Lab_1
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

                double result = Math.Log(Math.Pow(valueY, -Math.Sqrt(Math.Abs(valueX)))) * (valueX - (valueY/2)) + Math.Pow(Math.Sin(Math.Atan(valueZ)), 2);
                blockResult.Text += "\n\tЛабораторная работа №1, ст.гр. 10702423\n\tЖивоглод Никита, Павлоский Никита\n";
                blockResult.Text += "\n\tX = " + valueX.ToString() + '\n';
                blockResult.Text += "\n\tY = " + valueY.ToString() + '\n';
                blockResult.Text += "\n\tZ = " + valueZ.ToString() + '\n';
                blockResult.Text += "\n\tРезультат = " + result.ToString() + '\n';
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            
        }
    }
}

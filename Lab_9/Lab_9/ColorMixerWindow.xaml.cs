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
using System.Windows.Shapes;

namespace Lab_9
{
    /// <summary>
    /// Логика взаимодействия для ColorMixerWindow.xaml
    /// </summary>
    public partial class ColorMixerWindow : Window
    {
        public Color SelectedColor { get; private set; }

        // Добавляем событие для уведомления о выбранном цвете
        public event EventHandler<Color> ColorSelected;

        public ColorMixerWindow()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedEventArgs e)
        {
            UpdateColorPreview();
        }

        private void UpdateColorPreview()
        {
            byte r = (byte)RedSlider.Value;
            byte g = (byte)GreenSlider.Value;
            byte b = (byte)BlueSlider.Value;
            SelectedColor = Color.FromRgb(r, g, b);
            ColorPreview.Fill = new SolidColorBrush(SelectedColor);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Вызываем событие с выбранным цветом
            ColorSelected?.Invoke(this, SelectedColor);

            // Если окно открыто как модальное, устанавливаем DialogResult
            if (Owner != null)
            {
                DialogResult = true;
            }

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

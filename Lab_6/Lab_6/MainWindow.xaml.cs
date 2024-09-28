using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using static Lab_6.MainWindow;

namespace Lab_6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class PhoneCall
        {
            public DateTime Date { get; set; }
            public string CityCode { get; set; }
            public string CityName { get; set; }
            public int Duration { get; set; }
            public decimal Rate { get; set; }
            public string CityPhoneNumber { get; set; }
            public string CallerPhoneNumber { get; set; }
        }
      
        private List<PhoneCall> phoneCallList = new List<PhoneCall>();

        /// <summary>
        /// PlaceholderText - подсказка для пользователя
        /// Отображает подсказывающий текст внутри пустого текстового поля, указывая, какую информацию следует ввести.
        /// Здесь реализовано собственное поведение PlaceholderText с помощью обработчиков событий GotFocus и LostFocus.
        /// </summary>
        private void SetUpPlaceholder(TextBox textBox, string placeholderText)
        {
            textBox.Text = placeholderText;
            textBox.Foreground = Brushes.Gray;

            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.Foreground = Brushes.Black;
                }
            };

            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text += placeholderText;
                    textBox.Foreground = Brushes.Gray;
                }
            };
        }

        private void InitializePlaceholders()
        {
            SetUpPlaceholder(CityCodeTextBox, "Код города");
            SetUpPlaceholder(CityNameTextBox, "Название города");
            SetUpPlaceholder(DurationTextBox, "Длительность (мин)");
            SetUpPlaceholder(RateTextBox, "Тариф");
            SetUpPlaceholder(CityPhoneTextBox, "Номер телефона в городе");
            SetUpPlaceholder(CallerPhoneTextBox, "Номер абонента");
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializePlaceholders();
        }

        private void ToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("На междугородной АТС информация о разговорах содержит дату разговора, код и название города, время разговора, тариф, номер телефона в этом городе и номер телефона абонента. Вывести по каждому городу общее время разговоров с ним и сумму.",
                "Задание", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var phoneCall = new PhoneCall
            {
                Date = DatePicker.SelectedDate ?? DateTime.Now,
                CityCode = CityCodeTextBox.Text == "Код города" ? "" : CityCodeTextBox.Text,
                CityName = CityNameTextBox.Text == "Название города" ? "" : CityNameTextBox.Text,
                Duration = int.TryParse(DurationTextBox.Text, out int duration) ? duration : 0,
                Rate = decimal.TryParse(RateTextBox.Text, out decimal rate) ? rate : 0,
                CityPhoneNumber = CityPhoneTextBox.Text == "Номер телефона в городе" ? "" : CityPhoneTextBox.Text,
                CallerPhoneNumber = CallerPhoneTextBox.Text == "Номер абонента" ? "" : CallerPhoneTextBox.Text,
            };

            phoneCallList.Add(phoneCall);
            UpdateResultsDisplay();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveToFile(saveFileDialog.FileName);
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadFromFile(openFileDialog.FileName);
            }
        }

        private void SaveToFile(string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var call in phoneCallList)
                {
                    writer.WriteLine($"{call.Date},{call.CityCode},{call.CityName},{call.Duration},{call.Rate},{call.CityPhoneNumber},{call.CallerPhoneNumber}");
                }
            }
        }

        private void LoadFromFile(string filename)
        {
            phoneCallList.Clear();
            using (var reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    var call = new PhoneCall
                    {
                        Date = DateTime.Parse(parts[0]),
                        CityCode = parts[1],
                        CityName = parts[2],
                        Duration = int.Parse(parts[3]),
                        Rate = decimal.Parse(parts[4]),
                        CityPhoneNumber = parts[5],
                        CallerPhoneNumber = parts[6],
                    };
                    phoneCallList.Add(call);
                }
            }
            UpdateResultsDisplay();
        }

        private void UpdateResultsDisplay()
        {
            var results = phoneCallList
                .GroupBy(c => c.CityName)
                .Select(g => new
                {
                    CityName = g.Key,
                    TotalDuration = g.Sum(c => c.Duration),
                    TotalCost = g.Sum(c => c.Duration * c.Rate / 60)
                })
                .ToList();

            ResultsTextBox.Text = string.Join("\n", results.Select(r => $"{r.CityName}: Общее время: {r.TotalDuration} мин, Сумма: {r.TotalCost:C2}"));

            File.WriteAllText("results.txt", ResultsTextBox.Text);
        }
    }
}

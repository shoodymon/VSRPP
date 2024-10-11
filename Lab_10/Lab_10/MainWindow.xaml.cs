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
using System.IO;
using IniFiles;

namespace Lab_10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> groups = new List<string>();
        private INIFile iniFile;
        private const string IniFileName = "settings.ini";

        public MainWindow()
        {
            InitializeComponent();
            iniFile = new INIFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IniFileName));
            LoadSettings();
        }

        // Метод отвечает за загрузку настроек из INI-файла при запуске приложения
        private void LoadSettings()
        {
            // Читаем значение "GroupCount" из секции "Settings" INI-файла
            string groupCountString = iniFile.Read("Settings", "GroupCount");
            int groupCount;
            if (!int.TryParse(groupCountString, out groupCount))
            {
                groupCount = 0; // Значение по умолчанию, если парсинг не удался
            }

            for (int i = 0; i < groupCount; i++)
            {
                // Для каждой группы читаем её значение из секции "Groups" INI-файла
                string group = iniFile.Read("Groups", $"Group{i}");
                if (!string.IsNullOrEmpty(group))
                {
                    groups.Add(group);
                }
            }
            UpdateListBox();
        }

        // Метод отвечает за сохранение текущих настроек в INI-файл
        private void SaveSettings()
        {
            // Записываем количество групп в секцию "Settings" INI-файла
            iniFile.Write("Settings", "GroupCount", groups.Count.ToString());
            for (int i = 0; i < groups.Count; i++)
            {
                // Для каждой группы записываем её значение в секцию "Groups" INI-файла
                iniFile.Write("Groups", $"Group{i}", groups[i]);
            }
        }

        private void ToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дана строка, состоящая из групп нулей и единиц. Подсчитать количество единиц в группах с нечетным количеством символов.", "Задание", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            groups.Clear();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                int length = random.Next(1, 30);
                string group = new string(Enumerable.Repeat("01", length).Select(s => s[random.Next(s.Length)]).ToArray());
                groups.Add(group);
            }
            UpdateListBox();
            SaveSettings();
        }

        // Метод позволяет пользователю вручную сохранить текущие группы в отдельный INI-файл по выбору
        private void buttonSaveGroups_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "INI файлы (*.ini)|*.ini",
                DefaultExt = "ini",
                AddExtension = true,
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                INIFile customIniFile = new INIFile(saveFileDialog.FileName);
                for (int i = 0; i < groups.Count; i++)
                {
                    customIniFile.Write("Группы", $"Группа {i}", groups[i]);
                }
                customIniFile.Write("Настройки", "Количество групп", groups.Count.ToString());
                MessageBox.Show("Группы успешно сохранены в INI файл.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ListBoxGroups_GroupSelect(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxGroups.SelectedItem != null)
            {
                string selectedGroup = listBoxGroups.SelectedItem.ToString();
                if (selectedGroup.Length % 2 == 0)
                {
                    labelLongestGroup.Text = "Выбрана группа с четной длиной. Подсчет единиц не производится.";
                }
                else
                {
                    int onesCount = CountOnesInOddLengthGroup(selectedGroup);
                    labelLongestGroup.Text = $"Количество единиц в выбранной группе с нечетной длиной: {onesCount}";
                }
            }
        }

        private void UpdateListBox()
        {
            listBoxGroups.ItemsSource = null;
            listBoxGroups.ItemsSource = groups;
        }

        private int CountOnesInOddLengthGroup(string group)
        {
            return group.Count(c => c == '1');
        }

        /// <summary>
        /// Метод играет важную роль в жизненном цикле окна.
        /// Здесь мы переопределяем метод базового класса Window.
        /// Таким образом, мы полностью контролируем сохранение данных даже при неожиданном закрытии.
        /// </summary>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
            base.OnClosing(e);
        }
    }
}

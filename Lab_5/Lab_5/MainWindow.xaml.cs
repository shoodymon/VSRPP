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

namespace Lab_5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> groups = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
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
                // Генерирование случайной строки из нулей и единиц заданной длины
                string group = new string(Enumerable.Repeat("01", length).Select(s => s[random.Next(s.Length)]).ToArray());
                groups.Add(group); 
            }
            UpdateListBox();
        }

        private void buttonSaveGroups_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                DefaultExt = "txt",
                AddExtension = true,
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllLines(saveFileDialog.FileName, groups);
                MessageBox.Show("Группы успешно сохранены.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ListBoxGroups_GroupSelect(object sender, RoutedEventArgs e) 
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
    }
}

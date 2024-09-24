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

namespace Lab_4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] matrix;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowsTextBox.Text, out int rows) && int.TryParse(ColumnsTextBox.Text, out int columns))
            {
                matrix = new int[rows, columns];
                var random = new Random();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        matrix[i, j] = random.Next(1, 100);
                    }
                }

                UpdateDataGrid();
                ClearHighlighting();
            }
            else
            {
                MessageBox.Show("Укажите корректное число строк и столбцов.");
            }
        }

        private void UpdateDataGrid()
        {
            MatrixGrid.Columns.Clear();
            MatrixGrid.ItemsSource = null;

            // Добавляем столбец для номеров строк
            var rowNumberColumn = new DataGridTextColumn
            {
                Header = "Матрица NxM",
                Binding = new Binding("RowNumber"),
                IsReadOnly = true
            };
            MatrixGrid.Columns.Add(rowNumberColumn);

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                var column = new DataGridTextColumn
                {
                    Header = $"Столбец {j + 1}",
                    Binding = new Binding($"Values[{j}]")
                };
                MatrixGrid.Columns.Add(column);
            }

            var data = new List<MatrixRow>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var row = new MatrixRow { RowNumber = $"Строка {i + 1}", Values = new List<int>() };
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row.Values.Add(matrix[i, j]);
                }
                data.Add(row);
            }

            MatrixGrid.ItemsSource = data;
        }

        private void AnalyzeMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Сгенерируйте матрицу для начала.");
                return;
            }

            ClearHighlighting();
            int specialElements = CountAndHighlightSpecialElements(matrix);
            ResultTextBlock.Text = $"Количество \"особых\" элементов матрицы: {specialElements}";
        }

        private int CountAndHighlightSpecialElements(int[,] matrix)
        {
            int count = 0;
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (IsSpecialElement(matrix, i, j))
                    {
                        count++;
                        HighlightCell(i, j);
                    }
                }
            }

            return count;
        }

        private bool IsSpecialElement(int[,] matrix, int row, int col)
        {
            int value = matrix[row, col];
            int cols = matrix.GetLength(1);

            // Проверяем элементы слева
            for (int j = 0; j < col; j++)
            {
                if (matrix[row, j] >= value)
                {
                    return false;
                }
            }

            // Проверяем элементы справа
            for (int j = col + 1; j < cols; j++)
            {
                if (matrix[row, j] <= value)
                {
                    return false;
                }
            }

            return true;
        }

        private void HighlightCell(int row, int col)
        {
            if (MatrixGrid.ItemContainerGenerator.ContainerFromIndex(row) is DataGridRow dataGridRow)
            {
                // Учитываем, что первый столбец теперь номер строки
                if (MatrixGrid.Columns[col + 1].GetCellContent(dataGridRow)?.Parent is DataGridCell cell)
                {
                    cell.Tag = "Special";
                }
            }
        }

        private void ClearHighlighting()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (MatrixGrid.ItemContainerGenerator.ContainerFromIndex(i) is DataGridRow dataGridRow)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        // Учитываем, что первый столбец теперь номер строки
                        if (MatrixGrid.Columns[j + 1].GetCellContent(dataGridRow)?.Parent is DataGridCell cell)
                        {
                            cell.Tag = null;
                        }
                    }
                }
            }
        }
    }

    public class MatrixRow
    {
        public string RowNumber { get; set; }
        public List<int> Values { get; set; }
    }
}

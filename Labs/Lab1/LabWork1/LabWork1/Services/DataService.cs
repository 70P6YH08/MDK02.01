using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;

namespace LabWork1.Services
{
    public class DataService
    {
        public static string excelFilePath;
        public static DataView ImportData(DataView dataView)
        {
            DataTable dataTable = new();
            OpenFileDialog openFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() != true)
                return new DataView();

            excelFilePath = openFileDialog.FileName;

            if (!File.Exists(excelFilePath))
            {
                MessageBox.Show("Файл не найден!",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return new DataView();
            }

            try
            {
                dataTable.Rows.Clear();
                dataTable.Columns.Clear();

                int maxColumns = 0;

                var csvFile = File.ReadAllLines(excelFilePath);

                foreach (var line in csvFile)
                {
                    if (String.IsNullOrEmpty(line))
                        continue;

                    var columns = line.Split(";").Length;
                    if (columns > maxColumns)
                        maxColumns = columns;
                }

                for (int i = 0; i < maxColumns; i++)
                    dataTable.Columns.Add($"Column {i}");

                foreach (var line in csvFile)
                {
                    if (String.IsNullOrWhiteSpace(line))
                        continue;

                    var data = line.Split(";");
                    var newRow = dataTable.NewRow();

                    for (int i = 0; i < data.Length && i < maxColumns; i++)
                        newRow[i] = data[i];

                    dataTable.Rows.Add(newRow);
                }
                dataView = dataTable.DefaultView;

                MessageBox.Show("Импорт выполнен успешно",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return dataView;
        }

        public static void ExportData(DataView dataView)
        {
            if (dataView == null || dataView.Table == null)
            {
                MessageBox.Show("Отсутсвуют данные для экспорта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = $"export_{Path.GetFileName(excelFilePath)}",
                DefaultExt = ".csv"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            excelFilePath = saveFileDialog.FileName;

            try
            {
                var countColumns = dataView.Table.Columns.Count;
                List<string> exportData = new();
                foreach (DataRowView line in dataView)
                {
                    var row = line.Row;

                    string exportRow = "";
                    for (int i = 0; i < countColumns; i++)
                    {
                        exportRow += row[i];
                        if (i != countColumns - 1)
                            exportRow += ";";
                    }
                    exportData.Add(exportRow);
                }
                File.WriteAllLines(excelFilePath, exportData);
                MessageBox.Show("Экспорт выполнен успешно",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}

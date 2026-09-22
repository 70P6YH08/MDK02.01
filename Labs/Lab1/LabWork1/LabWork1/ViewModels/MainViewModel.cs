using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;

namespace LabWork1.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private DataView _dataView = new();

        private string? _excelFilePath;

        [RelayCommand]
        private void ImportExcelData()
        {
            DataTable dataTable = new();
            OpenFileDialog openFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            _excelFilePath = openFileDialog.FileName;

            if (!File.Exists(_excelFilePath))
            {
                MessageBox.Show("Файл не найден!",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            try
            {
                dataTable.Rows.Clear();
                dataTable.Columns.Clear();
                int maxColumns = 0;
                var csvFile = File.ReadAllLines(_excelFilePath);
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
                DataView = dataTable.DefaultView;

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
        }

        [RelayCommand]
        private void ExportExcelData()
        {
            if (DataView == null || DataView.Table == null)
            {
                MessageBox.Show("Отсутсвуют данные для экспорта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = $"export_{Path.GetFileName(_excelFilePath)}",
                DefaultExt = ".csv"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            _excelFilePath = saveFileDialog.FileName;

            try
            {
                var countColumns = DataView.Table.Columns.Count;
                List<string> exportData = new();
                foreach (DataRowView line in DataView)
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
                File.WriteAllLines(_excelFilePath, exportData);
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

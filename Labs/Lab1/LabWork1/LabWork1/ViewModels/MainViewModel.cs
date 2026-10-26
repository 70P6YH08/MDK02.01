using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabWork1.Services;
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


        [RelayCommand]
        private void ExportExcelData()
        {
            DataService.ExportData(DataView);
        }

        [RelayCommand]
        private void ImportExcelData()
        {
            DataView = DataService.ImportData(DataView);
        }
    }
}

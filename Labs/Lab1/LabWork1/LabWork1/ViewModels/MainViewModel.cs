using CommunityToolkit.Mvvm.ComponentModel;
using LabWork1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace LabWork1.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<User> _users = new();

        private string filePath = @"C:\temp\ispp31\MDK02.02\Labs\Lab1\users.csv";

        public MainViewModel()
        {
            ReadExcelFile(filePath);
        }


        private void ReadExcelFile(string excelFilePath)
        {
            if (!File.Exists(excelFilePath))
            {
                MessageBox.Show("Файл не найден!",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            try
            {
                var csvFile = File.ReadAllLines(filePath);
                foreach (var line in csvFile)
                {
                    if (line == null)
                        return;

                    var userData = line.Split(";");
                    User user = new()
                    {
                        Id = Convert.ToInt32(userData[0] + 1),
                        Login = userData[1],
                        Password = userData[2],
                        Email = userData[3],
                    };
                    Users.Add(user);
                }
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

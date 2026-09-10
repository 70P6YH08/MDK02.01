using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabWork1.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LabWork1.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _login; 
        [ObservableProperty]
        private string? _password;

        private string filePath = @"C:\temp\ispp31\MDK02.02\Labs\Lab1\users.csv";

        [RelayCommand]
        private void ToAuthorizationWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<RegistrationWindow>().SingleOrDefault(w => w.IsActive);
            AuthorizationWindow authorizationWindow = new();
            authorizationWindow.Show();
            if (currentWindow != null)
                currentWindow.Close();
        }

        [RelayCommand]
        private void RegistrationUser(object parameter)
        {
            try
            {
                var csvFile = File.ReadAllLines(filePath);
                foreach (var line in csvFile)
                {
                    if (line == null)
                        return;

                    var userData = line.Split(";");
                    var userLogin = userData[0];

                    if (String.IsNullOrEmpty(Login))
                    {
                        MessageBox.Show("Логин не может быть пустым!",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        return;
                    }
                    else if (userLogin != Login)
                    {
                        MessageBox.Show("Такого пользователя нет в системе!",
                           "Предупреждение",
                           MessageBoxButton.OK,
                           MessageBoxImage.Warning);
                        return;
                    }
                    else if (userLogin == Login)
                    {
                        if (parameter is PasswordBox passwordBox)
                            Password = passwordBox.Password;

                        var userPassword = userData[1];

                        if (userPassword == Password)
                        {
                            MessageBox.Show("Вы успешно вошли в систему!",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            Login = Password = null;
                            MainWindow mainWindow = new();
                            mainWindow.ShowDialog();
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Неправильный пароль",
                               "Предупреждение",
                               MessageBoxButton.OK,
                               MessageBoxImage.Warning);
                            return;
                        }
                    }
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

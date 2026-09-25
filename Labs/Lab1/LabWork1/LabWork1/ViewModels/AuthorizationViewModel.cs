using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabWork1.DTOs;
using LabWork1.Services;
using LabWork1.Views;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace LabWork1.ViewModels
{
    public partial class AuthorizationViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _login;

        AuthorizationWindow? currentWindow = Application.Current.Windows
                .OfType<AuthorizationWindow>()
                .SingleOrDefault(w => w.IsActive);

        [RelayCommand]
        private void ToRegistrationWindow()
        {
            RegistrationWindow registrationWindow = new();
            registrationWindow.Show();
        }

        [RelayCommand]
        private void AuthorizationUser(object parametrPasswordBox)
        {

            if (String.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Логин не может быть пустым!",
                        "Предупреждение",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                return;
            }

            IntPtr bstrPasswordString = IntPtr.Zero;
            try
            {
                var csvFile = File.ReadAllLines(DataValidator.filePath);
                foreach (var line in csvFile)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        MessageBox.Show("Негде искать!",
                           "Ошибка",
                           MessageBoxButton.OK,
                           MessageBoxImage.Warning);
                        return;
                    }

                    var userData = line.Split(";");
                    var userLogin = userData[2];

                    if (userLogin == Login)
                    {
                        if (parametrPasswordBox is PasswordBox passwordBox)
                        {
                            var securePassword = passwordBox.SecurePassword;
                            var userPasswordHash = userData[3];

                            if (!DataValidator.IsRightPassword(securePassword, userPasswordHash))
                            {
                                MessageBox.Show("Неправильный пароль",
                                   "Предупреждение",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Warning);
                                return;
                            }

                            MessageBox.Show("Вы успешно вошли в систему!",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                            UserDto userDto = new()
                            {
                                Id = Convert.ToInt32(userData[0]),
                                Login = userData[1],
                                Email = userData[2],
                            };


                            if (currentWindow != null)
                                currentWindow.Close();
                            MainWindow mainWindow = new();
                            mainWindow.Show();

                            return;
                        }
                    }
                }
                MessageBox.Show("Такого пользователя нет в системе!",
                       "Предупреждение",
                       MessageBoxButton.OK,
                       MessageBoxImage.Warning);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            finally
            {
                if (bstrPasswordString != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstrPasswordString);
            }
        }
    }
}

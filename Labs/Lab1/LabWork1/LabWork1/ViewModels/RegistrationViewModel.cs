using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabWork1.Models;
using LabWork1.Services;
using LabWork1.Views;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace LabWork1.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _login;
        [ObservableProperty]
        private string? _email;

        private SecureString? _password;
        private SecureString? _returnPassword;

        RegistrationWindow? currentWindow = Application.Current.Windows
                .OfType<RegistrationWindow>()
                .SingleOrDefault(w => w.IsActive);

        [RelayCommand]
        private void ToAuthorizationWindow()
        {
            AuthorizationWindow authorizationWindow = new();
            authorizationWindow.Show();
            if (currentWindow != null)
                currentWindow.Close();
        }

        [RelayCommand]
        private void RegistrationUser(object parameter)
        {
            if (parameter is PasswordBox[] passwordboxes)
            {
                _password = passwordboxes[0].SecurePassword;
                _returnPassword = passwordboxes[1].SecurePassword;
            }

            if (String.IsNullOrEmpty(Login) ||
                _password is null ||
                _returnPassword is null ||
                String.IsNullOrEmpty(Email)
                )
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string errorMessage;

            if (!DataValidator.IsValidLogin(Login))
            {
                errorMessage = DataValidator.GetMessageErrorForLogin(Login);
                MessageBox.Show(errorMessage, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DataValidator.IsValidPassword(_password))
            {
                errorMessage = DataValidator.GetMessageErrorForPassword(_password, _returnPassword);
                MessageBox.Show(errorMessage, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DataValidator.IsEqualsPasswords(_password, _returnPassword))
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DataValidator.IsValidMail(Email))
            {
                errorMessage = DataValidator.GetMessageErrorForMail(Email);
                MessageBox.Show(errorMessage, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using(FileStream fileStream = new(DataValidator.filePath, FileMode.Open))
                {
                    using (StreamReader streamReader = new(fileStream))
                    {
                        string? line;
                        int lineCount = 0;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            lineCount++;
                            var userData = line.Split(';');
                            var userLogin = userData[1];

                            if (userLogin == Login)
                            {
                                MessageBox.Show("Такой пользователь уже существует!",
                                    "Предупреждение",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                                return;
                            }

                            var userEmail = userData[3];
                            if (userEmail == Email)
                            {
                                MessageBox.Show("Такой пользователь уже существует!",
                                    "Предупреждение",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                                return;
                            }
                        }
                        using(StreamWriter streamWriter = new(fileStream))
                        {
                            streamWriter.WriteLine($"{lineCount + 1};" +
                                $"{Login};" +
                                $"{DataValidator.GetHashPassword(_password)};" +
                                $"{Email}");
                        }

                        MessageBox.Show("Вы успешно зарегистрировались!",
                            "Сообщение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);


                        if (currentWindow != null)
                            currentWindow.Close();
                        MainWindow mainWindow = new();
                        mainWindow.Show();

                        return;
                    }
                }
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }
}

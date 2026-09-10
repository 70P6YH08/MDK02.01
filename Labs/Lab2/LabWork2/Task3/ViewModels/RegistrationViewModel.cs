using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using Task3.Services;

namespace LabWork2.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _returnPassword;

        [ObservableProperty]
        private string? _email;

        [RelayCommand]
        private void RegistrationUser(object parameter)
        {
            try
            {

                if (String.IsNullOrEmpty(Login))
                {
                    MessageBox.Show("Логин не может быть пустым!",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    return;
                }

                else if (DataValidator.IsValidLogin(Login))
                {
                    if (parameter is PasswordBox passwordBox)
                        Password = passwordBox.Password;

                    if (String.IsNullOrEmpty(Password))
                    {
                        MessageBox.Show("Пароль не может быть пустым!",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        return;
                    }

                    if (DataValidator.IsValidPassword(Password))
                    {
                        MessageBox.Show("Вы успешно вошли в систему!",
                            "Уведомление",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        Login = Password = null;
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

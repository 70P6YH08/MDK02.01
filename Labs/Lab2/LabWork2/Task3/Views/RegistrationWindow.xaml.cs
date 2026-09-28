using Task3.ViewModels;
using System.Windows;
using Task3.Services;

namespace Task3.Views
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            DataContext = new RegistrationViewModel();
            InitializeComponent();
        }
    }
}

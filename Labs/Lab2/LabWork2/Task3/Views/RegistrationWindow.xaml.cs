using LabWork2.ViewModels;
using System.Windows;
using LabWork2.Services;

namespace LabWork2.Views
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

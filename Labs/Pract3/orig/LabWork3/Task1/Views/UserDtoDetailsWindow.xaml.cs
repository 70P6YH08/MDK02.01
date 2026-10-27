using System.Windows;
using Task1.DTOs;

namespace Task1.Views
{
    /// <summary>
    /// Логика взаимодействия для UserDtoDetails.xaml
    /// </summary>
    public partial class UserDtoDetailsWindow : Window
    {
        List<UserDto> _userDto = new();
        public UserDtoDetailsWindow()
        {
            InitializeComponent();
        }

        public UserDtoDetailsWindow(UserDto userDto)
        {
            InitializeComponent();
            _userDto.Add(userDto);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            userDtoDetailsDataGrid.ItemsSource = _userDto;
        }
    }
}

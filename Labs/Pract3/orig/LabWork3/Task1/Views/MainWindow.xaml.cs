using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Task1.DTOs;
using Task1.Models;
using Task1.Services;

namespace Task1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _usersFilePath = "users.csv";
        private readonly string _rolesFilePath = "roles.csv";
        private readonly string _privilegesFilePath = "privileges.csv";

        private int _currentPageNumber = 1;
        private int _pageSize = 6;

        List<User> users = new();
        Dictionary<int, string> roles = new();
        Dictionary<int, string> privileges = new();

        List<UserDto> userDtos = new();
        List<UserDto> userDtosView = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();

            userDtosDataGrid.ItemsSource = userDtosView;
            userDtosComboBox.SelectedIndex = 0;
        }

        private void LoadData()
        {
            DataService.GetUsers(users, _usersFilePath);
            DataService.GetUserRoles(roles, _rolesFilePath);
            DataService.GetUserPriveles(privileges, _privilegesFilePath);

            GetUserDtos();
            LoadPage();
        }

        private void LoadPage()
        {
            var newPageUserDtos = userDtos
                .Skip((_currentPageNumber - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            userDtosView = newPageUserDtos;
            userDtosDataGrid.ItemsSource = null;
            userDtosDataGrid.ItemsSource = userDtosView;
        }

        private void GetUserDtos()
        {
            userDtos.AddRange(
                users.Select(u => new UserDto
                {
                    Login = u.Login,
                    Role = roles.GetValueOrDefault(u.RoleId, "Неизвестная роль"),
                    Email = u.Email,
                    Privilege = privileges.GetValueOrDefault(u.PrivilegeId, "Неизвестная привилегия"),
                    Status = u.Status
                })
            );
        }


        private void userDtosComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (userDtosComboBox.SelectedValue is ComboBoxItem comboBoxItem)
            {
                var mode = comboBoxItem.Tag.ToString();

                var isAsc = ascOrderRadioButton.IsChecked == true;

                var sorted = mode switch
                {
                    "Login" => isAsc
                        ? userDtosView.OrderBy(x => x.Login).ToList()
                        : userDtosView.OrderByDescending(x => x.Login).ToList(),
                    "Role" => isAsc
                        ? userDtosView.OrderBy(x => x.Role).ToList()
                        : userDtosView.OrderByDescending(x => x.Role).ToList(),
                    "Privilege" => isAsc
                        ? userDtosView.OrderBy(x => x.Privilege).ToList()
                        : userDtosView.OrderByDescending(x => x.Privilege).ToList(),
                    "Status" => isAsc
                        ? userDtosView.OrderBy(x => x.Status).ToList()
                        : userDtosView.OrderByDescending(x => x.Status).ToList(),
                    "Email" => isAsc
                        ? userDtosView.OrderBy(x => x.Email).ToList()
                        : userDtosView.OrderByDescending(x => x.Email).ToList(),
                    _ => userDtosView
                };
                userDtosDataGrid.ItemsSource = sorted;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = searchTextBox.Text;
            var filterUserDtos = new List<UserDto>();

            foreach (var userDto in userDtosView)
                if (userDto.Login.Contains(search) || userDto.Email.Contains(search))
                    filterUserDtos.Add(userDto);

            userDtosDataGrid.ItemsSource = filterUserDtos;
        }

        private void userDtosDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = userDtosDataGrid.SelectedItem;

            if (selectedItem is UserDto userDto)
            {
                var userDtoWindow = new UserDtoDetailsWindow(userDto);
                userDtoWindow.ShowDialog();
            }
        }


        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPageNumber > 1)
            {
                _currentPageNumber--;
                LoadPage();
            }
            searchTextBox.Clear();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            double usersCount = userDtos.Count();
            var countPages = Math.Ceiling((double)usersCount/_pageSize);

            if (_currentPageNumber < countPages)
            {
                _currentPageNumber++;
                LoadPage();
            }
            searchTextBox.Clear();
        }
    }
}
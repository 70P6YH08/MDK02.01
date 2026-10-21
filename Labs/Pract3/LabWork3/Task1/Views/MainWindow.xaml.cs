using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Task1.DTOs;
using Task1.Models;

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
        private int _pageSize = 5;

        ObservableCollection<User> users = new();
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
            GetUsers(_usersFilePath);
            GetUserRoles(_rolesFilePath);
            GetUserPriveles(_privilegesFilePath);

            GetUserDtos();
            LoadPage();
        }

        //private void GenerateButtonPages()
        //{
        //    var stackPanel = new StackPanel();
        //    stackPanel.Orientation = Orientation.Horizontal;
        //    var countPages = userDtos.Count() / _pageSize;

        //    for (int i = 1; i <= countPages; i++)
        //    {
        //        var newButton = new Button();
        //        newButton.Content = $"{i}";
        //        newButton.Margin = new Thickness(5, 5, 5, 5);
        //        newButton.Width = 30;
        //        newButton.Height = 30;
        //        stackPanel.Children.Add(newButton);
        //    }

        //    GenerateButtonsSDockPanel.Children.Add(stackPanel);
        //}

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


        private void GetUsers(string usersFilePath)
        {
            if (!File.Exists(usersFilePath))
            {
                MessageBox.Show("Хранилище данных пользователей не найдено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string? usersData;

                using (var streamReader = new StreamReader(usersFilePath))
                {
                    while ((usersData = streamReader.ReadLine()) != null)
                    {
                        var userData = usersData.Split(';');

                        users.Add(new User
                        {
                            Id = int.TryParse(userData[0], out int userId) ? userId : userId,
                            RoleId = int.TryParse(userData[1], out int roleId) ? roleId : roleId,
                            PrivilegeId = int.TryParse(userData[2], out int privilegeId) ? privilegeId : privilegeId,
                            Login = userData[3],
                            PasswordHash = userData[4],
                            Email = userData[5],
                            Status = userData[6]
                        });
                    }
                    streamReader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetUserRoles(string rolesFilePath)
        {
            if (!File.Exists(rolesFilePath))
            {
                MessageBox.Show("Хранилище данных ролей не найдено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string? rolesData;

                using (var streamReader = new StreamReader(rolesFilePath))
                {
                    while ((rolesData = streamReader.ReadLine()) != null)
                    {
                        var roleData = rolesData.Split(';');

                        roles.Add(
                            Convert.ToInt32(roleData[0]),
                            roleData[1]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetUserPriveles(string privilegesFilePath)
        {
            if (!File.Exists(privilegesFilePath))
            {
                MessageBox.Show("Хранилище данных привилегий не найдено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string? privilegesData;

                using (var streamReader = new StreamReader(privilegesFilePath))
                {
                    while ((privilegesData = streamReader.ReadLine()) != null)
                    {
                        var privilegeData = privilegesData.Split(';');

                        privileges.Add(
                            Convert.ToInt32(privilegeData[0]),
                            privilegeData[1]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            {
                if (userDto.Login.Contains(search) || userDto.Email.Contains(search))
                    filterUserDtos.Add(userDto);
            }

            userDtosDataGrid.ItemsSource = filterUserDtos;
        }

        private void userDtosDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = userDtosDataGrid.SelectedItem;

            if (selectedItem is UserDto userDto && selectedItem != null)
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
        }
    }
}
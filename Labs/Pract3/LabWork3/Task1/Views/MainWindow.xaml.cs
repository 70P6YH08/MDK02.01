using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
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


        List<User> users = new();
        Dictionary<int, string> roles = new();
        Dictionary<int, string> privileges = new();

        List<UserDto> userDtos = new();


        public MainWindow()
        {
            InitializeComponent();
            LoadData();

            userDtosDataGrid.ItemsSource = userDtos;
            userDtosComboBox.SelectedIndex = 0;
        }

        private void LoadData()
        {
            GetUsers(_usersFilePath);
            GetUserRoles(_rolesFilePath);
            GetUserPriveles(_privilegesFilePath);

            GetUserDtos();
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
            if(userDtosComboBox.SelectedValue is ComboBoxItem comboBoxItem)
            {
                var mode = comboBoxItem.Tag.ToString();

                var isAsc = ascOrderRadioButton.IsChecked == true;

                var sorted = mode switch
                {
                    "Login" => isAsc
                        ? userDtos.OrderBy(x => x.Login).ToList()
                        : userDtos.OrderByDescending(x => x.Login).ToList(),
                    "Role" => isAsc
                        ? userDtos.OrderBy(x => x.Role).ToList()
                        : userDtos.OrderByDescending(x => x.Role).ToList(),
                    "Privilege" => isAsc
                        ? userDtos.OrderBy(x => x.Privilege).ToList()
                        : userDtos.OrderByDescending(x => x.Privilege).ToList(),
                    "Status" => isAsc
                        ? userDtos.OrderBy(x => x.Status).ToList()
                        : userDtos.OrderByDescending(x => x.Status).ToList(),
                    "Email" => isAsc
                        ? userDtos.OrderBy(x => x.Email).ToList()
                        : userDtos.OrderByDescending(x => x.Email).ToList(),
                    _ => userDtos
                };
                userDtosDataGrid.ItemsSource = sorted;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = searchTextBox.Text;
            var filterUserDtos = new List<UserDto>();

            foreach (var userDto in userDtos)
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
    }
}
using System.IO;
using System.Windows;
using Task1.DTOs;
using Task1.Models;

namespace Task1
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
        List<Role> roles = new();
        List<Privilege> privileges = new();

        List<UserDto> userDtos = new();


        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            usersDataGrid.ItemsSource = users;
        }

        private void LoadData()
        {
            users = GetUsers(_usersFilePath);
            roles = GetUserRoles(_rolesFilePath);
            privileges = GetUserPriveles(_privilegesFilePath);
        }

        private List<UserDto> GetUserDtos()
        {
            userDtos.AddRange(users.Select(u => new UserDto{
            Login = u.Login}))
        }


        static private List<User> GetUsers(string usersFilePath)
        {
            if (!File.Exists(usersFilePath))
            {   
                MessageBox.Show("Хранилище данных пользователей не найдено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<User>();
            }

            var users = new List<User>();
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
            return users;
        } 

        static private List<Role> GetUserRoles(string rolesFilePath)
        {
            if (!File.Exists(rolesFilePath))
            {
                MessageBox.Show("Хранилище данных ролей не найдено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Role>();
            }

            var roles = new List<Role>();

            try
            {
                string? rolesData;

                using (var streamReader = new StreamReader(rolesFilePath))
                {
                    while ((rolesData = streamReader.ReadLine()) != null)
                    {
                        var roleData = rolesData.Split(';');

                        roles.Add(new Role
                        {
                            Id = Convert.ToInt32(roleData[0]),
                            Name = roleData[1]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return roles;
        }
        static private List<Privilege> GetUserPriveles(string rolesFilePath)
        {
            if (!File.Exists(rolesFilePath))
            {
                MessageBox.Show("Хранилище данных привилегий не найдено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Privilege>();
            }

            var roles = new List<Privilege>();

            try
            {
                string? privilegesData;

                using (var streamReader = new StreamReader(rolesFilePath))
                {
                    while ((privilegesData = streamReader.ReadLine()) != null)
                    {
                        var privilegeData = privilegesData.Split(';');

                        roles.Add(new Privilege
                        {
                            Id = Convert.ToInt32(privilegeData[0]),
                            Name = privilegeData[1]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return roles;
        }
    }
}
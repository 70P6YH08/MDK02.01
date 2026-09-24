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
        List<User> users = new();

        List<User> resetUsers = new();
        
        List<Role> roles = new();

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
            
        }

        private void GetUserDtos()
        {
            foreach (var user in users)
            {
                userDtos.Add(new UserDto
                {
                    Login = user.Login,
                    Role = roles.FirstOrDefault()?.Name.Where(user.RoleId == roles.First().Id)
                }
            }
        }

        private List<User> GetUsers(string usersFilePath)
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
                            Id = Convert.ToInt32(userData[0]),
                            RoleId = roles.Select(r => r.Name),
                            Login = userData[2],
                            PasswordHash = userData[3],
                            Email = userData[4],
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
    }
}
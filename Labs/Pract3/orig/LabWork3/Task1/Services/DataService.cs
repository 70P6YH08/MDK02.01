using System.IO;
using System.Windows;
using Task1.Models;

namespace Task1.Services
{
    public class DataService
    {
        public static void GetUsers(List<User> users, string usersFilePath)
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
        public static void GetUserRoles(Dictionary<int, string> roles, string rolesFilePath)
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

        public static void GetUserPriveles(Dictionary<int, string> privileges, string privilegesFilePath)
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
    }
}

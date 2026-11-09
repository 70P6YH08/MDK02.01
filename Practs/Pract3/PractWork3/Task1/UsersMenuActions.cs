using PractLibrary.Services;
using PractWorkLibrary.DTOs;
using PractWorkLibrary.Models;
using PractWorkLibrary.Services;
using System.Reflection;

namespace Task1
{
    public class UsersMenuActions
    {
        private readonly UserService _userSerivce;
        private readonly RoleService _roleService;
        private readonly ConsoleHelper _consoleHelper;
        private readonly ConsoleNavigation _consoleNavigation;
        private readonly DataFormater _dataFormater;

        public UsersMenuActions(
            UserService userService,
            RoleService roleService,
            ConsoleHelper consoleHelper,
            ConsoleNavigation consoleNavigation,
            DataFormater dataFormater
            )
        {
            _userSerivce = userService;
            _roleService = roleService;
            _consoleHelper = consoleHelper;
            _consoleNavigation = consoleNavigation;
            _dataFormater = dataFormater;
        }

        public async Task UsersViewAsync()
        {
            var pageSize = 2;
            var pageNumber = 1;

            var usersCount = await _userSerivce.GetUsersCountAsync();
            var pagesCount = (int)Math.Ceiling((decimal)usersCount / pageSize);

            int addWidth = 2;

            while (true)
            {
                Console.Clear();

                var users = await _userSerivce.GetUserDtosAsync(pageNumber, pageSize);

                if (users == null)
                {
                    Console.WriteLine("\nСписок пользователей пуст");
                    _consoleNavigation.WaitQuit();
                    return;
                }

                var usersType = users.GetType().GetGenericArguments()[0];
                var userProps = usersType.GetProperties();

                List<int> widthColumns = new();

                foreach (var prop in userProps)
                {
                    int columnWidth = Math.Max(
                        prop.Name.Length,
                        users.Max(u => prop
                            .GetValue(u)?
                            .ToString()?.Length ?? 0)) + addWidth;
                    widthColumns.Add(columnWidth);
                }

                int intersectionCount = widthColumns.Count;
                var tableWidth = widthColumns.Sum() + intersectionCount;

                var rowBorder = new string('-', tableWidth);

                Console.WriteLine("=====Список пользователей=====\n");

                Console.WriteLine(rowBorder);
                for (int i = 0; i < userProps.Length; i++)
                {
                    Console.Write(_dataFormater.DataTableCenter(userProps[i].Name, widthColumns[i]) + "|");
                }

                Console.WriteLine("\n" + rowBorder);

                foreach (var user in users)
                {
                    for (int i = 0; i < userProps.Length; i++)
                    {
                        var userPropValue = userProps[i].GetValue(user)?.ToString();
                        Console.Write(_dataFormater.DataTableCenter(userPropValue, widthColumns[i]) + "|");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine(rowBorder);

                Console.WriteLine($"\n\t\t[{pageNumber} / {pagesCount}]\n");

                Console.WriteLine("[<-] - Предыдущая страница");
                Console.WriteLine("[->] - Следующая страница");
                Console.WriteLine("[ESC] - Выход в главное меню");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (pageNumber > 1)
                            pageNumber--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (pageNumber < pagesCount)
                            pageNumber++;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        public async Task UserDetailsViewAsync()
        {
            Console.Clear();

            Console.Write("Введите ID пользователя: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("\nНекорректный ID");
                _consoleNavigation.WaitQuit();
                return;
            }

            Console.WriteLine();

            var user = await _userSerivce.GetUserDetailsDtoAsync(userId);

            if (user == null)
            {
                Console.WriteLine($"Пользователь {userId} не найден");
                _consoleNavigation.WaitQuit();
                return;
            }

            int addWidth = 2;
            var userProps = user.GetType().GetProperties();

            List<string> propsValues = new();
            for (int i = 0; i < userProps.Length; i++)
                propsValues.Add(userProps[i].GetValue(user)?.ToString() ?? "");

            List<int> widthValueColumns = new();

            foreach (var prop in userProps)
            {
                int columnWidth = Math.Max(
                    prop.Name.Length,
                    propsValues.Max(p => prop
                        .GetValue(user)?
                        .ToString()?.Length ?? 0)) + addWidth;
                widthValueColumns.Add(columnWidth);
            }

            int intersectionCount = widthValueColumns.Count;
            var tableWidth = widthValueColumns.Sum() + intersectionCount;

            var rowBorder = new string('-', tableWidth);

            Console.WriteLine($"=====Пользователь {userId}=====");

            Console.WriteLine();

            Console.WriteLine(rowBorder);

            for (int i = 0; i < userProps.Length; i++)
                Console.Write(_dataFormater.DataTableCenter(userProps[i].Name, widthValueColumns[i]) + "|");

            Console.WriteLine("\n" + rowBorder);

            for (int i = 0; i < userProps.Length; i++)
                Console.Write(_dataFormater.DataTableCenter(propsValues[i], widthValueColumns[i]) + "|");
            Console.WriteLine();

            Console.WriteLine(rowBorder);

            _consoleNavigation.WaitQuit();
        }

        public async Task InsertUserViewAsync()
        {
            Console.Clear();

            Console.WriteLine($"=====ДОБАВЛЕНИЕ ДАННЫХ=====");

            var firstName = _consoleHelper.WriteUnrequiredString("Имя:");
            var lastName = _consoleHelper.WriteUnrequiredString("Фамилия:");

            var login = _consoleHelper.WriteRequiredString("Логин: ");
            var email = _consoleHelper.WriteRequiredString("Почта: ");
            var phoneNumber = _consoleHelper.WriteRequiredString("Номер телефона: ");
            var password = _consoleHelper.WriteRequiredString("Пароль: ");

            var roles = await _roleService.GetRoleDtosAsync();

            if (roles is null)
            {
                Console.WriteLine("Список ролей пуст");
                _consoleNavigation.WaitQuit();
                return;
            }
            ;

            Console.WriteLine("=====Список ролей=====");
            foreach (var role in roles)
                Console.WriteLine($"[{role.Id}] - [{role.Name}]");

            var roleId = _consoleHelper.WriteRoleId("Роль: ", roles);

            var createUserDto = new CreateUserDto
            {
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                Email = email,
                PhoneNumber = phoneNumber,
                Password = password,
                RoleId = roleId
            };

            try
            {
                await _userSerivce.AddUserAsync(createUserDto);
                Console.WriteLine($"Пользователь успешно добавлен");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _consoleNavigation.WaitQuit();
        }

        public async Task UpdateUserViewAsync()
        {
            Console.Clear();

            Console.WriteLine($"=====ИЗМЕНЕНИЕ ДАННЫХ=====");

            var userId = _consoleHelper.WriteRequiredInt("ID пользователя: ");

            var firstName = _consoleHelper.WriteUnrequiredString("Имя:");
            var lastName = _consoleHelper.WriteUnrequiredString("Фамилия:");

            var login = _consoleHelper.WriteRequiredString("Логин: ");
            var email = _consoleHelper.WriteRequiredString("Почта: ");
            var phoneNumber = _consoleHelper.WriteRequiredString("Номер телефона: ");

            var roles = await _roleService.GetRoleDtosAsync();

            if (roles is null)
            {
                Console.WriteLine("Список ролей пуст");
                _consoleNavigation.WaitQuit();
                return;
            }
            ;

            Console.WriteLine("=====Список ролей=====");
            foreach (var role in roles)
                Console.WriteLine($"[{role.Id}] - [{role.Name}]");

            var roleId = _consoleHelper.WriteRoleId("Роль: ", roles);

            var password = _consoleHelper.WriteRequiredString("Подтвердите изменения паролем: ");

            var updateUserDto = new UpdateUserDto
            {
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                Email = email,
                PhoneNumber = phoneNumber,
                RoleId = roleId
            };

            try
            {
                await _userSerivce.UpdateUserAsync(userId, updateUserDto, password);
                Console.WriteLine($"Данные пользователя {userId} успешно обновлены");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _consoleNavigation.WaitQuit();
        }

        public async Task DeleteUserViewAsync()
        {
            Console.Clear();

            Console.WriteLine($"=====УДАЛЕНИЕ ДАННЫХ=====");
            var userId = _consoleHelper.WriteRequiredInt("ID пользователя: ");

            Console.WriteLine("Вы уверены, что хотите удалить пользователя? [y/n]");

            if (Console.ReadKey(true).Key != ConsoleKey.Y)
            {
                Console.WriteLine("Удаление отменено");
                _consoleNavigation.WaitQuit();
                return;
            }

            try
            {
                await _userSerivce.DeleteUserAsync(userId);
                Console.WriteLine($"Пользовать успешно удален");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _consoleNavigation.WaitQuit();
        }
    }
}

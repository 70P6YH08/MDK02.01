
using PractLibrary.Services;
using PractWorkLibrary.Contexts;
using PractWorkLibrary.Services;
using Task1;

public class Program
{
    private readonly UsersMenuActions _usersMenu;

    public Program()
    {
        var practWorkDbContext = new PractWorkDbContext();
        var dataValidator = new DataValidator();
        var passwordHasher = new PasswordHasher();
        var dataAvailabilityChecker = new DataAvailabilityChecker(practWorkDbContext);

        var userService = new UserService(
            practWorkDbContext,
            dataValidator,
            passwordHasher,
            dataAvailabilityChecker);

        var roleService = new RoleService(practWorkDbContext);

        var consoleHelper = new ConsoleHelper();
        var consoleNavigation = new ConsoleNavigation();
        var dataFormater = new DataFormater();

        _usersMenu = new UsersMenuActions(
            userService,
            roleService,
            consoleHelper,
            consoleNavigation,
            dataFormater);
    }

    private static async Task Main(string[] args)
    {
        var program = new Program();
        await program.RunAsync();
    }

    private async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine($"=====УПРАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯМИ=====\n");
            Console.WriteLine("[1] - Просмотр пользователей");
            Console.WriteLine("[2] - Просмотр деталей пользователя");
            Console.WriteLine("[3] - Изменение данных пользователя");
            Console.WriteLine("[4] - Добавление пользователя");
            Console.WriteLine("[5] - Удаление пользователя");
            Console.WriteLine("[6] - Сортировать пользователей");
            Console.WriteLine("[7] - Фильтровать пользователей");
            Console.WriteLine("[Esc] - Выход из приложения");

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    await _usersMenu.UsersViewAsync();
                    break;
                case ConsoleKey.D2:
                    await _usersMenu.UserDetailsViewAsync();
                    break;
                case ConsoleKey.D3:
                    await _usersMenu.UpdateUserViewAsync();
                    break;
                case ConsoleKey.D4:
                    await _usersMenu.InsertUserViewAsync();
                    break;
                case ConsoleKey.D5:
                    await _usersMenu.DeleteUserViewAsync();
                    break;
            }
        }
    }
}
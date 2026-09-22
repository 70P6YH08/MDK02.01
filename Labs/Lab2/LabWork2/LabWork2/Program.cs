using System.Text.RegularExpressions;

ReturnCalculate();

IsReliablePassword();

void ReturnCalculate()
{
    Console.Write("Введите число: ");
    var a = Convert.ToDouble(Console.ReadLine());
    Console.Write("Введите степень числа: ");
    var n = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine(PowerCalculation(a, n));
}

static string PowerCalculation(double a, int n)
    => $"Результат {Math.Round(Math.Pow(a, n), 3)}";

void IsReliablePassword()
{
    Console.Write("Введите пароль: ");
    var password = Console.ReadLine();

    Console.WriteLine(IsEqualsRegexPassword(password));
}

static bool IsEqualsRegexPassword(string password)
{
    string passwordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[\p{P}\p{S}])[A-Za-z0-9\p{P}\p{S}]{8,30}$";
    return Regex.IsMatch(password, passwordPattern);
}
using System.Text.RegularExpressions;

namespace PractWorkLibrary.Services
{
    public class DataValidator
    {
        private const string _loginRegex = @"^[a-zA-Z0-9_]{3,10}$";
        private const string _emailRegex = @"^[a-zA-Z0-9]{3,}@[a-zA-Z0-9]{2,}\.[a-zA-Z]{2,}$";
        private const string _passwordRegex = @"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!@#$%^&*()-]).{12,}$";
        private const string _phoneNumberRegex = @"^(\+7|7|8)?[\s|\-]?[9][0-9]{2}[\-|\s]?[0-9]{3}[\s|\-]?[0-9]{2}[\s|\-]?[0-9]{2}$";

        private (bool isValid, string? error) IsValidLogin(string login)
        {
            if (String.IsNullOrWhiteSpace(login))
                return (false, "Логин пуст");
            if (!Regex.IsMatch(login, _loginRegex))
                return (false, "Неверный формат логина");
            return (true, null);
        }

        private (bool isValid, string? error) IsValidEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                return (false, "Почта пуста");
            if (!Regex.IsMatch(email, _emailRegex))
                return (false, "Неверный формат почты");
            return (true, null);
        }

        private (bool isValid, string? error) IsValidPassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
                return (false, "Пароль пуст");
            if (!Regex.IsMatch(password, _passwordRegex))
                return (false, "Неверный формат пароля");
            return (true, null);
        }

        private (bool isValid, string? error) IsValidPhoneNumber(string phoneNumber)
        {
            if (String.IsNullOrWhiteSpace(phoneNumber))
                return (false, "Номер телефона пуст");
            if (!Regex.IsMatch(phoneNumber, _phoneNumberRegex))
                return (false, "Неверный формат номера телефона");
            return (true, null);
        }

        public (bool isValid, string? error) CheckUserDataValidationAsync(string login, string email, string phoneNumber, string? password = null)
        {
            var loginValidation = IsValidLogin(login);
            if (!loginValidation.isValid)
                return loginValidation;

            var emailValidation = IsValidEmail(email);
            if (!emailValidation.isValid)
                return emailValidation;

            var phoneNumberValidation = IsValidPhoneNumber(phoneNumber);
            if (!phoneNumberValidation.isValid)
                return phoneNumberValidation;

            if (password != null)
            {
                var passwordValidation = IsValidPassword(password);
                if (!passwordValidation.isValid)
                    return passwordValidation;
            }

            return (true, null);
        }
    }
}

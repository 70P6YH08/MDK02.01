using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

namespace LabWork1.Services
{
    public class DataValidator
    {
        internal const string filePath = "users.csv";
        private const string _passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{7,}$";

        private const string _loginRegex = @"^[a-zA-Z][a-zA-Z0-9_]{2,25}$";

        private const string _mailRegex = @"^[a-zA-Z0-9_%+-]+(?:\.[a-zA-Z0-9_%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public static bool IsValidLogin(string login)
        {
            return Regex.IsMatch(login, _loginRegex);
        }

        public static bool IsValidPassword(SecureString securePassword)
        {
            IntPtr passwordBstr = IntPtr.Zero;
            try
            {
                passwordBstr = Marshal.SecureStringToBSTR(securePassword);
                string insecurePassword = Marshal.PtrToStringBSTR(passwordBstr);
                return Regex.IsMatch(insecurePassword, _passwordRegex);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(passwordBstr);
            }
        }

        public static bool IsValidMail(string mail)
        {
            //if (String.IsNullOrEmpty(mail))
            //    return true;

            return Regex.IsMatch(mail, _mailRegex);
        }

        public static string GetMessageErrorForLogin(string login)
        {
            if (login.Length < 3)
                return $"Логин должен содержать минимум 3 символа";

            return "Неверный формат логина. Пример логина: Examp1e_Log1n20";
        }

        public static string GetMessageErrorForPassword(SecureString password, SecureString returnPassword)
        {
            if (password.Length < 8 || returnPassword.Length < 8)
                return "Пароль должен содержать минимум 8 символов";

            if (password.Length != returnPassword.Length)
                return "Пароли разной длины";

            return "Неверный формат пароля.\nПример пароля: pa$Sw0rd";
        }

        public static string GetMessageErrorForMail(string mail)
        {
            if (!mail.Contains('@'))
                return "Это не почта";

            var mailParts = mail.Split('@');

            string localPart = mailParts[0];
            string domainPart = mailParts[1];

            if (localPart.StartsWith('.') || localPart.EndsWith('.'))
                return "Имя почты не может начинаться или заканчиваться точкой";

            if (domainPart.StartsWith('.') || domainPart.EndsWith('.'))
                return "Доменное имя почты не может начинаться или заканчиваться точкой";

            if (String.IsNullOrEmpty(localPart))
                return "Имя почты не может быть пустым";

            if (String.IsNullOrEmpty(domainPart))
                return "Доменное имя почты не может быть пустым";

            return "Неверный формат почты.\nПример почты: example1.qwe@gmail.com";
        }

        public static bool IsEqualsPasswords(SecureString password, SecureString returnPassword)
        {
            IntPtr passwordBstr = IntPtr.Zero;
            IntPtr returnPasswordBstr = IntPtr.Zero;

            try
            {
                passwordBstr = Marshal.SecureStringToBSTR(password);
                returnPasswordBstr = Marshal.SecureStringToBSTR(returnPassword);

                int length = Marshal.ReadInt32(passwordBstr, -4);
                for (int i = 0; i < length; i++)
                {
                    byte b1 = Marshal.ReadByte(passwordBstr, i);
                    byte b2 = Marshal.ReadByte(returnPasswordBstr, i);
                    if (b1 != b2)
                        return false;
                }

                return true;
            }
            finally
            {
                if (passwordBstr != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(passwordBstr);
                if (returnPasswordBstr != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(returnPasswordBstr);
            }
        }

        public static bool IsRightPassword(SecureString password, string passwordHash)
        {
            IntPtr passwordBstr = IntPtr.Zero;
            try
            {
                passwordBstr = Marshal.SecureStringToBSTR(password);
                var insecurePassword = Marshal.PtrToStringBSTR(passwordBstr);
                return BCrypt.Net.BCrypt.Verify(insecurePassword, passwordHash);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(passwordBstr);
            }
        }

        public static string GetHashPassword(SecureString password)
        {
            IntPtr passwordBstr = IntPtr.Zero;
            try
            {
                passwordBstr = Marshal.SecureStringToBSTR(password);
                string insecurePassword = Marshal.PtrToStringBSTR(passwordBstr);
                return BCrypt.Net.BCrypt.HashPassword(insecurePassword);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(passwordBstr);
            }
        }
    }
}

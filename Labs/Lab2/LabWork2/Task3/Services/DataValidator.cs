using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Task3.Services
{
    public class DataValidator
    {
        public static bool IsValidLogin(string login)
        {
            string loginPattern = @"^[a-zA-Z0-9_-]{3,20}$";
            return Regex.IsMatch(login, loginPattern);
        }

        public static bool IsValidPassword(string password)
        {
            string passwordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[\p{P}\p{S}])[A-Za-z0-9\p{P}\p{S}]{8,30}$";
            return Regex.IsMatch(password, passwordPattern);
        }

        public static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}

using PractLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class ConsoleHelper
    {
        public string WriteRequiredString(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();

                if (!String.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Поле не может быть пустым");
            }
        }

        public int WriteRequiredInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();

                if (int.TryParse(input, out int userId))
                    return userId;

                Console.WriteLine("Некорректный ID");
            }
        }

        public string? WriteUnrequiredString(string message)
        {
            Console.Write(message);
            var input = Console.ReadLine();
            return String.IsNullOrWhiteSpace(input) ? null : input.Trim();
        }

        public int WriteRoleId(string message, List<RoleDto> roles)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();

                if (int.TryParse(input, out int roleId) && roles.Any(r => r.Id == roleId))
                    return roleId;

                Console.WriteLine("Некорректный номер роли");
            }
        }
    }
}

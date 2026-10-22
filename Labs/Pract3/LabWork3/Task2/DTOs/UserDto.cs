using System;
using System.Collections.Generic;
using System.Text;

namespace Task2.DTOs
{
    public class UserDto
    {
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Privilege { get; set;  } = null!;
        public string Email { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}

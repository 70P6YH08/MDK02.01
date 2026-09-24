using System;
using System.Collections.Generic;
using System.Text;

namespace Task1.DTOs
{
    public class UserDto
    {
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}

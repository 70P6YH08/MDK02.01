using System;
using System.Collections.Generic;
using System.Text;

namespace LabWork1.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}

using PractWorkLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PractWorkLibrary.DTOs
{
    public class CreateUserDto
    {
        public int RoleId { get; set; } = 1;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

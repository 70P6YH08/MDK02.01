using PractWorkLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PractWorkLibrary.DTOs
{
    public class UserDto
    {
        public string UserRole { get; set; } = null!;
        public string UserPrivilege { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}

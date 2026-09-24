using System;
using System.Collections.Generic;
using System.Text;

namespace Task1.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}

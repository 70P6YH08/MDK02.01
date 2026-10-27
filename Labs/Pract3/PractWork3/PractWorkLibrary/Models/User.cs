using System;
using System.Collections.Generic;
using System.Text;

namespace PractWorkLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PrivilegeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public Privilege Privilege { get; set; } = null!;

    }
}

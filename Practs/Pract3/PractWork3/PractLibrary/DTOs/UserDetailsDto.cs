using System;
using System.Collections.Generic;
using System.Text;

namespace PractLibrary.DTOs
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string UserRole { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

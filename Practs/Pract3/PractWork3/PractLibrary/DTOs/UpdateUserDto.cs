namespace PractWorkLibrary.DTOs
{
    public class UpdateUserDto
    {
        public int RoleId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}

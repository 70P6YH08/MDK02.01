namespace PractWorkLibrary.Services
{
    public class PasswordHasher
    {
        public string PasswordHashing(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool PasswordVerification(string password, string passwordHash)
            => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}

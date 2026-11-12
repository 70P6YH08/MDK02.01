using Microsoft.EntityFrameworkCore;
using PractWorkLibrary.Contexts;

namespace PractWorkLibrary.Services
{
    public class DataAvailabilityChecker
    {
        private readonly PractWorkDbContext _context;
        public DataAvailabilityChecker(PractWorkDbContext context)
        {
            _context = context;
        }

        private async Task<(bool isAvailable, string? error)> IsAvailableLoginAsync(string login, int? userId = null)
            => !await _context.Users.AnyAsync(u => u.Login == login && (userId == null || u.Id != userId))
                ? (true, null)
                : (false, "Такой логин занят");
        private async Task<(bool isAvailable, string? error)> IsAvailableEmailAsync(string email, int? userId = null)
            => !await _context.Users.AnyAsync(u => u.Email == email && (userId == null || u.Id != userId))
                ? (true, null)
                : (false, "Такая почта занята");
        private async Task<(bool isAvailable, string? error)> IsAvailablePhoneNumberAsync(string phoneNumber, int? userId = null)
            => !await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber && (userId == null || u.Id != userId))
                ? (true, null)
                : (false, "Такой номер телефона занят");

        public async Task<(bool isAvailable, string? error)> CheckUserDataAvailabilityAsync(string login, string email, string phoneNumber, int? userId = null)
        {

            var loginAvailable = await IsAvailableLoginAsync(login, userId);
            if (!loginAvailable.isAvailable)
                return loginAvailable;

            var emailAvailable = await IsAvailableEmailAsync(email, userId);
            if (!emailAvailable.isAvailable)
                return emailAvailable;

            var phoneNumberAvailable = await IsAvailablePhoneNumberAsync(phoneNumber, userId);
            if (!phoneNumberAvailable.isAvailable)
                return phoneNumberAvailable;

            return (true, null);
        }
    }
}

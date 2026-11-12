using Microsoft.EntityFrameworkCore;
using PractLibrary.DTOs;
using PractWorkLibrary.Contexts;
using PractWorkLibrary.DTOs;
using PractWorkLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace PractWorkLibrary.Services
{
    public class UserService
    {
        private PractWorkDbContext _practWorkDbContext;
        private DataValidator _dataValidator;
        private PasswordHasher _passwordHasher;
        private DataAvailabilityChecker _dataAvailabilityChecker;

        public UserService(
            PractWorkDbContext practWorkDbContext,
            DataValidator dataValidator,
            PasswordHasher passwordHasher,
            DataAvailabilityChecker dataAvailabilityChecker
            )
        {
            _practWorkDbContext = practWorkDbContext;
            _dataValidator = dataValidator;
            _passwordHasher = passwordHasher;
            _dataAvailabilityChecker = dataAvailabilityChecker;
        }

        public async Task<int> GetUsersCountAsync()
            => await _practWorkDbContext.Users.CountAsync();

        public async Task<List<UserDto>?> GetUserDtosAsync(int pageNumber, int pageSize)
            => await _practWorkDbContext.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    FirstName = u.FirstName,
                    Login = u.Login,
                    UserRole = u.Role.Name
                })
                .OrderBy(u => u.Login)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        public async Task<UserDetailsDto?> GetUserDetailsDtoAsync(int userId)
            => await _practWorkDbContext.Users
                .Include(u => u.Role)
                .Where(u => u.Id == userId)
                .Select(u => new UserDetailsDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Login = u.Login,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserRole = u.Role.Name
                }).FirstOrDefaultAsync();

        public async Task AddUserAsync(CreateUserDto createUserDto)
        {
            if (createUserDto is null)
                throw new Exception("Данные пользователя не указаны");

            var roleExists = await _practWorkDbContext.Roles.AnyAsync(r => r.Id == createUserDto.RoleId);
            if (!roleExists)
                throw new Exception("Такой роли не сущетвует");

            var vaildateUserData = _dataValidator.CheckUserDataValidationAsync(
                createUserDto.Login,
                createUserDto.Email,
                createUserDto.PhoneNumber,
                createUserDto.Password);

            if (!vaildateUserData.isValid)
                throw new ValidationException(vaildateUserData.error);

            var availableUserData = await _dataAvailabilityChecker.CheckUserDataAvailabilityAsync(
                createUserDto.Login,
                createUserDto.Email,
                createUserDto.PhoneNumber);

            if (!availableUserData.isAvailable)
                throw new Exception(availableUserData.error);


            var passwordHash = _passwordHasher.PasswordHashing(createUserDto.Password);

            var newUser = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Login = createUserDto.Login,
                PhoneNumber = createUserDto.PhoneNumber,
                PasswordHash = passwordHash,
                RoleId = createUserDto.RoleId
            };

            await _practWorkDbContext.Users.AddAsync(newUser);
            await _practWorkDbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto, string password)
        {
            if (updateUserDto is null)
                throw new ArgumentNullException("Данные пользователя не указаны");

            var user = await _practWorkDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new ArgumentNullException("Пользователь не найден");

            if (password is null)
                throw new ArgumentNullException("Пароль пуст");

            if (!_passwordHasher.PasswordVerification(password, user.PasswordHash))
                throw new Exception("Неверный пароль");


            var roleExists = await _practWorkDbContext.Roles.AnyAsync(r => r.Id == updateUserDto.RoleId);
            if (!roleExists)
                throw new Exception("Такой роли не существует");

            var vaildateUserData = _dataValidator.CheckUserDataValidationAsync(
                updateUserDto.Login,
                updateUserDto.Email,
                updateUserDto.PhoneNumber);

            if (!vaildateUserData.isValid)
                throw new ValidationException(vaildateUserData.error);

            var availableUserData = await _dataAvailabilityChecker.CheckUserDataAvailabilityAsync(
                updateUserDto.Login,
                updateUserDto.Email,
                updateUserDto.PhoneNumber,
                userId);

            if (!availableUserData.isAvailable)
                throw new Exception(availableUserData.error);

            if(updateUserDto.FirstName != null)
                user.FirstName = updateUserDto.FirstName;
            if(updateUserDto.LastName != null)
                user.LastName = updateUserDto.LastName;

            user.Login = updateUserDto.Login;
            user.Email = updateUserDto.Email;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.RoleId = updateUserDto.RoleId;

            await _practWorkDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _practWorkDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new Exception("Пользователь не найден");

            _practWorkDbContext.Users.Remove(user);
            await _practWorkDbContext.SaveChangesAsync();
        }
    }
}

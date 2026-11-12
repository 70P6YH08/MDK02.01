using Microsoft.EntityFrameworkCore;
using PractWorkLibrary.Models;

namespace PractWorkLibrary.Contexts
{
    public class PractWorkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        public PractWorkDbContext()
            => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=pract3.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, RoleId = 3, FirstName = "Ким", LastName = "Горбунов",
                    Login = "k131lah", Email = "kimgorbunov@outlook.com", PhoneNumber = "89502575888",
                    PasswordHash = "$2a$11$cYlMbiJzzb45/gCbsZ3HVOSpTBWYHaXIw/DMzYJLFdBHz9B6fZBqW"
                }, //123
                new User() { Id = 2, RoleId = 1, FirstName = "Егор", LastName = "Денисов",
                    Login = "mefq", Email = "mefqgov333@gmail.com", PhoneNumber = "89256227222",
                    PasswordHash = "$2a$11$c9YWTI3tzbmqCtJkYcdCZuCma7bSSgQigoz3NiC9rl4LB.5acby7e"
                }, //egor
                new User() { Id = 3, RoleId = 4, FirstName = "Милена", LastName = "Леонтьева",
                    Login = "blsbabmilena", Email = "milena@yandex.ru", PhoneNumber = "89422627223",
                    PasswordHash = "$2a$11$i5EKVnCmVLheh79X2PqOB.fPVz8xeN16BdN81iwR00Wirwt1ra5Jq"
                }, //milenasbab
                new User() { Id = 4, RoleId = 4, FirstName = "Оля",
                    Login = "OlyaAlt", Email = "alt2007@gamil.com", PhoneNumber = "89131630923",
                    PasswordHash = "$2a$11$DiVcYcvudnQ/yf8oxmpFqeVHST/0fwkOuHO/dUSfVJ1xJ56wxbaJ2"
                }, //2007alt
                new User() { Id = 5, RoleId = 2, FirstName = "Илья",
                    Login = "6opec", Email = "ilyaalferov@gmail.com", PhoneNumber = "89426136122",
                    PasswordHash = "$2a$11$hvFne54jufHhnBeA8wqaZeGiki4oOpht9K7Lsq7dxRYaN.lYHWDpm"
                } //bolshaya_sheya
            );
            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Name = "customer"},
                new Role() { Id = 2, Name = "manager"},
                new Role() { Id = 3, Name = "admin"},
                new Role() { Id = 4, Name = "worker"}
            );
        }
    }
}

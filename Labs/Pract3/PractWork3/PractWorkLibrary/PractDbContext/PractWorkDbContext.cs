using Microsoft.EntityFrameworkCore;
using PractWorkLibrary.Models;

namespace PractWorkLibrary.Context
{
    public class PractWorkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public PractWorkDbContext() : base()
            => Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Алиса", LastName = "Шубина", Login = "Alisa", Email = "alisa20@gmail.com", Status = "Active", PrivilegeId = 1, RoleId = 2 },
                new User { Id = 2, LastName = "Горбнов", Login = "Kimasik", Email = "gorbunov@gmail.com", Status = "Inactive", PrivilegeId = 2, RoleId = 1 },
                new User { Id = 3, LastName = "Денисов", Login = "YAGA", Email = "denegDay@gmail.com", Status = "Inactive", PrivilegeId = 2, RoleId = 3 },
                new User { Id = 4, FirstName = "Анрдей", Login = "Minya", Email = "minya@gmail.com", Status = "Active", PrivilegeId = 2, RoleId = 3 },
                new User { Id = 5, FirstName = "Никита", LastName = "Матигоров", Login = "NIKICH", Email = "matigorov2006@gmail.com", Status = "Active", PrivilegeId = 2, RoleId = 2 }
            );
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "manager"},
                new Role { Id = 2, Name = "admin"},
                new Role { Id = 3, Name = "worker"},
                new Role { Id = 4, Name = "customer"}
            );
            modelBuilder.Entity<Privilege>().HasData(
                new Privilege { Id = 1, Name = "vip" },
                new Privilege { Id = 2, Name = "immortal" }
            );
        }
    }
}

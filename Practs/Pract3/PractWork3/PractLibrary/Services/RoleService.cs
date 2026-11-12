using Microsoft.EntityFrameworkCore;
using PractLibrary.DTOs;
using PractWorkLibrary.Contexts;
using PractWorkLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PractLibrary.Services
{
    public class RoleService
    {
        private readonly PractWorkDbContext _practWorkDbContext;

        public RoleService(PractWorkDbContext practWorkDbContext)
        {
            _practWorkDbContext = practWorkDbContext;
        }

        public async Task<List<RoleDto>?> GetRoleDtosAsync()
            => await _practWorkDbContext.Roles
                .Select(u => new RoleDto
                {
                    Id = u.Id,
                    Name = u.Name
                })
                .ToListAsync();
    }
}

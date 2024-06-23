using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserManagementService.Repository.Services;

namespace UserManagementService.Model
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<UserDetail> UserDetails { get; set; }
    }
}

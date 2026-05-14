using Auth.Api.DAL.Configurations;
using Auth.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.DAL
{
    public class AuthDbContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

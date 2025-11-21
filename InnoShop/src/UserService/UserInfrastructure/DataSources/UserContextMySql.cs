using Microsoft.EntityFrameworkCore;
using UserInfrastructure.Entities;

namespace UserInfrastructure.DataSources;

public class UserContextMySql : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public UserContextMySql(DbContextOptions<UserContextMySql> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasKey(u =>  u.Id);
        modelBuilder.Entity<UserEntity>().HasIndex(p => p.Email).IsUnique();
    }
}
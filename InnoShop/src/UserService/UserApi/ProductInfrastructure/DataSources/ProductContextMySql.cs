using Microsoft.EntityFrameworkCore;
using ProductInfrastructure.Entities;

namespace ProductInfrastructure.DataSources;

public class ProductContextMySql : DbContext
{
    public DbSet<ProductEntity> Products { get; set; }

    public ProductContextMySql(DbContextOptions<ProductContextMySql> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>().HasKey(p => p.Id);
    }
}
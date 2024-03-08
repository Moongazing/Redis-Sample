using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Entities;

namespace RedisExampleApp.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id= 1,
                Name = "Keyboard",
                Price = 1000
            },
            new Product {
                Id = 2,
                Name = "Mouse",
                Price = 1000

            },
            new Product
            {
                Id = 3,
                Name = "Monitor",
                Price = 1000
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}

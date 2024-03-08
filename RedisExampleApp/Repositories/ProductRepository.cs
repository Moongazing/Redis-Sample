using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Data;
using RedisExampleApp.Entities;

namespace RedisExampleApp.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext context;

    public ProductRepository(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<Product> CreateAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        context.Remove(id);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> GetAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
        return product;
    }
}

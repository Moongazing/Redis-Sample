using RedisExampleApp.Entities;

namespace RedisExampleApp.Repositories;

public interface IProductRepository
{

    Task<List<Product>> GetAsync();
    Task<Product> GetById(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);

}

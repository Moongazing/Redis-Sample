using Cache;
using RedisExampleApp.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.Repositories
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository productRepository;
        private readonly RedisService redisService;
        private readonly IDatabase cacheRepository;

        public ProductRepositoryWithCache(IProductRepository productRepository, RedisService redisService)
        {
            this.productRepository = productRepository;
            this.redisService = redisService;
            cacheRepository = redisService.GetDb(2);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await productRepository.CreateAsync(product);
            if(await cacheRepository.KeyExistsAsync(productKey))
            {
                await cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }
            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if(!await cacheRepository.KeyExistsAsync(productKey))
            {
                await LoadCacheFromDatabaseAsync();
            }
            var products = new List<Product>();
            foreach (var item in  (await cacheRepository.HashGetAllAsync(productKey)).ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value!);
                products.Add(product);
            }
            return products;
        }

        public async Task<Product> GetById(int id)
        {
            if(!await cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadCacheFromDatabaseAsync();
            return products.FirstOrDefault(x=> x.Id == id);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var updatedProduct = await productRepository.UpdateAsync(product);

            if (await cacheRepository.KeyExistsAsync(productKey))
            {
                await cacheRepository.HashSetAsync(productKey, updatedProduct.Id.ToString(), JsonSerializer.Serialize(updatedProduct));
            }

            return updatedProduct;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await productRepository.DeleteAsync(id);
            if (!deleted)
            {
                return false;
            }

            if (await cacheRepository.KeyExistsAsync(productKey))
            {
                await cacheRepository.HashDeleteAsync(productKey, id.ToString());
            }

            return true;
        }

        private async Task<List<Product>> LoadCacheFromDatabaseAsync()
        {
            var products = await productRepository.GetAsync();
            products.ForEach(p =>
            {
                cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });
            return products;
        }

    }
}

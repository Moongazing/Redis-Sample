using Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.Entities;
using RedisExampleApp.Repositories;

namespace RedisExampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;


        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await productRepository.GetAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await productRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty, await productRepository.CreateAsync(product));
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            return Ok(await productRepository.UpdateAsync(product));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await productRepository.DeleteAsync(id));
        }
    }
}


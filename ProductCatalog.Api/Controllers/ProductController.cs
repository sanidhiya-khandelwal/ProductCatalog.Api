using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly IDistributedCache _cache;
        private readonly string _cacheKey = "products_cache";
        public ProductController(IProductRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //fetch from cache
            var cachedData = await _cache.GetStringAsync(_cacheKey);
            if (cachedData != null)
            {
                var cachedProducts = JsonSerializer.Deserialize<List<Product>>(cachedData);  
                return Ok(cachedProducts);
            }

            //fetch from db
            var dbProducts = await _repo.GetAllAsync();

            //store in cache
            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(_cacheKey, JsonSerializer.Serialize(dbProducts), cacheOptions);
            return Ok(dbProducts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _repo.AddAsync(product);

            //invalidate cache
            await _cache.RemoveAsync(_cacheKey);

            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();
            var existingProduct = await _repo.GetByIdAsync(id);
            if (existingProduct is null)
                return NotFound();
            await _repo.UpdateAsync(product);

            //invalidate cache
            await _cache.RemoveAsync(_cacheKey);
            return Ok("Product Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _repo.GetByIdAsync(id);
            if (existingProduct is null)
                return NotFound();
            await _repo.DeleteAsync(id);

            //invalidate cache
            await _cache.RemoveAsync(_cacheKey);

            return Ok("Product Deleted Successfully");
        }

    }
}

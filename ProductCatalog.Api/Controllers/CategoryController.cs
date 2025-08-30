using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Dtos;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        private readonly IDistributedCache _cache;
        private readonly string _cacheKey = "categories_cache";

        public CategoryController(ICategoryRepository repo, IDistributedCache cache)
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
                var cachedCategories = JsonSerializer.Deserialize<List<Category>>(cachedData);
                return Ok(cachedCategories);
            }

            //fetch from db
            var dbCategories = await _repo.GetAllAsync();

            //store in cache
            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(_cacheKey, JsonSerializer.Serialize(dbCategories), cacheOptions);
            return Ok(dbCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequestDto category)
        {
            await _repo.AddAsync(category);

            //invalidate cache
            await _cache.RemoveAsync(_cacheKey);

            return Ok("Category created successfully");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.CategoryId)
                return BadRequest();
            var existingCategory = await _repo.GetByIdAsync(id);
            if (existingCategory is null)
                return NotFound();
            await _repo.UpdateAsync(category);

            //invalidate cache
            await _cache.RemoveAsync(_cacheKey);
            return Ok("Category Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCategory = await _repo.GetByIdAsync(id);
            if (existingCategory is null)
                return NotFound();
            await _repo.DeleteAsync(id);

            //invalidate cache
            await _cache.RemoveAsync(_cacheKey);

            return Ok("Category Deleted Successfully");
        }
    }
}

using ProductCatalog.Domain.Dtos;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(CategoryRequestDto category);

        Task DeleteAsync(int categoryId);

        Task UpdateAsync(Category category);

        Task<Category?> GetByIdAsync(int categoryId);

        Task<IEnumerable<Category>> GetAllAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);

        Task DeleteAsync(int productId);

        Task UpdateAsync(Product product);

        Task<Product?> GetByIdAsync(int productId);

        Task<IEnumerable<Product>> GetAllAsync();
    }
}

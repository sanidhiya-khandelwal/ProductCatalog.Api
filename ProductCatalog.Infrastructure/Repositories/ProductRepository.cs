using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.SqlQueries;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbConnectionHelper _dbConnectionHelper;
        
        public ProductRepository(DbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }
        public async Task AddAsync(Product product)
        {
            using var connection = _dbConnectionHelper.GetConnection();
            await connection.ExecuteAsync(ProductQueries.Insert, product);
        }

        public async Task DeleteAsync(int productId)
        {
            using var connection = _dbConnectionHelper.GetConnection();
            await connection.ExecuteAsync(ProductQueries.Delete, new { ProductId = productId });
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = _dbConnectionHelper.GetConnection();
            return await connection.QueryAsync<Product>(ProductQueries.GetAll);
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            using var connection = _dbConnectionHelper.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(ProductQueries.GetById, new { ProductId = productId });
        }

        public async Task UpdateAsync(Product product)
        {
            using var connection = _dbConnectionHelper.GetConnection();
            await connection.ExecuteAsync(ProductQueries.Update, product);
        }
    }
}

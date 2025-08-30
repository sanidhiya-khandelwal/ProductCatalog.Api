using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Dtos;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.SqlQueries;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbConnectionHelper _dbConnectionHelper;

        public CategoryRepository(DbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }
        public async Task AddAsync(CategoryRequestDto category)
        {
            var connection = _dbConnectionHelper.GetConnection();
            await connection.ExecuteAsync(CategoryQueries.Insert,
                new { CategoryName = category.CategoryName });
        }

        public async Task DeleteAsync(int categoryId)
        {
            var connection = _dbConnectionHelper.GetConnection();
            await connection.ExecuteAsync(CategoryQueries.Delete, new { CategoryId = categoryId });
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = _dbConnectionHelper.GetConnection();
            return await connection.QueryAsync<Category>(CategoryQueries.GetAll);
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            using var connection = _dbConnectionHelper.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Category>(CategoryQueries.GetById, new { CategoryId = categoryId });
        }

        public async Task UpdateAsync(Category category)
        {
            using var connection = _dbConnectionHelper.GetConnection();
            await connection.ExecuteAsync(CategoryQueries.Update, category);
        }
    }
}

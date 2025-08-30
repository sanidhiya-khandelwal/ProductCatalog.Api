using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Infrastructure.SqlQueries
{
    public static class CategoryQueries
    {
        public const string GetAll = "SELECT * FROM Categories";
        public const string GetById = "SELECT * FROM Categories WHERE CategoryId = @CategoryId";
        public const string Insert = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";
        public const string Update = "UPDATE Categories SET CategoryName=@CategoryName WHERE CategoryId=@CategoryId";
        public const string Delete = "DELETE FROM Categories WHERE CategoryId=@CategoryId";
    }
}

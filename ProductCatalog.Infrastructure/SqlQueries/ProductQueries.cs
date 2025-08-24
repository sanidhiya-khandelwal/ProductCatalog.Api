namespace ProductCatalog.Infrastructure.SqlQueries
{
    public static class ProductQueries
    {
        public const string GetAll = "SELECT * FROM Products";
        public const string GetById = "SELECT * FROM Products WHERE ProductId = @ProductId";
        public const string Insert = "INSERT INTO Products (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)";
        public const string Update = "UPDATE Products SET Name=@Name, Price=@Price, Quantity=@Quantity WHERE ProductId=@ProductId";
        public const string Delete = "DELETE FROM Products WHERE ProductId=@ProductId";
    }
}

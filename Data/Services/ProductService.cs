using Dapper;
using BlazorCRM.Data.Models;

namespace BlazorCRM.Data.Services;

public class ProductService
{
    private readonly DatabaseConnection _db;

    public ProductService(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Product>("SELECT * FROM Products");
    }

    public async Task<Product> GetProduct(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<Product>(
            "SELECT * FROM Products WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateProduct(Product product)
    {
        using var connection = _db.GetConnection();
        string sql = @"INSERT INTO Products (Name, Description, ProductGroupId, Cost, SalePrice) 
                      VALUES (@Name, @Description, @ProductGroupId, @Cost, @SalePrice);
                      SELECT LAST_INSERT_ID();";
        return await connection.ExecuteScalarAsync<int>(sql, product);
    }

    public async Task UpdateProduct(Product product)
    {
        using var connection = _db.GetConnection();
        string sql = @"UPDATE Products 
                      SET Name = @Name, Description = @Description, 
                          ProductGroupId = @ProductGroupId, Cost = @Cost, 
                          SalePrice = @SalePrice 
                      WHERE Id = @Id";
        await connection.ExecuteAsync(sql, product);
    }

    public async Task DeleteProduct(int id)
    {
        using var connection = _db.GetConnection();
        await connection.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
    }
}
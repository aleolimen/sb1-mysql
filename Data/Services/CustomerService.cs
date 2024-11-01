using Dapper;
using BlazorCRM.Data.Models;

namespace BlazorCRM.Data.Services;

public class CustomerService
{
    private readonly DatabaseConnection _db;

    public CustomerService(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Customer>("SELECT * FROM Customers");
    }

    public async Task<Customer> GetCustomer(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<Customer>(
            "SELECT * FROM Customers WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateCustomer(Customer customer)
    {
        using var connection = _db.GetConnection();
        string sql = @"INSERT INTO Customers (Name, Email, Phone, CustomerTypeId) 
                      VALUES (@Name, @Email, @Phone, @CustomerTypeId);
                      SELECT LAST_INSERT_ID();";
        return await connection.ExecuteScalarAsync<int>(sql, customer);
    }

    public async Task UpdateCustomer(Customer customer)
    {
        using var connection = _db.GetConnection();
        string sql = @"UPDATE Customers 
                      SET Name = @Name, Email = @Email, 
                          Phone = @Phone, CustomerTypeId = @CustomerTypeId 
                      WHERE Id = @Id";
        await connection.ExecuteAsync(sql, customer);
    }

    public async Task DeleteCustomer(int id)
    {
        using var connection = _db.GetConnection();
        await connection.ExecuteAsync("DELETE FROM Customers WHERE Id = @Id", new { Id = id });
    }
}
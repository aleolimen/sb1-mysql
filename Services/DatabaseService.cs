using MySql.Data.MySqlClient;
using System.Data;

namespace CrmMaui.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = "Server=localhost;Database=blazorcrm;Uid=root;Pwd=your_password;";
    }

    public async Task<DataTable> GetCustomersAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new MySqlCommand("SELECT * FROM Customers", connection);
        using var adapter = new MySqlDataAdapter(command);
        
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public async Task<DataTable> GetProductsAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new MySqlCommand("SELECT * FROM Products", connection);
        using var adapter = new MySqlDataAdapter(command);
        
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public async Task<DataTable> GetOrdersAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new MySqlCommand("SELECT * FROM Orders", connection);
        using var adapter = new MySqlDataAdapter(command);
        
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }
}
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace BlazorCRM.Data;

public class DatabaseConnection
{
    private readonly IConfiguration _configuration;

    public DatabaseConnection(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MySqlConnection GetConnection()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        return new MySqlConnection(connectionString);
    }
}
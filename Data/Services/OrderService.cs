using Dapper;
using BlazorCRM.Data.Models;

namespace BlazorCRM.Data.Services;

public class OrderService
{
    private readonly DatabaseConnection _db;

    public OrderService(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        using var connection = _db.GetConnection();
        var orders = await connection.QueryAsync<Order>("SELECT * FROM Orders");
        foreach (var order in orders)
        {
            order.Items = (await connection.QueryAsync<OrderItem>(
                "SELECT * FROM OrderItems WHERE OrderId = @OrderId", 
                new { OrderId = order.Id })).ToList();
        }
        return orders;
    }

    public async Task<Order> GetOrder(int id)
    {
        using var connection = _db.GetConnection();
        var order = await connection.QueryFirstOrDefaultAsync<Order>(
            "SELECT * FROM Orders WHERE Id = @Id", new { Id = id });
        if (order != null)
        {
            order.Items = (await connection.QueryAsync<OrderItem>(
                "SELECT * FROM OrderItems WHERE OrderId = @OrderId", 
                new { OrderId = id })).ToList();
        }
        return order;
    }

    public async Task<int> CreateOrder(Order order)
    {
        using var connection = _db.GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            string orderSql = @"INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) 
                               VALUES (@CustomerId, @OrderDate, @TotalAmount);
                               SELECT LAST_INSERT_ID();";
            int orderId = await connection.ExecuteScalarAsync<int>(orderSql, order, transaction);

            string itemSql = @"INSERT INTO OrderItems 
                             (OrderId, ProductId, Quantity, UnitPrice, TotalPrice) 
                             VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice, @TotalPrice)";
            foreach (var item in order.Items)
            {
                item.OrderId = orderId;
                await connection.ExecuteAsync(itemSql, item, transaction);
            }

            transaction.Commit();
            return orderId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task UpdateOrder(Order order)
    {
        using var connection = _db.GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            string orderSql = @"UPDATE Orders 
                               SET CustomerId = @CustomerId, 
                                   OrderDate = @OrderDate, 
                                   TotalAmount = @TotalAmount 
                               WHERE Id = @Id";
            await connection.ExecuteAsync(orderSql, order, transaction);

            await connection.ExecuteAsync(
                "DELETE FROM OrderItems WHERE OrderId = @OrderId", 
                new { OrderId = order.Id }, transaction);

            string itemSql = @"INSERT INTO OrderItems 
                             (OrderId, ProductId, Quantity, UnitPrice, TotalPrice) 
                             VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice, @TotalPrice)";
            foreach (var item in order.Items)
            {
                await connection.ExecuteAsync(itemSql, item, transaction);
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task DeleteOrder(int id)
    {
        using var connection = _db.GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            await connection.ExecuteAsync(
                "DELETE FROM OrderItems WHERE OrderId = @Id", 
                new { Id = id }, transaction);
            await connection.ExecuteAsync(
                "DELETE FROM Orders WHERE Id = @Id", 
                new { Id = id }, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
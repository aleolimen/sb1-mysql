namespace BlazorCRM.Data.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int CustomerTypeId { get; set; }
    public CustomerType? CustomerType { get; set; }
}
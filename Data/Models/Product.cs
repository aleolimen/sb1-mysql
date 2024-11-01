namespace BlazorCRM.Data.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProductGroupId { get; set; }
    public ProductGroup? ProductGroup { get; set; }
    public decimal Cost { get; set; }
    public decimal SalePrice { get; set; }
}
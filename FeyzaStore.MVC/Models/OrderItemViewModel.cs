namespace FeyzaStore.MVC.Models;

public class OrderItemViewModel
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}